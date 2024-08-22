using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.OpenApi.Models;
using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Entities;
using MinimalAPIPeliculas.Filters;
using MinimalAPIPeliculas.Repository;
using MinimalAPIPeliculas.Services;
using MinimalAPIPeliculas.Utilities;

namespace MinimalAPIPeliculas.Endpoints
{
    public static class ActorsEndpoints
    {
        private static readonly string container = "actors";
        public static RouteGroupBuilder MapActors(this WebApplication app)
        {
            var actorsEnpoints = app.MapGroup("/actor");



            actorsEnpoints.MapGet("/", getAll).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("actores-get"))
                .AddParametersPagination(); 
            actorsEnpoints.MapGet("/{id:int}", getById);
            actorsEnpoints.MapGet("/getbyname/{name}", getByName);
            actorsEnpoints.MapPost("/", createActor).DisableAntiforgery().AddEndpointFilter<ValidationFilter<CreateActorDto>>().RequireAuthorization("Admin")
                .WithOpenApi();
            actorsEnpoints.MapPut("/{id:int}", updateActor).DisableAntiforgery().AddEndpointFilter<ValidationFilter<CreateActorDto>>().RequireAuthorization("Admin")
                .WithOpenApi();
            actorsEnpoints.MapDelete("/{id:int}", deleteActor).RequireAuthorization("Admin");

            static async Task<Ok<List<ActorDto>>> getAll(IActorRepository actorRepository, IMapper mapper, 
                paginationDto pagination)
            {
                //var pagination = new paginationDto { Page = page, recordsPerPage = recordPerPage };
                var actors = await actorRepository.GetAll(pagination);
                var actorsDto = mapper.Map<List<ActorDto>>(actors);
                return TypedResults.Ok(actorsDto);
            }
            static async Task<Results<Ok<ActorDto>, NotFound>> getById(int id, IActorRepository actorRepository, IMapper mapper)
            {
                var actor = await actorRepository.GetById(id);
                if (actor is null)
                {
                    return TypedResults.NotFound();
                }
                var actorDto = mapper.Map<ActorDto>(actor);
                return TypedResults.Ok(actorDto);
            }
            static async Task<Ok<List<ActorDto>>> getByName(string name, IActorRepository actorRepository, IMapper mapper)
            {
                var actors = await actorRepository.GetByName(name);
                var actorsDto = mapper.Map<List<ActorDto>>(actors);
                return TypedResults.Ok(actorsDto);
            }
            static async Task<Results<Created<ActorDto>, ValidationProblem>> createActor([FromForm] CreateActorDto createActorDto,
                IActorRepository actorRepository, IOutputCacheStore outputCacheStore, IMapper mapper, IFileStorer fileStorer)
            {
                var actor = mapper.Map<Actor>(createActorDto);
                if (createActorDto.Foto is not null) 
                {
                    var url = await fileStorer.store(container, createActorDto.Foto);
                    actor.Foto = url;
                }
                var id = await actorRepository.CreateActor(actor);
                await outputCacheStore.EvictByTagAsync("actores-get", default);
                var actorDto = mapper.Map<ActorDto>(actor);
                return TypedResults.Created($"/actors/{actor.Id}", actorDto);
            }
            static async Task<Results<NoContent, NotFound>> updateActor(int id, [FromForm]CreateActorDto createActorDto, IActorRepository actorRepository, IMapper mapper,
                IOutputCacheStore outputCacheStore, IFileStorer fileStorer)
            {
                var actorDB = await actorRepository.GetById(id);

                if (actorDB is null) 
                {
                    return TypedResults.NotFound();
                }
                var updateActor = mapper.Map<Actor>(createActorDto);
                updateActor.Id = id; 
                updateActor.Foto = actorDB.Foto;
                if(createActorDto.Foto is not null)
                {
                    var url = await fileStorer.Edit(updateActor.Foto, 
                        container, createActorDto.Foto);
                    updateActor.Foto = url;
                }
                await actorRepository.UpdateActor(updateActor);
                await outputCacheStore.EvictByTagAsync("actores-get", default);
                return TypedResults.NoContent();    
            }
            static async Task<Results<NoContent, NotFound>> deleteActor(int id, IActorRepository actorRepository,
                IOutputCacheStore outputCacheStore, IFileStorer fileStorer)
            {
                var actorDB = await actorRepository.GetById(id); 
                if (actorDB is null) 
                { 
                    return TypedResults.NotFound(); 
                }
                await actorRepository.Delete(id);
                await fileStorer.delete(actorDB.Foto, container);
                await outputCacheStore.EvictByTagAsync("actores-get", default);
                return TypedResults.NoContent();
            }
            
            return actorsEnpoints;
        }
    }
}
