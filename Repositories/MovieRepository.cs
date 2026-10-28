using Dapper;
using MovieAPI.Models.DB;
using MovieAPI.Repositories.Interfaces;
using System.Data;

namespace MovieAPI.Repositories
{
    public class MovieRepository :BaseRepository<Movie>, IMovieRepository
    {
        public MovieRepository(IConfiguration configuration)
            :base(configuration)
        {
        }
        public async Task<IList<Movie>> GetAllAsync()
        {
            const string sql = """
                SELECT [Id]
                	,[Name]
                	,[YearOfRelease]
                	,[Plot]
                	,[CoverImage]
                	,[ProducerId]
                FROM Foundation.Movies;
                """;

            var movies = await GetAllAsync(sql);

            return movies.ToList();
        }
        public async Task<Movie?> GetByIdAsync(int id)
        {
            const string sql = """
                SELECT [Id]
                	,[Name]
                	,[YearOfRelease]
                	,[Plot]
                	,[CoverImage]
                	,[ProducerId]
                FROM Foundation.Movies
                WHERE [Id] = @Id;
                """;

            var movie = await GetByIdAsync(sql, new
            {
                Id = id
            });

            return movie;
        }

        public async Task<Movie> CreateAsync(Movie movie, List<int> actorIds, List<int> genreIds)
        {

            var procedureName = "Foundation.usp_AddMovie";

            using var connection = CreateConnection();

            var parameters = new
            {
                movie.Name,
                movie.YearOfRelease,
                movie.Plot,
                movie.ProducerId,
                movie.CoverImage,

                ActorIds = string.Join(",", actorIds),
                GenreIds = string.Join(",", genreIds)
            };

            var createdMovieId = await connection.QuerySingleAsync<int>
                (
                    procedureName, 
                    parameters, 
                    commandType: CommandType.StoredProcedure
                );

            movie.Id = createdMovieId;

            return movie;
        }

        public async Task<Movie> UpdateAsync(Movie movie, List<int> actorIds, List<int> genreIds)
        {
            var procedureName = "Foundation.usp_UpdateMovie";

            using var connection = CreateConnection();

            var parameter = new
            {
                movie.Id,
                movie.Name,
                movie.YearOfRelease,
                movie.Plot,
                movie.ProducerId,
                movie.CoverImage,

                ActorIds = string.Join(",", actorIds),
                GenreIds = string.Join(",", genreIds)

            };

            await connection.QuerySingleAsync<int>
                (
                    procedureName,
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

            return movie;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var movie = await GetByIdAsync(id);

            if (movie == null)
            {
                return false;
            }

            const string sql = "Foundation.usp_DeleteMovie";

            using var connection = CreateConnection();

            await connection.ExecuteAsync(sql, new
            {
                MovieId = id
            }, commandType: CommandType.StoredProcedure);

            return true;
        }

        public async Task<List<int>> GetActorIdsAsync(int movieId)
        {
            const string sql = """
                SELECT [ActorId]
                FROM Foundation.Actors_Movies
                WHERE [MovieId] = @MovieId
                """;

            using var connection = CreateConnection();

            var actorIds = await connection.QueryAsync<int>(sql, new
            {
                MovieId = movieId
            });

            return actorIds.ToList();
        }

        public async Task<List<int>> GetGenreIdsAsync(int movieId)
        {
            const string sql = """
                SELECT [GenreId]
                FROM Foundation.Genre_Movies
                WHERE [MovieId] = @MovieId
                """;

            using var connection = CreateConnection();

            var genreIds = await connection.QueryAsync<int>(sql, new
            {
                MovieId = movieId
            });

            return genreIds.ToList();
        }

        public async Task<Movie?> GetByNameYearAsync(string name, int yearOfRelease)
        {
            const string sql = """
                SELECT [Id]
                	,[Name]
                	,[YearOfRelease]
                	,[Plot]
                	,[CoverImage]
                	,[ProducerId]
                FROM Foundation.Movies
                WHERE [Name] = @Name
                AND [YearOfRelease] = @YearOfRelease;
                """;

            var existMovie = await QuerySingleOrDefault(sql, new
            {
                Name = name,
                YearOfRelease = yearOfRelease
            });

            return existMovie;
        }

        public async Task<bool> UpdateCoverImageAsync(int movieId, string coverImage)
        {
            const string sql = """
                UPDATE Foundation.Movies 
                SET [CoverImage] = @CoverImage
                WHERE [Id] = @Id
                """;

            var rowsAffected = await UpdatedAsync(sql, new
            {
                CoverImage = coverImage,
                Id = movieId
            });

            return rowsAffected > 0;
        }
    }
}
