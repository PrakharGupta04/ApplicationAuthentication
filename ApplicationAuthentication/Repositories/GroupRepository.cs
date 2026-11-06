using ApplicationAuthentication.Data;
using ApplicationAuthentication.Entities;
using ApplicationAuthentication.Repositories.Interfaces;
using Dapper;

namespace ApplicationAuthentication.Repositories;

public class GroupRepository : IGroupRepository {
	private readonly DatabaseConnection _databaseConnection;
	public GroupRepository(DatabaseConnection databaseConnection) {
		_databaseConnection = databaseConnection; }
	public async Task<IEnumerable<Group>> GetGroupsByUserIdAsync(string userId)
	{
		const string sql = """
			SELECT
			g.GroupId, g.GroupName, g.Description, g.IsActive, g.CreatedAt
			FROM Groups g
			INNER JOIN UserGroups ug
			     ON g.GroupId = ug.GroupId
			WHERE ug.UserId =@UserId
			    AND g.IsActive =1;
			""";
		using var connection = _databaseConnection.CreateConnection();
		var groups = await connection.QueryAsync<Group>(sql, new { UserId = userId });
		return groups; 
	}

}
