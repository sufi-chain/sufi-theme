using System;
using System.Security.Claims;
using SufiChain.SufiAbp.UI.Users;
using Volo.Abp.Users;

namespace SufiChain.SufiTheme.Blazor.Server.Users;

/// <summary>
/// Bridges ABP's <see cref="ICurrentUser"/> to SufiAbp's <see cref="ICurrentUserAccessor"/>.
/// </summary>
public class AbpCurrentUserAccessorAdapter : ICurrentUserAccessor
{
    private readonly ICurrentUser _currentUser;

    public AbpCurrentUserAccessorAdapter(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public bool IsAuthenticated => _currentUser.IsAuthenticated;

    public Guid? Id => _currentUser.Id;

    public string? UserName => _currentUser.UserName;

    public string? Name => _currentUser.Name;

    public string? SurName => _currentUser.SurName;

    public string? PhoneNumber => _currentUser.PhoneNumber;

    public bool PhoneNumberVerified => _currentUser.PhoneNumberVerified;

    public string? Email => _currentUser.Email;

    public bool EmailVerified => _currentUser.EmailVerified;

    public Guid? TenantId => _currentUser.TenantId;

    public string[] Roles => _currentUser.Roles;

    public Claim? FindClaim(string claimType) => _currentUser.FindClaim(claimType);

    public Claim[] FindClaims(string claimType) => _currentUser.FindClaims(claimType);

    public Claim[] GetAllClaims() => _currentUser.GetAllClaims();

    public bool IsInRole(string roleName) => _currentUser.IsInRole(roleName);
}
