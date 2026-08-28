using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsProjects.Controllers.Contracts;
using PetsProjects.DataBaseAccess.Postgres.Repositories;
using PetsProjects.Models;

namespace PetsProjects.Controllers;

[ApiController]
[Route("[controller]")]
public class ServiceJournalController : ControllerBase
{
    private readonly IServiceJournalRepository _repository;
    private readonly IValidator<ServiceJournalRequest> _validator;
    private readonly ILogger<ServiceJournalController> _logger;

    public ServiceJournalController(IServiceJournalRepository  repository, IValidator<ServiceJournalRequest> validator,ILogger<ServiceJournalController> logger)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<ServiceJournalResponse>> Create([FromBody] ServiceJournalRequest request , CancellationToken ct)
    {
        try
        {   
            
            _logger.LogInformation("Входные данные:  {request}", request);
            
            var utcDate = DateTime.SpecifyKind(request.VisitDate, DateTimeKind.Utc);
            
            var resultValidate = await _validator.ValidateAsync(request, ct);

            if (!resultValidate.IsValid)
            {
                _logger.LogWarning("Ошибка валидации: {resultValidate.Errors}" , resultValidate.Errors);
                return BadRequest(resultValidate);
            }
            
            var serviceJournal = ServiceJournal.Create(
                Guid.NewGuid(),
                request.Id_Pet,
                request.Id_Service,
                utcDate);
            
            if (!serviceJournal.IsSuccess)
            {
                _logger.LogWarning("Ошибка валидации: {serviceJournal.Error}",serviceJournal.Error);
                return BadRequest(serviceJournal);
            }

            await _repository.Create(serviceJournal.Value!, ct);
            _logger.LogInformation("Запись успешно создалась!");
            return Ok(serviceJournal.Value);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка сервера {request}", request);
            return StatusCode(500, "Внутренняя ошибка сервера");
        }
    }
    
    [HttpGet]
    public async Task<ActionResult<List<ServiceJournalResponse>>> Get(CancellationToken ct)
    {

        var result = await _repository.GetAll(ct);

        var response = result.Select(s => new ServiceJournalResponse(s.Id, s.Id_Pet, s.Id_Service, s.VisitDate));
        
        return Ok(response);
    }
    
}