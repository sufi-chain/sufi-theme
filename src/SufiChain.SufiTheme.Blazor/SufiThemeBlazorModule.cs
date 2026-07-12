using Microsoft.Extensions.DependencyInjection;
using SufiChain.SufiBlazor;
using SufiChain.SufiTheme.Blazor.DependencyInjection;
using SufiChain.SufiTheme.Blazor.Menus;
using SufiChain.SufiTheme.Blazor.Toolbar;
using SufiChain.SufiPlatform.UI.Navigation;
using SufiChain.SufiPlatform.UI.Routing;
using SufiChain.SufiPlatform.UI.Theming;
using Volo.Abp.Modularity;

namespace SufiChain.SufiTheme.Blazor;

/// <summary>
/// ABP Module for SufiTheme shared Blazor components.
/// Uses SufiAbp's theming system with SufiBlazor design system.
/// No dependency on ABP UI packages.
/// </summary>
public class SufiThemeBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Register theme layouts and options
        context.Services.AddSufiThemeBlazor();

        // Register RTE font-family dropdown (Dirooz, Samim, Gandom, Sahel FD)
        context.Services.AddRteToolbarContributor<FontFamilyToolbarContributor>();

        // Register the theme class as transient
        context.Services.AddTransient<SufiBlazorTheme>();

        // Register this assembly for Blazor routing
        Configure<SufiRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(SufiThemeBlazorModule).Assembly);
        });

        // Register the SufiTheme with SufiAbp's theming system
        Configure<ThemingOptions>(options =>
        {
            options.Themes.Add<SufiBlazorTheme>();

            if (options.DefaultThemeName == null)
            {
                options.DefaultThemeName = SufiBlazorTheme.Name;
            }
        });

        // Localize the Administration menu group label (e.g. "مدیریت" for Farsi)
        Configure<SufiNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new AdministrationMenuLocalizationContributor());
        });
    }
}
