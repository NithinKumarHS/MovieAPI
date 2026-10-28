using MovieAPI.Models.DB;

namespace MovieAPI.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        Task<IList<Movie>> GetAllAsync();

        Task<Movie?> GetByIdAsync(int id);

        Task<Movie> CreateAsync(Movie movie, List<int> actorIds, List<int> genreIds);

        Task<Movie> UpdateAsync(Movie movie, List<int> actorIds, List<int> genreIds);

        Task<bool> DeleteAsync(int id);

        Task<List<int>> GetActorIdsAsync(int movieId);
        Task<List<int>> GetGenreIdsAsync(int movieId);

        Task<Movie?> GetByNameYearAsync(string name, int yearOfRelease);

        Task<bool> UpdateCoverImageAsync(int movieId, string coverImage);

    }
}
