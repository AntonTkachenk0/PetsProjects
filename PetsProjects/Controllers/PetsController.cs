using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetsProjects.Controllers.Contracts;
using PetsProjects.DataBaseAccess.Postgres.Repositories;
using PetsProjects.Models;

namespace PetsProjects.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class PetsController : ControllerBase
    {
        private readonly IPetRepository _repository;
        private readonly IValidator<PetRequest> _validator;

        public PetsController(IPetRepository repository, IValidator<PetRequest> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<List<PetResponse>>> Get()
        {
            var pets = await _repository.Get();

            var response = pets.Select(p => new PetResponse(p.Id, p.Name, p.Breed, p.Gender, p.Id_User));

            return Ok(response);    
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreatePet([FromBody] PetRequest request)
        {


            var resultValidate = await _validator.ValidateAsync(request);

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
                request.Id_User);

            if (!pet.IsSuccess)
            {
                Console.WriteLine("Ошибка");
                Console.WriteLine(pet.Error);
                return BadRequest(pet);
            }

            await _repository.Create(pet.Value!);
            return Ok(pet.Value);
        }

        [HttpPut("{id:guid}")]
        public async Task <ActionResult<Guid>> Update(Guid id, [FromBody] PetRequest request)
        {
            await _repository.Update(id, request.Name, request.Breed, request.Gender, request.Id_User);
            return Ok(id);
        }
    }
}
