using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Diagnostics;
using MinimalAPIPeliculas.Endpoints;
using MinimalAPIPeliculas.Entities;
using MinimalAPIPeliculas.Repository;
using MinimalAPIPeliculas.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MinimalAPIPeliculas.Utilities;
using Microsoft.OpenApi.Models;
using MinimalAPIPeliculas.Swagger;

var builder = WebApplication.CreateBuilder(args);
var allowedOrigins = builder.Configuration.GetValue<string>("AllowedOrigins")!;


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(configuration =>
    {
        configuration.WithOrigins(allowedOrigins).AllowAnyHeader();
    });
    options.AddPolicy("libre", configuration =>
    {
        configuration.AllowAnyOrigin();
    }
    );
});

//builder.Services.AddOutputCache();
builder.Services.AddStackExchangeRedisOutputCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("redis"); 
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "Peliculas API",
        Description = "API para la administración de peliculas",
        Contact = new OpenApiContact
        {
            Name = "Alberto",
            Email = "alberto@email.com",
            Url = new Uri("https://www.alberto.com")
        },
        License = new OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme",
    });
    c.OperationFilter<AuthorizationFilter>();
});
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IActorRepository, ActorRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<ICommentaryRepository, CommentaryRepository>();
builder.Services.AddScoped<IErrorRepository, ErrorRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFileStorer, LocalFileStorer>();
builder.Services.AddTransient<IUsersServices, UsersServices>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddProblemDetails();
builder.Services.AddAuthentication().AddJwtBearer(options =>
    {   options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKeys = Keys.GetAllKey(builder.Configuration),
            ClockSkew = TimeSpan.Zero
        };    
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
});
builder.Services.AddTransient<IUserStore<IdentityUser>, UserStore>();
builder.Services.AddIdentityCore<IdentityUser>();
builder.Services.AddTransient<SignInManager<IdentityUser>>();


var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.Run(async context =>
{
    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
    var exception = exceptionHandlerFeature?.Error!;
    var error = new Error();
    error.Fecha = DateTime.UtcNow;
    error.MensajeDeError = exception.Message;
    error.StackTrace = exception?.StackTrace;

    var errorRepository = context.RequestServices.GetRequiredService<IErrorRepository>();
    await errorRepository.Create(error);
    await TypedResults.BadRequest(new { type = "error", message = "Ha ocurrido un error inesperado ", status = 500 }).ExecuteAsync(context);
}));
app.UseStatusCodePages();
app.UseStaticFiles();
app.UseCors();
app.UseOutputCache();
app.UseAuthorization();
app.MapGenres();
app.MapActors();
app.MapMovies();
app.MapUser();
app.MapGet("/error", () =>
{
    throw new InvalidOperationException("Error de prueba");
});
app.MapGroup("/movie/{movieId:int}/commentaries").MapCommentaries();
app.MapGet("/", [EnableCors(policyName: "libre")] () => "Hello World!").CacheOutput(c => c.Expire(TimeSpan.FromSeconds(15)));


app.Run();