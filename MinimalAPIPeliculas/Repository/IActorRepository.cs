using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Entities;

namespace MinimalAPIPeliculas.Repository
{
    public interface IActorRepository
    {
        Task<List<int>> ActorsExist(List<int> ids);
        Task<int> CreateActor(Actor actor);
        Task Delete(int id);
        Task<bool> Exist(int id);
        Task<List<Actor>> GetAll(paginationDto paginationDto);
        Task<Actor?> GetById(int id);
        Task<List<Actor>> GetByName(string name);
        Task UpdateActor(Actor actor);
    }
}