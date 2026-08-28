using FluentValidation;

namespace PetsProjects.Controllers.Contracts.Validator;

public class ServiceJournalRequestValidator
    : AbstractValidator<ServiceJournalRequest>
{
    public ServiceJournalRequestValidator()
    {
        RuleFor(p => p.Id_Pet)
            .NotNull();
        RuleFor(p => p.Id_Service)
            .NotNull();
        RuleFor(p => p.VisitDate)
            .NotNull();
        RuleFor(p => p.VisitDate)
            .Must(a => a.Date >= DateTime.Today);
        RuleFor(p => p.VisitDate)
            .Must(a => a.Hour >= 9 && a.Hour < 20);

    }
}

