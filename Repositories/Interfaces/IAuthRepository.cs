using MovieAPI.Models.DB;

namespace MovieAPI.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<IList<User>> GetAllAsync();
        Task<User?> GetUserAsync(string email);
        Task<User> CreateAsync(User user);
    }
}
