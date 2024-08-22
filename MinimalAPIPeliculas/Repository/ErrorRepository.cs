using Dapper;
using Microsoft.Data.SqlClient;
using MinimalAPIPeliculas.Entities;
using System.Data;

namespace MinimalAPIPeliculas.Repository
{
    public class ErrorRepository : IErrorRepository
    {
        private string connectionString;

        public ErrorRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;

        }
        public async Task Create(Error error)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync("Errores_Crear", new
                {
                    error.MensajeDeError,
                    error.StackTrace,
                    error.Fecha
                }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
