using FluentValidation;
using MinimalAPIPeliculas.DTOs;

namespace MinimalAPIPeliculas.Validations
{
    public class CreateMovieDtoValidation : AbstractValidator<CreateMovieDto>
    {
        public CreateMovieDtoValidation()
        {
            RuleFor(x => x.Titulo).NotEmpty().WithMessage(Utilities.requiredFieldMessage)
            .MaximumLength(50).WithMessage(Utilities.requiredFieldMessageMaxLength);
        }
    }
}
