using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SufiChain.SufiPlatform.UI.MultiTenancy;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;
using ICurrentTenant = SufiChain.SufiPlatform.UI.MultiTenancy.ICurrentTenant;

namespace SufiChain.SufiTheme.Blazor.Server.TenantSelector;

/// <summary>
/// ABP-backed implementation of ITenantSelectorVisibilityService.
/// Always shows tenant selector when multi-tenancy is enabled (required for login/register).
/// The SufiUi.TenantSelector.Mode feature controls the mode (InputName, SelectFromList, Search).
/// Reads the feature in host context so the same mode is used whether the user is on host or a tenant.
/// </summary>
public class AbpTenantSelectorVisibilityService : ITenantSelectorVisibilityService, ITransientDependency
{
    public const string TenantSelectorModeFeatureName = "SufiUI.TenantSelector.Mode";
    public const string ModeInputName = "InputName";

    private readonly AbpMultiTenancyOptions _multiTenancyOptions;
    private readonly IServiceProvider _serviceProvider;
    private readonly ICurrentTenant _currentTenant;

    public AbpTenantSelectorVisibilityService(
        IOptions<AbpMultiTenancyOptions> multiTenancyOptions,
        IServiceProvider serviceProvider,
        ICurrentTenant currentTenant)
    {
        _multiTenancyOptions = multiTenancyOptions.Value;
        _serviceProvider = serviceProvider;
        _currentTenant = currentTenant;
    }

    public async Task<(bool Show, string Mode)> GetOptionsAsync()
    {
        if (!_multiTenancyOptions.IsEnabled)
        {
            return (false, ModeInputName);
        }

        var featureChecker = _serviceProvider.GetService<IFeatureChecker>();
        string? featureValue = null;

        if (featureChecker != null)
        {
            try
            {
                // Read feature in host context so the same mode is used when on host or tenant
                using (_currentTenant.Change(null))
                {
                    featureValue = await featureChecker.GetOrNullAsync(TenantSelectorModeFeatureName);
                }
            }
            catch
            {
                featureValue = ModeInputName;
            }
        }

        bool show = _multiTenancyOptions.IsEnabled;
        string mode = string.IsNullOrEmpty(featureValue) ? ModeInputName : featureValue;

        return (show, mode);
    }
}
