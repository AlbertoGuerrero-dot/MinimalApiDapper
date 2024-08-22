using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.Routing;
using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Entities;
using MinimalAPIPeliculas.Filters;
using MinimalAPIPeliculas.Repository;
using MinimalAPIPeliculas.Services;

namespace MinimalAPIPeliculas.Endpoints
{
    public static class CommentariesEndpoints
    {
        public static RouteGroupBuilder MapCommentaries(this RouteGroupBuilder group) 
        {
            group.MapGet("/", GetAll).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60))
            .Tag("get-commentaries")
            .SetVaryByRouteValue(new string[] { "movieId" }));
            group.MapGet("/{id:int}", GetById);
            group.MapPost("/", CreateCommentary).AddEndpointFilter<ValidationFilter<CreateCommentaryDto>>()
                .RequireAuthorization();    
            group.MapPut("/{id:int}", Update).AddEndpointFilter<ValidationFilter<CreateCommentaryDto>>()
                .RequireAuthorization();
            group.MapDelete("/{id:int}", Delete)
                .RequireAuthorization();
            return group;
        }

        static async Task<Results<Ok<List<ComentaryDto>>, NotFound>> GetAll(int movieId, 
            ICommentaryRepository commentaryRepository, 
            IMovieRepository movieRepository,
            IMapper mapper)
        {
            if (!await movieRepository.Exist(movieId))
            {
                return TypedResults.NotFound();
            }
            var commentaries = await commentaryRepository.GetAllComentaries(movieId);
            var commentariesDto = mapper.Map<List<ComentaryDto>>(commentaries);
            return TypedResults.Ok(commentariesDto);
        }

        static async Task<Results<Ok<ComentaryDto>, NotFound>> GetById(int movieId, int id,
            ICommentaryRepository commentaryRepository, 
            IMapper mapper)
        {
            var commentary = await commentaryRepository.GetComentaryById(id);
            if (commentary is null)
            {
                return TypedResults.NotFound();
            }
            var commentaryDto = mapper.Map<ComentaryDto>(commentary);
            return TypedResults.Ok(commentaryDto);
        }
        static async Task<Results<Created<ComentaryDto>, NotFound, BadRequest<string>>> CreateCommentary(int movieId, CreateCommentaryDto createCommentaryDto,
                    ICommentaryRepository commentaryRepository,
                    IMovieRepository movieRepository,
                    IOutputCacheStore outputChacheStore, IMapper mapper,
                    IUsersServices usersServices)
        {
            if (!await movieRepository.Exist(movieId))
            {
                return TypedResults.NotFound();
            }
            var commentary = mapper.Map<Commentary>(createCommentaryDto);
            commentary.PeliculaId = movieId;
            var user = await usersServices.GetUser();
            if (user is null)
            {
                return TypedResults.BadRequest("Usuario no encontrado");
            }
            commentary.UserId = user.Id;
            var id = await commentaryRepository.CreateComentary(commentary);
            await outputChacheStore.EvictByTagAsync("get-commentaries", default);
            var comentarioDto = mapper.Map<ComentaryDto>(commentary);
            return TypedResults.Created($"/commentaries/{id}", comentarioDto);

        }
        static async Task<Results<NoContent, NotFound, ForbidHttpResult>> Update(int movieId, int id, 
            CreateCommentaryDto createCommentaryDto,
            ICommentaryRepository commentaryRepository,
            IMovieRepository movieRepository,
            IOutputCacheStore outputChacheStore, IUsersServices usersServices)
        {
            if (!await movieRepository.Exist(movieId))
            {
                return TypedResults.NotFound();
            }
            var commentaryDB = await commentaryRepository.GetComentaryById(id);
            if (commentaryDB is null)
            {
                return TypedResults.NotFound();
            }
            var user = await usersServices.GetUser();
            if (user is null)
            {
                return TypedResults.NotFound();
            }
            if (commentaryDB.UserId != user.Id)
            {
                return TypedResults.Forbid();
            }
            commentaryDB.Cuerpo = createCommentaryDto.Cuerpo;
            await commentaryRepository.UpdateComentary(commentaryDB);
            await outputChacheStore.EvictByTagAsync("get-commentaries", default);
            return TypedResults.NoContent();
        }
        static async Task<Results<NoContent, NotFound, ForbidHttpResult>> Delete(int movieId, int id,
            ICommentaryRepository commentaryRepository,
            IOutputCacheStore outputChacheStore, IUsersServices usersServices)
        {
            var commentaryDB = await commentaryRepository.GetComentaryById(id);
            if (commentaryDB is null)
            {
                return TypedResults.NotFound();
            }
            var user = await usersServices.GetUser();
            if (user is null)
            {
                return TypedResults.NotFound();
            }
            if (commentaryDB.UserId != user.Id)
            {
                return TypedResults.Forbid();
            } 
            await commentaryRepository.DeleteComentary(id);
            await outputChacheStore.EvictByTagAsync("get-commentaries", default);
            return TypedResults.NoContent();
        }
    }
}
