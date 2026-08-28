using FluentValidation;
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

    public S3Controller(IOptions<S3Options> options, IAmazonS3 s3, IUserRepository userRepository, IPetRepository petRepository,IValidator<FileUploadDto> validator)
    {
        _s3Options = options.Value;
        _s3 = s3;
        _userRepository = userRepository;
        _petRepository = petRepository;
        _validator = validator;
    }

    [HttpPost("image/")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<PutObjectResponse>> Upload(
        [FromForm] FileUploadDto uploadDto,
        CancellationToken ct)
    {
        var resultValidate = await _validator.ValidateAsync(uploadDto, ct);

        if (!resultValidate.IsValid)
        {
            foreach (var failure in resultValidate.Errors)
            {
                Console.WriteLine($"Свойство {failure.PropertyName} не прошло валидацию. Ошибка {failure.ErrorMessage} ");
            }
            return BadRequest(resultValidate.Errors);
        }
        
        try
        {
            var newGuid = Guid.NewGuid().ToString();

            var photoId = string.Empty;
            
            
            var requestCreate = new PutObjectRequest
            {
                BucketName = _s3Options.BucketName,
                Key = $"{newGuid}",
                ContentType = uploadDto.File.ContentType,
                InputStream = uploadDto.File.OpenReadStream(),
            };

            switch (uploadDto.Role.ToLower())
            {
                case "user":
                    photoId = await _userRepository.TakePhotoId(uploadDto.PetOrUserId, ct);
                    Console.WriteLine(photoId);
                    await _userRepository.UpdatePhoto(uploadDto.PetOrUserId, newGuid, ct);
                    break;
                case "pet":
                    photoId = await _petRepository.TakePhotoId(uploadDto.PetOrUserId, ct);
                    Console.WriteLine(photoId);
                    await _petRepository.UpdatePhoto(uploadDto.PetOrUserId, newGuid, ct);
                    break;
            }
            

            Console.WriteLine(photoId);
            if (photoId != "defaultPet" && photoId != "default" )
            {
                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = _s3Options.BucketName,
                    Key = photoId
                };
                await _s3.DeleteObjectAsync(deleteRequest, ct);
            }

            var responseCreate = await _s3.PutObjectAsync(requestCreate, ct);

            if (responseCreate.HttpStatusCode == HttpStatusCode.OK)
            {
                return Ok(responseCreate);
            }
            else
            {
                Console.WriteLine($"Ошибка тута. создание - {responseCreate.HttpStatusCode}") ;
                return BadRequest(
                    $"Ошибка создания или удаления.  создание - {responseCreate.HttpStatusCode}");
            }
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }

    [HttpGet("image/{photo_id}")]
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