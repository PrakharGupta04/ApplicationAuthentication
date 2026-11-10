using ApplicationAuthentication.Data;
using ApplicationAuthentication.Entities;
using ApplicationAuthentication.Repositories.Interfaces;
using Dapper;

namespace ApplicationAuthentication.Repositories

{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseConnection _databaseConnection;
        public UserRepository(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        public async Task<User?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            const string sql = """
                
                SELECT UserId, UserName, PasswordHash,IsActive,IsBlocked,CreatedAt 
                FROM Users
                WHERE UserId = @UserId;
                
                """;
            var command = new CommandDefinition(commandText: sql,
                parameters: new { UserId = userId }, cancellationToken: cancellationToken);
            using var connection = _databaseConnection.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(command);
        }

        public async Task UpdatePasswordHashAsync(string userId, string newPasswordHash, CancellationToken cancellationToken = default)
		{
			const string sql = """
                
                UPDATE Users
                SET PasswordHash = @NewPasswordHash
                WHERE UserId = @UserId;
                
                """;
            var command = new CommandDefinition(commandText: sql, 
                parameters:new {UserId=userId,NewPasswordHash=newPasswordHash }, cancellationToken: cancellationToken);
			using var connection = _databaseConnection.CreateConnection();
			await connection.ExecuteAsync(command);
		}
	}
}
