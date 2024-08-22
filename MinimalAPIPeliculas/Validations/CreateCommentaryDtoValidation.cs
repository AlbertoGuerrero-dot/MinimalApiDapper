using FluentValidation;
using MinimalAPIPeliculas.DTOs;

namespace MinimalAPIPeliculas.Validations
{
    public class CreateCommentaryDtoValidation: AbstractValidator<CreateCommentaryDto>
    {
        public CreateCommentaryDtoValidation()
        {
            RuleFor(x => x.Cuerpo).NotEmpty().WithMessage(Utilities.requiredFieldMessage);
        }
    }
}
