using ApplicationAuthentication.Data;
using ApplicationAuthentication.Entities;
using ApplicationAuthentication.Repositories.Interfaces;
using Dapper;

namespace ApplicationAuthentication.Repositories;

public class ApplicationRepository : IApplicationRepository
{
	private readonly DatabaseConnection _databaseConnection;
	public ApplicationRepository(DatabaseConnection databaseConnection)
	{
		_databaseConnection = databaseConnection;
	}
	public async Task<Application?> GetByApplicationIdAsync(string applicationId) {
		const string sql = """
			SELECT ApplicationId, ApplicationName, HashKey,IsActive,CreatedAt
			FROM Applications
			WHERE ApplicationId = @ApplicationId;
			""";
		using var connection = _databaseConnection.CreateConnection();
		var application = await connection.QueryFirstOrDefaultAsync<Application>(sql, new { ApplicationId = applicationId });

		return application;
	}
}
