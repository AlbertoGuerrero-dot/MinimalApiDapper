using FluentValidation;
using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Repository;

namespace MinimalAPIPeliculas.Validations
{
    public class CreateGenreDtoValidation : AbstractValidator<CreateGenreDto>
    {
        public CreateGenreDtoValidation(IGenreRepository genreRepository, IHttpContextAccessor httpContextAccessor)
        {
            var routeValues = httpContextAccessor.HttpContext?.Request.RouteValues["id"];   
            var id = 0;
            if (routeValues is string stringValue)
            {
                int.TryParse(stringValue, out id);
            }
            RuleFor(x => x.Nombre).NotEmpty().WithMessage(Utilities.requiredFieldMessage)
            .MaximumLength(50).WithMessage(Utilities.requiredFieldMessageMaxLength)
            .Must(Utilities.capitalLetter).WithMessage(Utilities.firstLetterCapitalMessage)
            .MustAsync(async (nombre, _) =>
            {
                var exist = await genreRepository.Exist(id, nombre);
                return !exist;
            }).WithMessage(g => $"El género con nombre {g.Nombre} ya existe");
        } 
    }
}
