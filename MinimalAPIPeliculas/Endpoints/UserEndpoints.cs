using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Filters;
using MinimalAPIPeliculas.Services;
using MinimalAPIPeliculas.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MinimalAPIPeliculas.Endpoints
{
    public static class UserEndpoints
    {
        public static RouteGroupBuilder MapUser(this WebApplication app)
        {
            var userEnpoints = app.MapGroup("/user");
            userEnpoints.MapPost("/login", Login).AddEndpointFilter<ValidationFilter<UserCredentialDto>>();
            userEnpoints.MapPost("/register", Register).AddEndpointFilter<ValidationFilter<UserCredentialDto>>();
            userEnpoints.MapPost("/admin", Admin).AddEndpointFilter<ValidationFilter<EditClaimDto>>().RequireAuthorization("Admin");
            userEnpoints.MapPost("/removeadmin", RemoveAdmin).AddEndpointFilter<ValidationFilter<EditClaimDto>>().RequireAuthorization("Admin");
            userEnpoints.MapGet("/renewtoken", RenewToken).RequireAuthorization();
            static async Task<Results<Ok<AuthenticationResponseDto>, BadRequest<IEnumerable<IdentityError>>>> Register(UserCredentialDto userCredentialDto,
                [FromServices] UserManager<IdentityUser> userManager, IConfiguration configuration)
            {
                var user = new IdentityUser
                {
                    UserName = userCredentialDto.Email,
                    Email = userCredentialDto.Email
                };
                var result = await userManager.CreateAsync(user, userCredentialDto.Password);
                if (result.Succeeded)
                {
                    var credentialsResponse = await BuildToken(userCredentialDto, configuration, userManager);
                    return TypedResults.Ok(credentialsResponse);
                }
                else
                {
                    return TypedResults.BadRequest(result.Errors);
                }
            }
            static async Task<Results<Ok<AuthenticationResponseDto>, BadRequest<string>>> Login(UserCredentialDto userCredentialDto,
                [FromServices] SignInManager<IdentityUser> signInManager, 
                [FromServices] UserManager<IdentityUser> userManager, IConfiguration configuration)
            {
                var user = await userManager.FindByEmailAsync(userCredentialDto.Email);
                if (user is  null)
                {
                    return TypedResults.BadRequest("Login incorrecto");
                }
                var result = await signInManager.CheckPasswordSignInAsync(user, userCredentialDto.Password, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var authenticationResponse = await BuildToken(userCredentialDto, configuration, userManager);
                    return TypedResults.Ok(authenticationResponse);
                }
                else 
                {
                    return TypedResults.BadRequest("Login incorrecto");
                }
            }
            static async Task<Results<NoContent, NotFound>> Admin(EditClaimDto editClaimDto, 
                [FromServices] UserManager<IdentityUser> userManager)
            {
                var user = await userManager.FindByEmailAsync(editClaimDto.Email);
                if (user is null)
                {
                    return TypedResults.NotFound();
                }
                await userManager.AddClaimAsync(user, new Claim("Admin", "true"));
                return TypedResults.NoContent();
            }
            static async Task<Results<NoContent, NotFound>> RemoveAdmin(EditClaimDto editClaimDto,
                [FromServices] UserManager<IdentityUser> userManager)
            {
                var user = await userManager.FindByEmailAsync(editClaimDto.Email);
                if (user is null)
                {
                    return TypedResults.NotFound();
                }
                await userManager.RemoveClaimAsync(user, new Claim("Admin", "true"));
                return TypedResults.NoContent();
            }
            return userEnpoints;
        }
        public async static Task<Results<Ok<AuthenticationResponseDto>, NotFound>> RenewToken (
            IUsersServices usersServices, IConfiguration configuration, [FromServices] UserManager<IdentityUser> userManager)
        {
            var user = await usersServices.GetUser();
            if ( user is null)
            {
                return TypedResults.NotFound();
            }
            var userCredentialDto = new UserCredentialDto { Email = user.Email! };
            var authenticationResponseDto = await BuildToken(userCredentialDto, configuration, userManager); 
            return TypedResults.Ok(authenticationResponseDto);
        }
        private async static Task<AuthenticationResponseDto> 
            BuildToken(UserCredentialDto userCredentialDto, 
            IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            var claims = new List<Claim>
            {
                new Claim("email", userCredentialDto.Email),
                new Claim("random2 fsfsdf", "random")
            };
            var user = await userManager.FindByEmailAsync(userCredentialDto.Email);
            var claimsDB = await userManager.GetClaimsAsync(user!);
            claims.AddRange(claimsDB);
            var key = Keys.GetKey(configuration);
            var creds = new SigningCredentials(key.First(), SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddYears(1);
            var securityToken = new JwtSecurityToken(issuer: null,
                                                     claims: claims,
                                                     expires: expiration,
                                                     signingCredentials: creds);
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken); 
            return new AuthenticationResponseDto { Token = token, Expiracion = expiration };
        }
    }
}
