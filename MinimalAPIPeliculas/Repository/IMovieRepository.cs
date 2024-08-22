using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Entities;

namespace MinimalAPIPeliculas.Repository
{
    public interface IMovieRepository
    {
        Task AssingActors(int id, List<MovieActor> actors);
        Task<int> CreateMovie(Movie movie);
        Task Delete(int id);
        Task<bool> Exist(int id);
        Task<List<Movie>> GetAll(paginationDto paginationDto);
        Task<Movie?> GetMovieById(int id);
        Task InsertGenre(int id, List<int> generosId);
        Task UpdateMovie(Movie movie);
        Task<List<Movie>> Filter(MovieFilterDTO movieFilterDTO);
    }
}