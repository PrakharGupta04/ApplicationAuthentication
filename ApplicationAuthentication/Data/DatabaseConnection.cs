using Microsoft.Data.SqlClient;

namespace ApplicationAuthentication.Data
{
    public class DatabaseConnection
    {
        private readonly string _connectionString;
        public DatabaseConnection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Database connection string not found");
        }
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

    }
}
