using MovieAPI.Models.DB;
using MovieAPI.Repositories.Interfaces;

namespace MovieAPI.Repositories
{
    public class ActorRepository :BaseRepository<Actor>, IActorRepository
    {

        public ActorRepository(IConfiguration configuration)
            : base(configuration)
        {
        }


        public async Task<IList<Actor>> GetAllAsync()
        {

            const string sql = """
                SELECT  [Id], 
                		[Name],
                		[DateOfBirth],
                		[Biography],
                		[GenderId]
                FROM Foundation.Actors;
                """;

            var actors = await GetAllAsync(sql);

            return actors.ToList();
        }
        public async Task<Actor?> GetByIdAsync(int id)
        {

            const string sql = """
                SELECT  [Id], 
                		[Name],
                		[DateOfBirth],
                		[Biography],
                		[GenderId]
                FROM Foundation.Actors
                WHERE Id = @Id;
                """;

            return await GetByIdAsync(sql, new {Id =  id});

        }

        public async Task<Actor> CreateAsync(Actor actor)
        {

            const string sql = """
                INSERT INTO Foundation.Actors (
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

            

            var createdActor = await CreateAsync(sql, new
            {
                actor.Name,
                actor.DateOfBirth,
                actor.Biography,
                actor.GenderId
            });

            actor.Id = createdActor;

            return actor;
        }

        public async Task<Actor?> UpdateAsync(Actor actor)
        {

            const string sql = """
                UPDATE [Foundation].[Actors]
                SET
                    [Name] = @Name,
                    [DateOfBirth] = @DateOfBirth,
                    [Biography] = @Biography,
                    [GenderId] = @GenderId
                WHERE [Id] = @Id
                """;

            var existingActor = await UpdatedAsync(
                sql,
                new
                {
                    actor.Id,
                    actor.Name,
                    actor.DateOfBirth,
                    actor.Biography,
                    actor.GenderId
                }
            );

            if(existingActor == 0)
            {
                return null;
            }

            return actor ;
        }

        public async Task<bool> DeleteAsync(int id)
        {

            const string sql = """
                DELETE FROM [Foundation].[Actors]
                WHERE [Id] = @Id;
                """;

            var rowsAffected = await DeleteAsync(
                sql,
                new { Id = id }
            );

            return rowsAffected > 0;

        }

        public async Task<Actor?> GetByNameDateOfBirthAsync(string name,  DateOnly dateOfBirth)
        {

            const string sql = """
                SELECT  [Id], 
                      		[Name],
                      		[DateOfBirth],
                      		[Biography],
                      		[GenderId]
                FROM Foundation.Actors
                WHERE [Name] = @Name AND [DateOfBirth] = @DateOfBirth ;
                """;


            var existActor =  await QuerySingleOrDefault(sql, new
            {
                Name = name,
                DateOfBirth = dateOfBirth
            });

            return existActor;
        }
    }
}
