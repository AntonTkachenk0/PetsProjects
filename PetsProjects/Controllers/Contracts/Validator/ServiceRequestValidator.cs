using FluentValidation;

namespace PetsProjects.Controllers.Contracts.Validator
{
    public class ServiceRequestValidator : AbstractValidator<ServiceRequest>
    {

        public ServiceRequestValidator()
        {
            RuleFor(s => s.Title)
                .NotEmpty()
                .MaximumLength(120);
            RuleFor(s => s.Cost)
                .GreaterThan(0)
                .WithMessage("Цена должна быть больше нуля");
        }

    }
}
