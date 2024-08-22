using FluentValidation;
using MinimalAPIPeliculas.DTOs;

namespace MinimalAPIPeliculas.Validations
{
    public class EditClaimDtoValidation: AbstractValidator<EditClaimDto>
    {
        public EditClaimDtoValidation()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(Utilities.requiredFieldMessage)
                .MaximumLength(256).WithMessage(Utilities.requiredFieldMessageMaxLength)
                .EmailAddress().WithMessage(Utilities.EmailMessage);
        }
    }
}
