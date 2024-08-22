using FluentValidation;
using MinimalAPIPeliculas.DTOs;

namespace MinimalAPIPeliculas.Validations
{
    public class CreateActorDtoValidation: AbstractValidator<CreateActorDto>
    {
        public CreateActorDtoValidation()
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage(Utilities.requiredFieldMessage)
                .MaximumLength(50).WithMessage(Utilities.requiredFieldMessageMaxLength);
            var minumumdate = new DateTime(1900, 01, 01);
            RuleFor(x => x.FechaNacmiento).NotEmpty().WithMessage("El campo {PropertyName} es requerido")
                .LessThan(DateTime.Now).WithMessage("El campo {PropertyName} no puede ser mayor a la fecha actual")
                .GreaterThan(minumumdate).WithMessage(Utilities.GreaterThanOrEqual(minumumdate));
        }
    }
}
