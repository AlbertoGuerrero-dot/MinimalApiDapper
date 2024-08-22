using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;

namespace MinimalAPIPeliculas.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string? connectionString;

        public UserRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<IdentityUser?> SearchUserByEmail(string normalizedEmail)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<IdentityUser>("Usuarios_BuscarPorEmail", new { normalizedEmail },
                    commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<string> CreateUser(IdentityUser user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                user.Id = Guid.NewGuid().ToString();
                await connection.ExecuteAsync("Usuarios_Crear", new
                {
                    user.Id,
                    user.Email,
                    user.NormalizedEmail,
                    user.UserName,
                    user.NormalizedUserName,
                    user.PasswordHash
                },
                    commandType: CommandType.StoredProcedure);
            }
            return user.Id;
        }
        public async Task<List<Claim>> GetClaims (IdentityUser user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var claims = await connection.QueryAsync<Claim>("Usuarios_ObtenerClaims", new { user.Id },
                    commandType: CommandType.StoredProcedure);
                    return claims.ToList();
            }
        }
        public async Task AddClaim(IdentityUser user, IEnumerable<Claim> claims)
        {
            var sql = @"INSERT INTO UsuariosClaims (UserId, ClaimType, ClaimValue) VALUES (@Id, @Type, @Value)";
            var parameters = claims.Select(c => new { user.Id, c.Type, c.Value });
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(sql, parameters);
            }
        }
        public async Task RemoveClaim(IdentityUser user, IEnumerable<Claim> claims)
        {
            var sql = @"DELETE FROM UsuariosClaims WHERE UserId = @Id AND ClaimType = @Type";
            var parameters = claims.Select(c => new { user.Id, c.Type });
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}
