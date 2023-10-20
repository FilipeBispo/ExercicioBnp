using ExercicioBnp.Infrastructure.Interfaces;
using ExercicioBnp.Model;
using Dapper;
using System.Data.SqlClient;
using ExercicioBnp.Exceptions;
using ExercicioBnp.Controllers;

namespace ExercicioBnp.Infrastructure
{
    public class IsinRepository : IIsinRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<IsinRepository> _logger;

        public IsinRepository(IConfiguration configuration, ILogger<IsinRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public async Task<Isin?> GetByIsinIdentifierAsync(string isinIdentifier)
        {
            try {
                using var connection = new SqlConnection(_connectionString);
                const string query = "SELECT Identifier, Price FROM Isins WHERE Identifier = @Identifier";

                var isin = await connection.QuerySingleOrDefaultAsync<Isin>(query, new { Identifier = isinIdentifier });

                return isin;
            }
            catch (SqlException ex) {
                _logger.LogError(ex, "An error occurred while processing your request.");
                throw new DatabaseConnectionException();
            }
        }

        public async Task InsertAsync(Isin isin)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                const string query = "INSERT INTO Isins (Identifier, Price) VALUES (@Identifier, @Price)";
                await connection.ExecuteAsync(query, isin);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "An error occurred while processing your request.");
                throw new IsinInsertionException(isin.Identifier);
            }
        }

        public async Task BatchInsertAsync(IEnumerable<Isin> isins)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"INSERT INTO Isins (Identifier, Price) VALUES (@Identifier, @Price)";
                await connection.ExecuteAsync(sql, isins);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "An error occurred while processing your request.");
                throw new IsinInsertionException();
            }
        }

    }
}
