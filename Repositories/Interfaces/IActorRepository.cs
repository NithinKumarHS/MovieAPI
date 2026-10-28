using MovieAPI.Models.DB;

namespace MovieAPI.Repositories.Interfaces
{
    public interface IActorRepository
    {
        Task<IList<Actor>> GetAllAsync();

        Task<Actor?> GetByIdAsync(int id);

        Task<Actor> CreateAsync(Actor actor);

        Task<Actor?> UpdateAsync(Actor actor);

        Task<bool> DeleteAsync(int id);

        Task<Actor?> GetByNameDateOfBirthAsync(string name, DateOnly dateOfBirth);
    }
}