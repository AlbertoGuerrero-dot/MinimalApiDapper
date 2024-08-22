using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace MinimalAPIPeliculas.Utilities
{
    public static class SwaggerExtensions
    {
        public static TBuider AddMoviesParametersPagination<TBuider>(this TBuider buider) where TBuider : IEndpointConventionBuilder
        {

            return buider.WithOpenApi(options =>
            {
                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "page",
                    In = ParameterLocation.Query,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer",
                        Default = new OpenApiInteger(1)
                    }
                });
                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "recordsPerPage",
                    In = ParameterLocation.Query,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer",
                        Default = new OpenApiInteger(10)
                    }
                });
                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "Titulo",
                    In = ParameterLocation.Query,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "string"
                    }
                });
                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "enCines",
                    In = ParameterLocation.Query,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "boolean"
                    }
                });
                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "proximosEstrenos",
                    In = ParameterLocation.Query,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "boolean"
                    }
                });
                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "generoId",
                    In = ParameterLocation.Query,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer"
                    }
                });
                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "campoOrdenar",
                    In = ParameterLocation.Query,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Enum = new List<IOpenApiAny>
                        {
                            new OpenApiString("Titulo"),
                            new OpenApiString("FechaLanzamiento")
                        }
                    }
                });
                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "ordenAscendente",
                    In = ParameterLocation.Query,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "boolean",
                        Default = new OpenApiBoolean(true)
                    }
                });
                return options;
            });
        }
        public static TBuider AddParametersPagination<TBuider>(this TBuider buider) where TBuider : IEndpointConventionBuilder
        {

            return buider.WithOpenApi(options =>
            {
                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "page",
                    In = ParameterLocation.Query,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer",
                        Default = new OpenApiInteger(1)
                    }
                });
                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "recordsPerPage",
                    In = ParameterLocation.Query,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer",
                        Default = new OpenApiInteger(10)
                    }
                });
                return options;
            });
        }
    }
}
