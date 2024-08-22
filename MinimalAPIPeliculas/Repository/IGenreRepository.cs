using MinimalAPIPeliculas.Entities;

namespace MinimalAPIPeliculas.Repository
{
    public interface IGenreRepository
    {
        Task<int> CreateGenre(Genre genre);
        Task<List<Genre>> GetAll();
        Task<Genre?> GetById(int id);
        Task<bool> Exist(int id);
        Task UpdateGenre(Genre genre);
        Task Delete(int id);
        Task<List<int>> GenreExists(List<int> ids);
        Task<bool> Exist(int id, string nombre);
    }
}