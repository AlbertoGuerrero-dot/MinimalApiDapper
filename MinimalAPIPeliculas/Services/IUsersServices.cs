using Microsoft.AspNetCore.Identity;

namespace MinimalAPIPeliculas.Services
{
    public interface IUsersServices
    {
        Task<IdentityUser?> GetUser();
    }
}