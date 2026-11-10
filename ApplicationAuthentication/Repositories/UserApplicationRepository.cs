using ApplicationAuthentication.Data;
using ApplicationAuthentication.Repositories.Interfaces;
using Dapper;

namespace ApplicationAuthentication.Repositories;

public class UserApplicationRepository : IUserApplicationRepository
{
	private readonly DatabaseConnection _databaseConnection;
	public UserApplicationRepository(DatabaseConnection databaseConnection)
	{
		_databaseConnection = databaseConnection;
	}
	public async Task<bool> IsUserAssignedToApplicationAsync(string userId, string applicationId
		,CancellationToken cancellationToken=default)
	{
		const string sql = @"
			SELECT COUNT(1) 
			FROM UserApplications
			WHERE UserId = @UserId AND ApplicationId = @ApplicationId AND IsActive = 1;
		";
		var command = new CommandDefinition(commandText:sql,
			parameters: new { UserId = userId, ApplicationId = applicationId },
			cancellationToken: cancellationToken);
		using var connection = _databaseConnection.CreateConnection();
		var count = await connection.ExecuteScalarAsync<int>(command);
		return count > 0;
	}
}
