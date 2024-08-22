using MinimalAPIPeliculas.Entities;

namespace MinimalAPIPeliculas.Repository
{
    public interface ICommentaryRepository
    {
        Task<int> CreateComentary(Commentary comentary);
        Task DeleteComentary(int id);
        Task<bool> Exist(int id);
        Task<List<Commentary>> GetAllComentaries(int movieId);
        Task<Commentary?> GetComentaryById(int id);
        Task UpdateComentary(Commentary comentary);
    }
}