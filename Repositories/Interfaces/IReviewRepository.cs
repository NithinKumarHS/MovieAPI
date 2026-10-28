using MovieAPI.Models.DB;

namespace MovieAPI.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<IList<Review>> GetAllAsync();
        Task<Review?> GetByIdAsync(int id);
        Task<Review> CreateAsync(Review review);
        Task<Review?> UpdateAsync(Review review);
        Task<bool> DeleteAsync(int id);
    }
}
