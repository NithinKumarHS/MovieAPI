using MovieAPI.Models.Request;
using MovieAPI.Models.Response;

namespace MovieAPI.Services.Interfaces
{
    public interface IMovieService
    {
        Task<IList<MovieResponse>> GetAllAsync(int? year);

        Task<MovieResponse> GetByIdAsync(int id);

        Task<MovieResponse> CreateAsync(MovieRequest request);

        Task<MovieResponse> UpdateAsync(int id, MovieRequest request);

        Task DeleteAsync(int id);
        Task<MovieResponse> UpdatePosterAsync(int movieId, IFormFile poster);
    }
}
