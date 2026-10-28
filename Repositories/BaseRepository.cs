using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MovieAPI.Repositories
{
    public class BaseRepository<T> where T : class
    {
        private readonly string _configuration;

        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration.GetConnectionString("MovieDB")!;
        }

        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(_configuration);
        }

        protected async Task<IEnumerable<T>> GetAllAsync(string sql, object? parameter = null)
        {
            using var connection = CreateConnection();

            return await connection.QueryAsync<T>(sql, parameter);
        }

        protected async Task<T?> GetByIdAsync(string sql, object parameter)
        {
            using var connection = CreateConnection();

            return await connection.QuerySingleOrDefaultAsync<T>(sql, parameter);
        }

        protected async Task<int> CreateAsync(string sql, object parameter)
        {
            using var connection = CreateConnection();

            return await connection.QuerySingleAsync<int>(sql, parameter); 
        }

        protected async Task<int> UpdatedAsync(string sql, object parameter)
        {
            using var connection = CreateConnection();

            var rowsAffected = await connection.ExecuteAsync(sql, parameter);

             return rowsAffected;
        }

        protected async Task<int> DeleteAsync(string sql, object paramater)
        {
            using var connection = CreateConnection();

            var rowsAffected = await connection.ExecuteAsync(sql, paramater);

            return rowsAffected;
        }

        protected async Task<T?> QuerySingleOrDefault(string sql, object parameter)
        {
            using var connection = CreateConnection();

            return await connection.QuerySingleOrDefaultAsync<T>(sql, parameter);
        }
    }
}
