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
	public async Task<bool> IsUserAssignedToApplicationAsync(string userId, string applicationId)
	{
		const string sql = @"""
			SELECT COUNT(1) 
			FROM UserApplications
			WHERE UserId = @UserId AND ApplicationId = @ApplicationId AND IsActive = 1;
		""";
		using var connection = _databaseConnection.CreateConnection();
		var count = await connection.ExecuteScalarAsync<int>(sql, new { UserId = userId, ApplicationId = applicationId });
		return count > 0;
	}
}
