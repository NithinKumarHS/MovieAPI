using MovieAPI.Models.DB;

namespace MovieAPI.Repositories.Interfaces
{
    public interface IProducerRepository
    {
        Task<IList<Producer>> GetAllAsync();
        Task<Producer?> GetByIdAsync(int id);

        Task<Producer> CreateAsync(Producer actor);

        Task<Producer?> UpdateAsync(Producer actor);

        Task<bool> DeleteAsync(int id);
        Task<Producer?> GetByNameDateOfBirthAsync(string name, DateOnly dateOfBirth);
    }
}
