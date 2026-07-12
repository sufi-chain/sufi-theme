using SufiChain.SufiAbp.UI.Branding;
using AbpBranding = Volo.Abp.Ui.Branding;

namespace SufiChain.SufiTheme.Blazor.Server.Branding;

/// <summary>
/// Adapter that bridges ABP's IBrandingProvider to SufiAbp's IBrandingProvider.
/// This allows SufiAbp UI components to use branding configured via ABP's system.
/// </summary>
public class AbpBrandingProviderAdapter : IBrandingProvider
{
    private readonly AbpBranding.IBrandingProvider _abpBrandingProvider;

    public AbpBrandingProviderAdapter(AbpBranding.IBrandingProvider abpBrandingProvider)
    {
        _abpBrandingProvider = abpBrandingProvider;
    }

    /// <inheritdoc/>
    public string AppName => _abpBrandingProvider.AppName;

    /// <inheritdoc/>
    public string? LogoUrl => _abpBrandingProvider.LogoUrl;

    /// <inheritdoc/>
    public string? LogoReverseUrl => _abpBrandingProvider.LogoReverseUrl;

    /// <inheritdoc/>
    public string? CopyrightText => null;
}
