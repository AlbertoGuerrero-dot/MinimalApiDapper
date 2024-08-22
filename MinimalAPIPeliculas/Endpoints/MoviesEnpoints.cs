 using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Entities;
using MinimalAPIPeliculas.Filters;
using MinimalAPIPeliculas.Repository;
using MinimalAPIPeliculas.Services;
using MinimalAPIPeliculas.Utilities;

namespace MinimalAPIPeliculas.Endpoints
{
    public static class MoviesEnpoints
    {
        private static readonly string container = "movies";
        public static RouteGroupBuilder MapMovies(this WebApplication app)
        {
            var moviesEndpoints = app.MapGroup("/movie");

            moviesEndpoints.MapGet("/", getAll).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("movies-get")).AddParametersPagination();
            moviesEndpoints.MapGet("/{id:int}", getById).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("movies-get"));
            moviesEndpoints.MapPost("/", createMovie).DisableAntiforgery().AddEndpointFilter<ValidationFilter<CreateMovieDto>>().RequireAuthorization("Admin").WithOpenApi();
            moviesEndpoints.MapPut("/{id:int}", updateMovie).DisableAntiforgery().AddEndpointFilter<ValidationFilter<CreateMovieDto>>().RequireAuthorization("Admin").WithOpenApi();
            moviesEndpoints.MapDelete("/{id:int}", deleteMovie).RequireAuthorization("Admin");
            moviesEndpoints.MapPost("/{id:int}/assigngenres", AssignGenres).RequireAuthorization("Admin");
            moviesEndpoints.MapPost("/{id:int}/assignactors", AssignActor).RequireAuthorization("Admin");
            moviesEndpoints.MapGet("/filter", Filter).AddMoviesParametersPagination();

            static async Task<Ok<List<MovieDto>>> getAll(IMovieRepository movieRepository, IMapper mapper, paginationDto pagination)
            {
                var movies = await movieRepository.GetAll(pagination);
                var moviesDto = mapper.Map<List<MovieDto>>(movies);
                return TypedResults.Ok(moviesDto);
            }
            static async Task<Results<Ok<MovieDto>, NotFound>> getById(IMovieRepository movieRepository, IMapper mapper, int id)
            {
                var movie = await movieRepository.GetMovieById(id);
                if (movie is null)
                {
                    return TypedResults.NotFound();
                }
                var movieDto = mapper.Map<MovieDto>(movie);
                return TypedResults.Ok(movieDto);
            }
            static async Task<Created<MovieDto>> createMovie([FromForm] CreateMovieDto createMovieDto, IMovieRepository movieRepository, IMapper mapper, IFileStorer fileStorer, IOutputCacheStore outputCacheStore)
            {
                var movie = mapper.Map<Movie>(createMovieDto);
                if (createMovieDto.Poster is not null)
                {
                    var url = await fileStorer.store(container, createMovieDto.Poster);
                    movie.Poster = url;
                }
                var id = await movieRepository.CreateMovie(movie);
                var movieDto = mapper.Map<MovieDto>(movie);
                await outputCacheStore.EvictByTagAsync("movies-get", default);
                return TypedResults.Created($"/movie/{movie.Id}", movieDto);
            }
            static async Task<Results<NoContent, NotFound>> updateMovie(int id, [FromForm] CreateMovieDto createMovieDto, IMovieRepository movieRepository, IMapper mapper, IFileStorer fileStorer, IOutputCacheStore outputCacheStore)
            {
                var movie = await movieRepository.GetMovieById(id);

                if (movie is null)
                {
                    return TypedResults.NotFound();
                }
                var updateMovie = mapper.Map<Movie>(createMovieDto);
                updateMovie.Id = id;
                updateMovie.Poster = movie.Poster;

                if (createMovieDto.Poster is not null)
                {
                    var url = await fileStorer.Edit(updateMovie.Poster, container, createMovieDto.Poster);
                    updateMovie.Poster = url;
                }
                await movieRepository.UpdateMovie(updateMovie);
                await outputCacheStore.EvictByTagAsync("movies-get", default);
                return TypedResults.NoContent();
            }
            static async Task<NoContent> deleteMovie(int id, IMovieRepository movieRepository, IOutputCacheStore outputCacheStore)
            {
                var movie = await movieRepository.GetMovieById(id);
                if (movie is null)
                {
                    return TypedResults.NoContent();
                }
                await movieRepository.Delete(id);
                await outputCacheStore.EvictByTagAsync("movies-get", default);
                return TypedResults.NoContent();
            }
            static async Task<Results<NoContent, NotFound, BadRequest<string>>> AssignGenres(int id, List<int> genresIds, 
                IMovieRepository movieRepository, 
                IGenreRepository genreRepository)
            {
                if(!await movieRepository.Exist(id))
                {
                    return TypedResults.NotFound();
                }
                var genres = new List<int>();

                if (genresIds.Count != 0)
                {
                    genres = await genreRepository.GenreExists(genresIds);
                }
                if (genres.Count != genresIds.Count)
                {
                    var genresNotExists = genresIds.Except(genres).ToList();
                    return TypedResults.BadRequest($"Genres with Ids {string.Join(", ", genresNotExists)} not found");
                }
                await movieRepository.InsertGenre(id, genresIds);
                return TypedResults.NoContent();
            }
            static async Task<Results<NoContent, NotFound, BadRequest<string>>> AssignActor(int id, List<AssingMovieActorDto> actorsDto, 
                IMovieRepository movieRepository, 
                IActorRepository actorRepository,
                IMapper mapper)
            {
                if(!await movieRepository.Exist(id))
                {
                    return TypedResults.NotFound();
                }
                var actorsExist = new List<int>();
                var actorsIds = actorsDto.Select(a => a.ActorId).ToList();
                if (actorsDto.Count != 0)
                {
                    actorsExist = await actorRepository.ActorsExist(actorsIds);
                }   
                if(actorsExist.Count != actorsDto.Count)
                {
                    var actorsNotExists = actorsIds.Except(actorsExist);
                    return TypedResults.BadRequest($"Actors with Ids {string.Join(", ", actorsNotExists)} not found");
                } 
                var actorsToAssign = mapper.Map<List<MovieActor>>(actorsDto);
                await movieRepository.AssingActors(id, actorsToAssign); 
                return TypedResults.NoContent();
            }
            static async Task<Ok<List<MovieDto>>> Filter(MovieFilterDTO movieFilterDTO, IMovieRepository movieRepository, IMapper mapper)
            {
                var movies = await movieRepository.Filter(movieFilterDTO);
                var moviesDto = mapper.Map<List<MovieDto>>(movies);
                return TypedResults.Ok(moviesDto);
            }
            return moviesEndpoints;
        }
    }
}
