using Microsoft.IdentityModel.Tokens;

namespace MinimalAPIPeliculas.Utilities
{
    public static class HttpContextExtensionsUtilities
    {
        public static T ExtractDefaultValue<T>(this  HttpContext httpContext, string fieldName, T DefaultValue )
            where T : IParsable<T>
        {
            var value = httpContext.Request.Query[fieldName];
            if (value.IsNullOrEmpty())
            {
                return DefaultValue;
            }
            return T.Parse(value!, null);
        }
    }
}
