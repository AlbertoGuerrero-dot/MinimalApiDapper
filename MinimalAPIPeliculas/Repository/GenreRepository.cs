using Dapper;
using Microsoft.Data.SqlClient;
using MinimalAPIPeliculas.Entities;
using System.Data;

namespace MinimalAPIPeliculas.Repository
{
    public class GenreRepository : IGenreRepository

    {
        private readonly string? connectionString;

        public GenreRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<List<Genre>> GetAll()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var genres = await connection.QueryAsync<Genre>("Generos_ObtenerTodos", commandType: CommandType.StoredProcedure);
                return genres.ToList();
            }
        }
        public async Task<Genre?> GetById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var genre = await connection.QueryFirstOrDefaultAsync<Genre>("Generos_ObtenerPorId", new { id }, commandType: CommandType.StoredProcedure);
                return genre;
            }
        }
        public async Task<int> CreateGenre(Genre genre)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var id = await connection.QuerySingleAsync<int>("Generos_Crear", new { genre.Nombre },
                    commandType: CommandType.StoredProcedure);
                genre.Id = id;
                return id;
            }
        }

        public async Task<bool> Exist(int id)
        {
            using var connection = new SqlConnection(connectionString);
            {
                var exist = await connection.QuerySingleAsync<bool>("Generos_ExistePorID", new { id }, commandType: CommandType.StoredProcedure);
                return exist;
            }
        }

        public async Task UpdateGenre(Genre genre)
        {
            using var connection = new SqlConnection(connectionString);
            {
                await connection.ExecuteAsync("Generos_Actulizar", genre, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync("Generos_Borrar", new { id }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<List<int>> GenreExists(List<int> GenerosIds)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            foreach (var id in GenerosIds)
            {
                dt.Rows.Add(id);
            }
            using (var connection = new SqlConnection(connectionString))
            {
                var idsExist = await connection.QueryAsync<int>("Generos_ObtenerVariosPorId",
                    new { GenerosIds = dt },
                    commandType: CommandType.StoredProcedure);
                return idsExist.ToList();
            }
        }

        public async Task<bool> Exist(int id, string nombre)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var exist = await connection.QuerySingleAsync<bool>("Geenros_ExistePorIdYNombre", 
                    new { id, nombre }, commandType: CommandType.StoredProcedure);
                return exist;
            }
        }
    }
}
