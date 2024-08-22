using FluentValidation;
using MinimalAPIPeliculas.DTOs;

namespace MinimalAPIPeliculas.Validations
{
    public class UserCredentialsDtoValidator: AbstractValidator<UserCredentialDto>
    {
        public UserCredentialsDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(Utilities.requiredFieldMessage)
                .MaximumLength(256).WithMessage(Utilities.requiredFieldMessageMaxLength)
                .EmailAddress().WithMessage(Utilities.EmailMessage);
            RuleFor(x => x.Password).NotEmpty().WithMessage(Utilities.requiredFieldMessage);
        }
    }
}
