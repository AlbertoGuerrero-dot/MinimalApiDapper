using AutoMapper;
using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Entities;

namespace MinimalAPIPeliculas.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CreateGenreDto, Genre>();
            CreateMap<Genre, GenreDto>();

            CreateMap<CreateActorDto, Actor>()
                .ForMember(x => x.Foto, options => options.Ignore());
            CreateMap<Actor, ActorDto>().ReverseMap();

            CreateMap<CreateMovieDto, Movie>()
                .ForMember(x => x.Poster, options => options.Ignore());

            CreateMap<Movie, MovieDto>()
                .ForMember(x => x.Genres, entity =>
                entity.MapFrom(p => p.Genres.Select(gp =>
                new GenreDto { Id = gp.GeneroId, Nombre = gp.Genre.Nombre })))
                .ForMember(x => x.Actors, entity =>
                entity.MapFrom(p =>
                p.Actors.Select(ma => new MovieActorDto { Id = ma.ActorId, 
                    Nombre = ma.Actor.Nombre, Personaje = ma.Personaje })));
            
            CreateMap<CreateCommentaryDto, Commentary>();
            CreateMap<Commentary, ComentaryDto>();

            CreateMap<AssingMovieActorDto, MovieActor>();
        }
    }
}
