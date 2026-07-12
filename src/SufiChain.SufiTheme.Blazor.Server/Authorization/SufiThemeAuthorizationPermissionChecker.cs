using SufiChain.SufiPlatform.UI.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;

namespace SufiChain.SufiTheme.Blazor.Server.Authorization;

/// <summary>
/// Bridges ABP <see cref="IPermissionChecker"/> to SufiAbp menu/toolbar permission filtering.
/// </summary>
public class SufiThemeAuthorizationPermissionChecker : ISufiPermissionChecker
{
    private readonly IPermissionChecker _permissionChecker;

    public SufiThemeAuthorizationPermissionChecker(IPermissionChecker permissionChecker)
    {
        _permissionChecker = permissionChecker;
    }

    public Task<bool> IsGrantedAsync(string permissionName)
    {
        return _permissionChecker.IsGrantedAsync(permissionName);
    }

    public async Task<Dictionary<string, bool>> IsGrantedAsync(IEnumerable<string> permissionNames)
    {
        var result = new Dictionary<string, bool>(StringComparer.Ordinal);
        foreach (var permissionName in permissionNames.Distinct(StringComparer.Ordinal))
        {
            result[permissionName] = await _permissionChecker.IsGrantedAsync(permissionName);
        }

        return result;
    }
}
