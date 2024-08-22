using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

internal static class UserEndpointsHelpers
{
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