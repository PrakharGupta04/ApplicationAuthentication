using ApplicationAuthentication.Contracts;
using ApplicationAuthentication.Repositories.Interfaces;
using ApplicationAuthentication.Services.Interfaces;

namespace ApplicationAuthentication.Services;

public class UserAuthorizationProvider : IUserAuthorizationProvider
{
	private readonly IGroupRepository _groupRepository;
	private readonly IPermissionRepository _permissionRepository;

	public UserAuthorizationProvider(IGroupRepository groupRepository, IPermissionRepository permissionRepository)
	{

		_groupRepository = groupRepository;
		_permissionRepository = permissionRepository;
	}
	public async Task<UserAuthorizationResult?> GetCurrentAuthorizationAsync(string userId, string applicationId, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		var groups = await _groupRepository.GetGroupsByUserIdAsync(userId);
		cancellationToken.ThrowIfCancellationRequested();
		var permissions = await _permissionRepository.GetPermissionsByUserIdAsync(userId);
		return new UserAuthorizationResult
		{
			Roles = groups.Select(g => g.GroupName).ToList(),
			Permissions = permissions.Select(p => p.PermissionCode).ToList(),
		};
	}

}
