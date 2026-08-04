using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetsProjects.Abstractions;
using PetsProjects.Controllers.Contracts;
using PetsProjects.Models;

namespace PetsProjects.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository _repository;
        private readonly IValidator<ServiceRequest> _validator;

        public ServiceController(IServiceRepository repository, IValidator<ServiceRequest> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<List<ServiceResponse>>> Get(CancellationToken ct)
        {
            var service = await _repository.Get(ct);

            var response = service.Select(s => new ServiceResponse(s.Id, s.Title, s.Cost));

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateService([FromBody] ServiceRequest request, CancellationToken ct)
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

            var service = Service.Create(
                Guid.NewGuid(),
                request.Title,
                request.Cost);

            if (!service.IsSuccess)
            {
                Console.WriteLine("Ошибка");
                return BadRequest(service);
            }

            await _repository.Create(service.Value!, ct);
            return Ok(service);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] ServiceRequest request, CancellationToken ct)
        {
            await _repository.Update(id, request.Title, request.Cost, ct);
            return Ok(id);
        }
    }
}
