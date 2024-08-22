using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using MinimalAPIPeliculas.Entities;
using System.Data;

namespace MinimalAPIPeliculas.Repository
{
    public class CommentaryRepository : ICommentaryRepository
    {
        private readonly string? connectionString;

        public CommentaryRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<Commentary>> GetAllComentaries(int peliculaId)
        {

            using (var connection = new SqlConnection(connectionString))
            {
                var comentaries = await connection.QueryAsync<Commentary>("Comentarios_ObtenerTodos", new { peliculaId },
                    commandType: CommandType.StoredProcedure);
                return comentaries.ToList();
            }
        }

        public async Task<Commentary?> GetComentaryById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var comentary = await connection.QueryFirstOrDefaultAsync<Commentary>("Comentarios_ObtenerPorId",
                    new { id }, commandType: CommandType.StoredProcedure);
                return comentary;
            }
        }
        public async Task<int> CreateComentary(Commentary commentary)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var id = await connection.QuerySingleAsync<int>("Comentarios_Crear", new { commentary.PeliculaId, commentary.Cuerpo, commentary.UserId },
                    commandType: CommandType.StoredProcedure);
                commentary.Id = id;
                return id;
            }
        }
        public async Task<bool> Exist(int id)
        {
            using var connection = new SqlConnection(connectionString);
            {
                var exist = await connection.QuerySingleAsync<bool>("Comentarios_ExistePorID", new { id }, commandType: CommandType.StoredProcedure);
                return exist;
            }
        }

        public async Task UpdateComentary(Commentary commentary)
        {
            using var connection = new SqlConnection(connectionString);
            {
                await connection.ExecuteAsync("Comentarios_Actualizar", new {commentary.Id, commentary.PeliculaId, commentary.Cuerpo}, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task DeleteComentary(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync("Comentarios_Eliminar", new { id }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
