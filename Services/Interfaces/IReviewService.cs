using MovieAPI.Models.Request;
using MovieAPI.Models.Response;

namespace MovieAPI.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IList<ReviewResponse>> GetAllAsync();
        Task<ReviewResponse?> GetByIdAsync(int id);
        Task<ReviewResponse> CreateAsync(ReviewRequest reviewRequest);
        Task<ReviewResponse?> UpdateAsync(int id, ReviewRequest reviewRequest);
        Task DeleteAsync(int id);
    }
}
