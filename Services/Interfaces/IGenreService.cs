using MovieAPI.Models.Request;
using MovieAPI.Models.Response;

namespace MovieAPI.Services.Interfaces
{
    public interface IGenreService
    {
        Task<IList<GenreResponse>> GetAllAsync();
        Task<GenreResponse?> GetByIdAsync(int id);
        Task<GenreResponse> CreateAsync(GenreRequest genreRequest);
        Task<GenreResponse?> UpdateAsync(int id, GenreRequest genreRequest);
        Task DeleteAsync(int id);
    }
}
