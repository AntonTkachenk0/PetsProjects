using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetsProjects.Controllers.Contracts;
using PetsProjects.DataBaseAccess.Postgres.Repositories;
using PetsProjects.Models;

namespace PetsProjects.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PetsController : ControllerBase
    {
        private readonly IPetRepository _repository;
        private readonly IValidator<PetRequest> _validator;
        private readonly IValidator<FileUploadDto> _fileValidator;
        private readonly IOptions<S3Options> _options;
        private readonly IAmazonS3 _s3;
        private readonly ILogger<PetsController> _logger;
        private readonly S3Options _s3Options;
        public PetsController(
            IPetRepository repository, 
            IValidator<PetRequest> validator,
            IValidator<FileUploadDto> fileValidator,
            IOptions<S3Options> options, 
            IAmazonS3 s3,
            ILogger<PetsController> logger)
        {
            _s3Options = options.Value;
            _repository = repository;
            _validator = validator;
            _fileValidator = fileValidator;
            _options = options;
            _s3 = s3;
            _logger = logger;
        }

    
        [HttpGet]
        public async Task<ActionResult<List<PetResponse>>> Get(CancellationToken ct)
        {
            var userId = User.FindFirst("userId")?.Value;
            
            if (userId == null)
            {
                return Unauthorized("Пользователь не вошел");
            }
            
            var userIdGuid = Guid.Parse(userId);
            
            var pets = await _repository.GetForCurrentUser(userIdGuid, ct);

            var response = pets.Select(p => new PetResponse(p.Id, p.Name, p.Breed, p.Gender,  p.Photo_Id! , p.Id_User));

            return Ok(response);    
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Guid>> CreatePet(
            [FromBody] PetRequest request,
            CancellationToken ct)
        {
            try
            {
                var resultValidate = await _validator.ValidateAsync(request, ct);

                if (!resultValidate.IsValid)
                {
                    foreach (var failure in resultValidate.Errors)
                    {
                        Console.WriteLine(
                            $"Свойство {failure.PropertyName} не прошло валидацию. Ошибка {failure.ErrorMessage} ");
                    }

                    return BadRequest(resultValidate.Errors);
                }

                var userId = User.FindFirst("userId")?.Value;
                
                _logger.LogInformation("Попытка запарсить id {UserId}", userId);
                
                var userIdGuid =  Guid.Parse(userId);
                
                _logger.LogInformation("Парс выполнен успешно");
              
                if (userIdGuid == Guid.Empty)
                {
                    _logger.LogError("Ошибка создания питомца. Некорректный id {UserId}", userId);
                    return BadRequest("Не выбран владелец.");
                }

                var pet = Pet.Create(
                    Guid.NewGuid(),
                    request.Name,
                    request.Breed,
                    request.Gender,
                    "defaultPet",
                    userIdGuid);

                if (!pet.IsSuccess)
                {
                    Console.WriteLine("Ошибка");
                    Console.WriteLine(pet.Error);
                    return BadRequest(pet);
                }

                await _repository.Create(pet.Value!, ct);
                return Ok(pet.Value);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task <ActionResult<Guid>> Update(
            Guid id,
            [FromBody] PetRequest request,
            CancellationToken ct)
        {
            try
            {


                var userId = User.FindFirst("UserId")?.Value;

                _logger.LogInformation("Попытка запарсить id {UserId}", userId);

                var userIdGuid = Guid.Parse(userId);

                await _repository.Update(id, request.Name, request.Breed, request.Gender, userIdGuid, ct);
                return Ok(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }
        
        [HttpPut("{id:guid}/photo")]
        public async Task<ActionResult<Guid>> UpdatePhoto(
            Guid id, 
            [FromBody] string photoId,
            CancellationToken ct)
        {
            await _repository.UpdatePhoto(id, photoId,  ct);
            return Ok(id);
        }

    }
}
