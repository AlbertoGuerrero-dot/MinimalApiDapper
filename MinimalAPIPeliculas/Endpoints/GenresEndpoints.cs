using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Entities;
using MinimalAPIPeliculas.Repository;
using FluentValidation;
using MinimalAPIPeliculas.Filters;

namespace MinimalAPIPeliculas.Endpoints
{
    public static class GenresEndpoints
    {
        public static RouteGroupBuilder MapGenres(this WebApplication app)
        {
            var genereEnpoints = app.MapGroup("/genre");

            genereEnpoints.MapGet("/", GetGenres).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(15)).Tag("get-genre"));
            genereEnpoints.MapGet("/{id:int}", GetGenreById);
            genereEnpoints.MapPost("/", CreateGenre).AddEndpointFilter<ValidationFilter<CreateGenreDto>>().RequireAuthorization("Admin");
            genereEnpoints.MapPut("/{id:int}", UpdateGenre).AddEndpointFilter<ValidationFilter<CreateGenreDto>>().RequireAuthorization("Admin")
                .WithOpenApi(options =>
                {
                    options.Summary = "Update a genre";
                    options.Description = "Update a genre";
                    options.Parameters[0].Description = "Id of the genre to update";
                    options.RequestBody.Description = "Genre to update";
                    return options;
                });
            genereEnpoints.MapDelete("/{id:int}", DeleteGenre).RequireAuthorization("Admin");

            static async Task<Ok<List<GenreDto>>> GetGenres(IGenreRepository genreRepository, IMapper mapper, ILoggerFactory loggerFactory)
            {
                var type = typeof(GenresEndpoints);
                var logger = loggerFactory.CreateLogger(type.FullName!);
                logger.LogInformation("GetGenres");
                var genres = await genreRepository.GetAll();
                var genreDto = mapper.Map<List<GenreDto>>(genres);
                return TypedResults.Ok(genreDto);

            }
            static async Task<Results<Ok<GenreDto>, NotFound>> GetGenreById(int id, IGenreRepository genreRepository, IMapper mapper)
            {
                var genre = await genreRepository.GetById(id);
                if (genre == null)
                {
                    return TypedResults.NotFound();
                }
                var genreDto = mapper.Map<GenreDto>(genre);
                return TypedResults.Ok(genreDto);
            }
            static async Task<Results<Created<GenreDto>, ValidationProblem>> CreateGenre(CreateGenreDto createGenreDto, IGenreRepository genreRepository,
                        IOutputCacheStore outputChacheStore, IMapper mapper)
            { 
                var genre = mapper.Map<Genre>(createGenreDto);
                var id = await genreRepository.CreateGenre(genre);
                await outputChacheStore.EvictByTagAsync("get-genre", default);
                var genreDto = mapper.Map<GenreDto>(genre);
                return TypedResults.Created($"/genre{genre.Id}", genreDto);
            }
            static async Task<Results<NoContent, NotFound, ValidationProblem>> UpdateGenre(int id, CreateGenreDto createGenreDto, IGenreRepository genreRepository,
                    IOutputCacheStore outputChacheStore, IMapper mapper)
            {

                var exist = await genreRepository.Exist(id);
                if (!exist)
                {
                    return TypedResults.NotFound();
                }
                var genre = mapper.Map<Genre>(createGenreDto);
                genre.Id = id;
                await genreRepository.UpdateGenre(genre);
                await outputChacheStore.EvictByTagAsync("get-genre", default);
                return TypedResults.NoContent();
            }
            static async Task<Results<NoContent, NotFound>> DeleteGenre(int id, IGenreRepository genreRepository,
                   IOutputCacheStore outputCacheStore)
            {
                var exist = await genreRepository.Exist(id);
                if (!exist)
                {
                    return TypedResults.NotFound();
                }
                await genreRepository.Delete(id);
                await outputCacheStore.EvictByTagAsync("get-genre", default);
                return TypedResults.NoContent();
            } 
            return genereEnpoints;
        }
    }
}
