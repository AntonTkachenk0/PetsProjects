using Microsoft.AspNetCore.Mvc;
using PetsProjects.Abstractions;
using PetsProjects.Controllers.Contracts;
using PetsProjects.Models;

namespace PetsProjects.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class PetsService : ControllerBase
    {
        private readonly IPetRepository _repository;

        public PetsService(IPetRepository repository)
        {
            _repository = repository;
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
            var pet = Pet.Create(
                Guid.NewGuid(), 
                request.Name,
                request.Breed,
                request.Gender, 
                request.Id_User);

            if (pet.IsSuccess)
            {
                await _repository.Create(pet.Value!);
                return Ok(pet);
            }

            return BadRequest(pet);
        }
    }
}
