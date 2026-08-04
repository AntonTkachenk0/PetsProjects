using FluentValidation;

namespace PetsProjects.Controllers.Contracts.Validator
{
    public class PetRequestValidator : AbstractValidator<PetRequest>
    {

        public PetRequestValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .MaximumLength(120);
            RuleFor(p => p.Breed)
                .NotEmpty()
                .MaximumLength(120);
            RuleFor(p => p.Gender)
                .NotEmpty()
                .MaximumLength(120);

        }
    }
}