
using FluentValidation;
using MinimalAPIPeliculas.DTOs;

namespace MinimalAPIPeliculas.Filters
{
    public class ValidationFilter<T> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validation = context.HttpContext.RequestServices.GetService<IValidator<T>>();
            if (validation is null)
            {
                return await next(context);
            }
            var inputToValidate = context.Arguments.OfType<T>().FirstOrDefault();
            if (inputToValidate is null)
            {
                return TypedResults.Problem("Input not found");
            }
            var validationresult = await validation.ValidateAsync(inputToValidate);
            if (!validationresult.IsValid)
            {
                return TypedResults.ValidationProblem(validationresult.ToDictionary());
            }
            return await next(context);
        }
    }
}
