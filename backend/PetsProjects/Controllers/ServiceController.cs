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
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(IServiceRepository repository, IValidator<ServiceRequest> validator, ILogger<ServiceController> logger)
        {
            _repository = repository;
            _validator = validator;
            _logger = logger;
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
            try
            {
                var resultValidate = await _validator.ValidateAsync(request, ct);
                _logger.LogInformation("Входные данные: {request}", request);
                if (!resultValidate.IsValid)
                {
                    foreach (var failure in resultValidate.Errors)
                    {
                        _logger.LogInformation("Свойство {failure.PropertyName} не прошло валидацию. Ошибка {failure.ErrorMessage}", failure.PropertyName, failure.ErrorMessage);
                    }

                    _logger.LogInformation("Не прошло валидацию. Входные данные: {request}. Ошибки валидации: {error}", request, resultValidate.Errors);
                    return BadRequest(resultValidate.Errors);
                }

                var service = Service.Create(
                    Guid.NewGuid(),
                    request.Title,
                    request.Cost);

                if (!service.IsSuccess)
                {
                    _logger.LogWarning("Ошибка создания. Ошибка: {service.Error}", service.Error);
                    return BadRequest(service);
                }

                await _repository.Create(service.Value!, ct);
                _logger.LogInformation("Услуга успешно создана!");
                return Ok(service);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка при создании услуги: {request}" , request);
                return  BadRequest(e.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] ServiceRequest request, CancellationToken ct)
        {
            await _repository.Update(id, request.Title, request.Cost, ct);
            return Ok(id);
        }
    }
}
