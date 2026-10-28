using MovieAPI.Models.DB;
using MovieAPI.Repositories.Interfaces;

namespace MovieAPI.Repositories
{
    public class ProducerRepository :BaseRepository<Producer>, IProducerRepository
    {
        public ProducerRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<IList<Producer>> GetAllAsync()
        {
            const string sql = """
                SELECT  [Id], 
                		[Name],
                		[DateOfBirth],
                		[Biography],
                		[GenderId]
                FROM Foundation.Producers;
                """;

            var producers =  await GetAllAsync(sql);

            return producers.ToList();
        }
        public async Task<Producer?> GetByIdAsync(int id)
        {
            const string sql = """
                SELECT  [Id], 
                		[Name],
                		[DateOfBirth],
                		[Biography],
                		[GenderId]
                FROM Foundation.Producers
                WHERE Id = @Id;
                """;

            var producer = await GetByIdAsync(sql, new { Id = id });

            return producer;    
        }

        public async Task<Producer> CreateAsync(Producer producer)
        {

            const string sql = """
                INSERT INTO Foundation.Producers (
                	[Name]
                	,[DateOfBirth]
                	,[Biography]
                	,[GenderId]
                	)
                VALUES (
                	@Name
                	,@DateOfBirth
                	,@Biography
                	,@GenderId
                	);
                SELECT CAST(SCOPE_IDENTITY() AS INT)
                """;

            var createdProducer = await CreateAsync(sql, new
            {
                producer.Name,
                producer.DateOfBirth,
                producer.Biography,
                producer.GenderId
            });

            producer.Id = createdProducer;

            return producer;
        }

        public async Task<Producer?> UpdateAsync(Producer producer)
        {
            const string sql = """
                UPDATE [Foundation].[Producers]
                SET
                    [Name] = @Name,
                    [DateOfBirth] = @DateOfBirth,
                    [Biography] = @Biography,
                    [GenderId] = @GenderId
                WHERE [Id] = @Id
                """;

            var existingProducer = await UpdatedAsync(sql, new
            {
                producer.Id,
                producer.Name,
                producer.DateOfBirth,
                producer.Biography,
                producer.GenderId
            });

            if(existingProducer == 0)
            {
                return null;
            }

            return producer;
        }

        public async Task<bool> DeleteAsync(int id)
        {

            const string sql = """
                DELETE FROM [Foundation].[Producers]
                WHERE [Id] = @Id;
                """;

            var rowsAffected = await DeleteAsync(sql, new { Id = id });

            return rowsAffected > 0;


        }

        public async Task<Producer?> GetByNameDateOfBirthAsync(string name, DateOnly dateOfBirth)
        {

            const string sql = """
                SELECT  [Id], 
                      		[Name],
                      		[DateOfBirth],
                      		[Biography],
                      		[GenderId]
                FROM Foundation.Producers
                WHERE [Name] = @Name AND [DateOfBirth] = @DateOfBirth;
                """;

            var existingProducer = await QuerySingleOrDefault(sql, new
            {
                Name = name,
                DateOfBirth = dateOfBirth
            });

            return existingProducer;
        }
    }
}
