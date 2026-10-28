using MovieAPI.Models.DB;

namespace MovieAPI.Repositories.Interfaces
{
    public interface IGenreRepository
    {
        Task<IList<Genre>> GetAllAsync();
        Task<Genre?> GetByIdAsync(int id);
        Task<Genre> CreateAsync(Genre genre);
        Task<Genre?> UpdateAsync(Genre genre);
        Task<bool> DeleteAsync(int id);
    }
}
