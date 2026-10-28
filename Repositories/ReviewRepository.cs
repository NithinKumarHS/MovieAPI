using MovieAPI.Models.DB;
using MovieAPI.Repositories.Interfaces;

namespace MovieAPI.Repositories
{
    public class ReviewRepository :BaseRepository<Review>, IReviewRepository
    {
        public ReviewRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<IList<Review>> GetAllAsync()
        {
            const string sql = """
                SELECT [Id]
                	,[Message]
                	,[MovieId]
                FROM Foundation.Reviews;
                """;

            var reviews = await GetAllAsync(sql);

            return reviews.ToList();
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            const string sql = """
                SELECT [Id]
                	,[Message]
                	,[MovieId]
                FROM Foundation.Reviews
                WHERE [Id] = @Id;
                """;

            var review = await GetByIdAsync(sql, new
            {
                Id = id,
            });

            return review;
        }

        public async Task<Review> CreateAsync(Review review)
        {
            const string sql = """
                INSERT INTO Foundation.Reviews (
                	[Message]
                	,[MovieId]
                	)
                VALUES (
                	@Message
                	,@MovieId
                	)
                
                SELECT CAST(SCOPE_IDENTITY() AS INT);
                """;

            var createdReview = await CreateAsync(sql, new
            {
                Message = review.Message,
                MovieId = review.MovieId,
            });

            review.Id = createdReview;

            return review;
        }

        public async Task<Review?> UpdateAsync(Review review)
        {
            const string sql = """
                UPDATE Foundation.Reviews
                SET [Message] = @Message,
                    [MovieId] = @MovieId
                WHERE [Id] = @Id;
                """;

            var rowsAffected = await UpdatedAsync(sql, new
            {
                review.Id,
                review.Message,
                review.MovieId
            });

            if(rowsAffected == 0)
            {
                return null;
            }

            return review;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = """
                DELETE
                FROM Foundation.Reviews
                WHERE [Id] = @Id;
                """;

            var rowsAffected = await DeleteAsync(sql, new
            {
                Id = id
            });

            return rowsAffected > 0;
        }
    }
}
