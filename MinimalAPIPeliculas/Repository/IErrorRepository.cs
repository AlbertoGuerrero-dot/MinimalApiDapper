using MinimalAPIPeliculas.Entities;

namespace MinimalAPIPeliculas.Repository
{
    public interface IErrorRepository
    {
        Task Create(Error error);
    }
}