using ApplicationAuthentication.Data;
using ApplicationAuthentication.Entities;
using ApplicationAuthentication.Repositories.Interfaces;
using Dapper;

namespace ApplicationAuthentication.Repositories;

public class GroupRepository : IGroupRepository {
	private readonly DatabaseConnection _databaseConnection;
	public GroupRepository(DatabaseConnection databaseConnection) {
		_databaseConnection = databaseConnection; }
	public async Task<IEnumerable<Group>> GetGroupsByUserIdAsync(string userId, string applicationId
		,CancellationToken cancellationToken=default)
	{
		const string sql = """
			SELECT
			g.GroupId,g.ApplicationId, g.GroupName, g.Description, g.IsActive, g.CreatedAt
			FROM Groups g
			INNER JOIN UserGroups ug
			     ON g.GroupId = ug.GroupId
				 AND g.ApplicationId = ug.ApplicationId
			WHERE ug.UserId =@UserId
			    AND g.ApplicationId = @ApplicationId
			    AND ug.IsActive = 1
			    AND g.IsActive =1;
			""";
		var command = new CommandDefinition (commandText :sql,
			parameters: new {UserId=userId, ApplicationId = applicationId }, cancellationToken: cancellationToken);
		using var connection = _databaseConnection.CreateConnection();
		var groups = await connection.QueryAsync<Group>(command);
		return groups; 
	}

}
