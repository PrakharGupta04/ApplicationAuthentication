using ApplicationAuthentication.Data;
using ApplicationAuthentication.Entities;
using ApplicationAuthentication.Repositories.Interfaces;
using Dapper;

namespace ApplicationAuthentication.Repositories;

public class PermissionRepository : IPermissionRepository
{
	private readonly DatabaseConnection _databaseConnection;
	public PermissionRepository(DatabaseConnection databaseConnection)
	{
		_databaseConnection = databaseConnection;
	}
	public async Task<IEnumerable<Permission>> GetPermissionsByUserIdAsync(string userId)
	{
		const string sql = """
			SELECT DISTINCT p.PermissionId, p.PermissionCode, p.PermissionName, p.Description, p.IsActive, p.CreatedAt
			FROM Permissions p
			INNER JOIN GroupPermissions gp
			    ON p.PermissionId = gp.PermissionId
			INNER JOIN Groups g
			    ON gp.GroupId = g.GroupId
			INNER JOIN UserGroups ug
			    ON g.GroupId = ug.GroupId
			WHERE ug.UserId = @UserId
			  AND g.IsActive = 1
			  AND p.IsActive = 1;
			""";
		using var connection = _databaseConnection.CreateConnection();
		var permissions = await connection.QueryAsync<Permission>(sql, new { UserId = userId });
		return permissions;

	}
}
