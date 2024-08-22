 using Dapper;
using Microsoft.Data.SqlClient;
using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Entities;
using System.Data;

namespace MinimalAPIPeliculas.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly string? connectionString;
        private readonly HttpContext httpContext;

        public MovieRepository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
            httpContext = httpContextAccessor.HttpContext!;
        }
        public async Task<List<Movie>> GetAll(paginationDto paginationDto)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var movies = await connection.QueryAsync<Movie>("Peliculas_ObtenerTodas", new { paginationDto.Page, paginationDto.RecordsPerPage }, commandType: CommandType.StoredProcedure);
                var amoutMovies = await connection.QuerySingleAsync<int>("Peliculas_Cantidad", commandType: CommandType.StoredProcedure);

                httpContext.Response.Headers.Append("cantidadPeliculas", amoutMovies.ToString());

                return movies.ToList();
            }
        }
        public async Task<Movie?> GetMovieById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var multi = await connection.QueryMultipleAsync("Peliculas_ObtenerPorId",
                    new { id }, commandType: CommandType.StoredProcedure))
                {
                    var movie = await multi.ReadFirstOrDefaultAsync<Movie>();
                    if (movie == null)
                    {
                        return null;
                    }

                    var commentaries = await multi.ReadAsync<Commentary>();
                    var genres = await multi.ReadAsync<Genre>();
                    var actors = await multi.ReadAsync<MovieActorDto>();

                    foreach (var genre in genres)
                    {
                        movie.Genres.Add(new MovieGenre
                        {
                            GeneroId = genre.Id,
                            Genre = genre
                        });
                    }

                    foreach (var actor in actors)
                    {
                        movie.Actors.Add(new MovieActor
                        {
                            ActorId = actor.Id,
                            Personaje = actor.Personaje,
                            Actor = new Actor { Nombre = actor.Nombre }
                        });
                    }

                    movie.Comentarios = commentaries.ToList();
                    return movie;
                }
            }
        }

        public async Task<int> CreateMovie(Movie movie)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var id = await connection.QuerySingleAsync<int>("Peliculas_Crear",
                    new { movie.Titulo, movie.Poster, movie.FechaLanzamiento, movie.Encines }, commandType: CommandType.StoredProcedure);
                movie.Id = id;
                return id;
            }
        }

        public async Task<bool> Exist(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var exist = await connection.QuerySingleAsync<bool>("Peliculas_Existe", new { id }, commandType: CommandType.StoredProcedure);
                return exist;
            }
        }
        public async Task UpdateMovie(Movie movie)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync("Peliculas_Actulizar", movie, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync("Peliculas_Borrar", new { id }, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task InsertGenre(int id, List<int> generosIds)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            foreach (var generoId in generosIds)
            {
                dt.Rows.Add(generoId);
            }
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync("Peliculas_AsignarGeneros",
                new { PeliculaId = id, GenerosIds = dt }, // Nombre de los paramtros
                commandType: CommandType.StoredProcedure);
            }
        }
        public async Task AssingActors(int id, List<MovieActor> actors)
        {
            for (int i = 1; i <= actors.Count; i++)
            {
                actors[i-1].Orden = i;
            }

            var dt = new DataTable();
            dt.Columns.Add("ActorId", typeof(int));
            dt.Columns.Add("Personaje", typeof(string));
            dt.Columns.Add("Orden", typeof(int));



            foreach (var movieActor in actors)      
            {
                dt.Rows.Add(movieActor.ActorId, movieActor.Personaje, movieActor.Orden);
            }

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync("Peliculas_AsiganarActores",
                    new { PeliculaId = id, 
                        actores = dt },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<List<Movie>> Filter(MovieFilterDTO movieFilterDTO)
        {
            using(var connection = new SqlConnection(connectionString))
            {
                var movie = await connection.QueryAsync<Movie>("Peliculas_Filtrar",
                    new
                    {
                       movieFilterDTO.Pagina,
                       movieFilterDTO.RecordsPorPagina,
                       movieFilterDTO.Titulo,
                       movieFilterDTO.GeneroId,
                       movieFilterDTO.EnCines,
                       movieFilterDTO.ProximoEstrenos,
                       movieFilterDTO.CampoOrdenar,
                       movieFilterDTO.OrdenAscendente
                    }, commandType: CommandType.StoredProcedure);
                var movieCount = await connection.QuerySingleAsync<int>("Peliculas_Cantidad",
                    new
                    {
                        movieFilterDTO.Titulo,
                        movieFilterDTO.GeneroId,
                        movieFilterDTO.EnCines,
                        movieFilterDTO.ProximoEstrenos
                    }, commandType: CommandType.StoredProcedure);
                httpContext.Response.Headers.Append("cantidadPeliculas", movieCount.ToString());
                return movie.ToList();
            }
        }
    }
}
