using FluentValidation;
using TemplateProductName.Common;

namespace TemplateProductName.Domain.Features.CreateDinosaur
{
    public class CreateDinosaurValidator : AbstractValidator<CreateDinosaurCommand>
    {
        public CreateDinosaurValidator() =>
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithGuid()
                .MinimumLength(10)
                .WithGuid();
    }
}
