using MovieAPI.Models.DB;
using MovieAPI.Repositories.Interfaces;

namespace MovieAPI.Repositories
{
    public class AuthRepository :BaseRepository<User>, IAuthRepository
    {
        public AuthRepository(IConfiguration configuration)
            :base(configuration)
        {
        }
        public async Task<IList<User>> GetAllAsync()
        {

            const string sql = """
                SELECT [Id]
                	,[Name]
                	,[Email]
                	,[Password]
                FROM Foundation.Users;
                """;

            var users = await GetAllAsync(sql);

            return users.ToList();
        }

        public async Task<User?> GetUserAsync(string email)
        {

            const string sql = """
                SELECT [Id]
                	,[Name]
                	,[Email]
                	,[Password]
                FROM Foundation.Users
                WHERE [Email] = @Email;
                """;

            var existUser = await GetByIdAsync(sql, new
            {
                Email = email
            });

            return existUser;
        }

        public async Task<User> CreateAsync(User user)
        {

            const string sql = """
                INSERT INTO Foundation.Users (
                	[Name]
                	,Email
                	,[Password]
                	)
                VALUES (
                	@Name
                	,@Email
                	,@Password
                	)

                SELECT CAST(SCOPE_IDENTITY() AS INT);
                """;

            var createdUser = await CreateAsync(sql, new
            {
                user.Name,
                user.Email,
                user.Password
            });

            user.Id = createdUser;

            return user;
        }
    }
}