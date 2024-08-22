using Dapper;
using Microsoft.Data.SqlClient;
using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Entities;
using System.Data;

namespace MinimalAPIPeliculas.Repository
{
    public class ActorRepository : IActorRepository
    {
        private readonly string? connectionString;
        private readonly HttpContext httpContentAccesor;

        public ActorRepository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
            httpContentAccesor = httpContextAccessor.HttpContext!;
        }
        public async Task<List<Actor>> GetAll(paginationDto paginationDto)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var actors = await connection.QueryAsync<Actor>("Atores_ObtenerTodos", 
                new {paginationDto.Page, paginationDto.RecordsPerPage },    
                commandType: CommandType.StoredProcedure);
                var amountActors = await connection.QuerySingleAsync<int>("Actores_Cantidad", 
                    commandType: CommandType.StoredProcedure);
                httpContentAccesor.Response.Headers.Append("CantidadTotalRegistros", amountActors.ToString());
                return actors.ToList();
            }
        }

        public async Task<Actor?> GetById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var actor = await connection.QueryFirstOrDefaultAsync<Actor>("Actores_ObeterPorId", new { id }, commandType: CommandType.StoredProcedure);
                return actor;
            }
        }
        public async Task<List<Actor>> GetByName (string nombre)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var actors = await connection.QueryAsync<Actor>("Actores_ObtenerPorNombre", 
                    new { nombre }, commandType: CommandType.StoredProcedure);
                return actors.ToList();
            }
        }
        public async Task<int> CreateActor(Actor actor)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var id = await connection.QuerySingleAsync<int>("Actores_Crear", new { actor.Nombre, actor.Foto, actor.FechaNacmiento },
                    commandType: CommandType.StoredProcedure);
                actor.Id = id;
                return id;
            }
        }
        public async Task<bool> Exist(int id)
        {
            using var connection = new SqlConnection(connectionString);
            {
                var exist = await connection.QuerySingleAsync<bool>("Actores_ExistePorID", new { id }, commandType: CommandType.StoredProcedure);
                return exist;
            }
        }
        public async Task UpdateActor(Actor actor)
        {
            using var connection = new SqlConnection(connectionString);
            {
                await connection.ExecuteAsync("Actores_Actulizar", actor, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            {
                await connection.ExecuteAsync("Actores_Borrar", new { id }, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<List<int>> ActorsExist(List<int> ids)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            foreach (var id in ids)
            {
                dt.Rows.Add(id);
            }
            using (var connection = new SqlConnection(connectionString))
            {
                var idsExist = await connection.QueryAsync<int>("Actores_ObtenerVariosPorId",
                    new { actoresIds = dt },
                    commandType: CommandType.StoredProcedure);
                return idsExist.ToList();
            }
        }
    }
}
