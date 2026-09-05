using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using PetsProjects.Abstractions;
using PetsProjects.Controllers.Contracts;
using PetsProjects.DataBaseAccess.Postgres.Repositories;

namespace PetsProjects.Controllers;

[ApiController]
[Route("api/s3")]
public class S3Controller : ControllerBase
{
    private readonly S3Options _s3Options;
    private readonly IAmazonS3 _s3;
    private readonly IUserRepository _userRepository;
    private readonly IPetRepository _petRepository;
    private readonly IValidator<FileUploadDto> _validator;
    private readonly ILogger _logger;

    public S3Controller(IOptions<S3Options> options, IAmazonS3 s3, IUserRepository userRepository, IPetRepository petRepository,IValidator<FileUploadDto> validator, ILogger<S3Controller> logger)
    {
        _s3Options = options.Value;
        _s3 = s3;
        _userRepository = userRepository;
        _petRepository = petRepository;
        _validator = validator;
        _logger = logger;
    }

    [HttpPost("image/")]
    [Authorize]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<PutObjectResponse>> Upload(
        [FromForm] FileUploadDto uploadDto,
        CancellationToken ct)
    {
        var resultValidate = await _validator.ValidateAsync(uploadDto, ct);
        
        _logger.LogInformation("Полученный файл: {File}", uploadDto);
        
        if (!resultValidate.IsValid)
        {
            foreach (var failure in resultValidate.Errors)
            {
               _logger.LogError($"Свойство {failure.PropertyName} не прошло валидацию. Ошибка {failure.ErrorMessage} ");
            }
            return BadRequest(resultValidate.Errors);
        }
        
        try
        {
            var userId = User.FindFirst("userId")?.Value;
                
            _logger.LogInformation("Попытка запарсить id при изменении фото {UserId}", userId);

            if (userId == null)
            {
                _logger.LogError("Полученный id равен null");
                return Unauthorized("Ошибка авторизации.");
            }
            var userIdGuid =  Guid.Parse(userId);
            
            var newGuid = Guid.NewGuid().ToString();

            _logger.LogInformation("Новый id для фото {NewGuid}", newGuid);
            
            var photoId = string.Empty;
            
            
            var requestCreate = new PutObjectRequest
            {
                BucketName = _s3Options.BucketName,
                Key = $"{newGuid}",
                ContentType = uploadDto.File.ContentType,
                InputStream = uploadDto.File.OpenReadStream(),
            };

            _logger.LogInformation("Корзина для создания: {RequestCreate}",  requestCreate);
            
            switch (uploadDto.Role.ToLower())
            {
                case "user":
                    photoId = await _userRepository.TakePhotoId(userIdGuid, ct);
                    _logger.LogInformation("Изменения для пользователя. Id пользователя {UserId} - новое фото {PhotoId}",userIdGuid, photoId);
                    await _userRepository.UpdatePhoto(userIdGuid, newGuid, ct);
                    
                    break;
                case "pet":
                    if (uploadDto.PetId == null)
                    {
                        _logger.LogError("PetId = null");
                        return BadRequest("PetId = null");
                    }
                    photoId = await _petRepository.TakePhotoId(uploadDto.PetId, ct);
                    _logger.LogInformation("Изменения для питомца. Id питомца {UserId}  - новое фото {PhotoId}",uploadDto.PetId, photoId);
                    await _petRepository.UpdatePhoto(uploadDto.PetId, newGuid, ct);
                    break;
            }
            

            
            if (photoId != "defaultPet" && photoId != "default" )
            {
                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = _s3Options.BucketName,
                    Key = photoId
                };
                _logger.LogInformation("Попытка удаления старого фото... Название коризы - {S3Optins}, Id photo {PhotoId}", _s3Options.BucketName, photoId);
                await _s3.DeleteObjectAsync(deleteRequest, ct);
                _logger.LogInformation("Старое фото было успешно удалено. Id фото {PhotoId}", photoId);
            }

            
            _logger.LogInformation("Попытка добавить фото...");
            var responseCreate = await _s3.PutObjectAsync(requestCreate, ct);

            if (responseCreate.HttpStatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation("Фото успешно добавлено!");
                return Ok(responseCreate);
            }
            else
            {
                _logger.LogWarning($"Ошибка тут. Cоздание - {responseCreate.HttpStatusCode}") ;
                return BadRequest(
                    $"Ошибка создания или удаления.  создание - {responseCreate.HttpStatusCode}");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message,  "Ошибка. {UploadDto}", uploadDto.PetId);
            return StatusCode(500, new { error = e.Message });
        }
    }

    [HttpGet("image/{photo_id}")]
    [Authorize]
    public async Task<IActionResult> GetImage([FromRoute] string photo_id, CancellationToken ct)
    {

            
        var getRequest = new GetObjectRequest
        {
            BucketName = _s3Options.BucketName,
        };

        getRequest.Key = photo_id switch
        {
            "defaultPet" => "defaultPet",
            "default" => "default",
            _ => $"{photo_id}"
        };

        var response = await _s3.GetObjectAsync(getRequest, ct);
        
        if (response.HttpStatusCode == HttpStatusCode.OK)
            return File(response.ResponseStream, response.Headers["Content-Type"]);

        return BadRequest();
    }

    [HttpDelete("image/{id}")]
    public async Task<IActionResult> DeleteImage([FromRoute] int id)
    {
        var objectRequest = new DeleteObjectRequest
        {
            BucketName = _s3Options.BucketName,
            Key = $"{id}"
        };
        var response = await _s3.DeleteObjectAsync(objectRequest);
        return response.HttpStatusCode == HttpStatusCode.NoContent ? NoContent() : BadRequest();
    }
}