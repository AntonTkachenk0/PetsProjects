using FluentValidation;
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
        private readonly S3Options _s3Options;
        public PetsController(
            
            IPetRepository repository, 
            IValidator<PetRequest> validator,
            IValidator<FileUploadDto> fileValidator,
            IOptions<S3Options> options, 
            IAmazonS3 s3)
        {
            _s3Options = options.Value;
            _repository = repository;
            _validator = validator;
            _fileValidator = fileValidator;
            _options = options;
            _s3 = s3;
        }

       [HttpGet("{photo_id}/photo")]
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
        
        [HttpGet]
        public async Task<ActionResult<List<PetResponse>>> Get(CancellationToken ct)
        {
            var pets = await _repository.Get(ct);

            var response = pets.Select(p => new PetResponse(p.Id, p.Name, p.Breed, p.Gender,  p.Photo_Id! , p.Id_User));

            return Ok(response);    
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreatePet(
            [FromBody] PetRequest request,
            CancellationToken ct)
        {


            var resultValidate = await _validator.ValidateAsync(request, ct);

            if (!resultValidate.IsValid)
            {
                foreach (var failure in resultValidate.Errors)
                {
                    Console.WriteLine($"Свойство {failure.PropertyName} не прошло валидацию. Ошибка {failure.ErrorMessage} ");
                }
                return BadRequest(resultValidate.Errors);
            }

            var pet = Pet.Create(
                Guid.NewGuid(),
                request.Name,
                request.Breed,
                request.Gender,
                "defaultPet",
                request.Id_User);

            if (!pet.IsSuccess)
            {
                Console.WriteLine("Ошибка");
                Console.WriteLine(pet.Error);
                return BadRequest(pet);
            }

            await _repository.Create(pet.Value!, ct);
            return Ok(pet.Value);
        }

        [HttpPut("{id:guid}")]
        public async Task <ActionResult<Guid>> Update(
            Guid id,
            [FromBody] PetRequest request,
            CancellationToken ct)
        {
            await _repository.Update(id, request.Name, request.Breed, request.Gender, request.Id_User, ct);
            return Ok(id);
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
