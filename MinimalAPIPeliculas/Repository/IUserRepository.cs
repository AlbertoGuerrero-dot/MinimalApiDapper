using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MinimalAPIPeliculas.Repository
{
    public interface IUserRepository
    {
        Task AddClaim(IdentityUser user, IEnumerable<Claim> claims);
        Task<string> CreateUser(IdentityUser user);
        Task<List<Claim>> GetClaims(IdentityUser user);
        Task RemoveClaim(IdentityUser user, IEnumerable<Claim> claims);
        Task<IdentityUser?> SearchUserByEmail(string normalizedEmail);
    }
}