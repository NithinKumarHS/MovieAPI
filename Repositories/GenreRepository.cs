using MovieAPI.Models.DB;
using MovieAPI.Repositories.Interfaces;

namespace MovieAPI.Repositories
{
    public class GenreRepository :BaseRepository<Genre>, IGenreRepository
    {
        public GenreRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IList<Genre>> GetAllAsync()
        {
            const string sql = """
                SELECT [Id]
                	,[Name]
                FROM Foundation.Genres;
                """;

            var genres = await GetAllAsync(sql);

            return genres.ToList();
        }

        public async Task<Genre?> GetByIdAsync(int id)
        {
            const string sql = """
                SELECT [Id]
                	,[Name]
                FROM Foundation.Genres
                WHERE [Id] = @Id;
                """;
            return await GetByIdAsync(sql, new { Id = id });

        }

        public async Task<Genre> CreateAsync(Genre genre)
        {

            const string sql = """
                INSERT INTO Foundation.Genres([Name])
                VALUES (@Name);

                SELECT CAST(SCOPE_IDENTITY() AS INT);
                """;

            var createdGenre = await CreateAsync(sql, new
            {
                Name = genre.Name
            });

            genre.Id = createdGenre;

            return genre;
        }

        public async Task<Genre?> UpdateAsync(Genre genre)
        {
            const string sql = """
                UPDATE Foundation.Genres
                SET [Name] = @Name
                WHERE [Id] = @Id;
                """;
            var rowsAffected = await UpdatedAsync(sql, new
            {
                Id = genre.Id,
                Name = genre.Name
            });

            if(rowsAffected == 0)
            {
                return null;
            }

            return genre;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = """
                DELETE
                FROM Foundation.Genres
                WHERE [Id] = @Id
                """;

            var rowsAffected = await DeleteAsync(sql, new
            {
                Id = id
            });

            return rowsAffected > 0;
        }
    }
}
