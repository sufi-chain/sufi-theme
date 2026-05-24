using Microsoft.Extensions.DependencyInjection;
using SufiChain.SufiBlazor;
using SufiChain.KomTheme.Blazor.DependencyInjection;
using SufiChain.KomTheme.Blazor.Menus;
using SufiChain.KomTheme.Blazor.Toolbar;
using SufiChain.SufiAbp.UI.Navigation;
using SufiChain.SufiAbp.UI.Routing;
using SufiChain.SufiAbp.UI.Theming;
using Volo.Abp.Modularity;

namespace SufiChain.KomTheme.Blazor;

/// <summary>
/// ABP Module for KomTheme shared Blazor components.
/// Uses SufiAbp's theming system with SufiBlazor design system.
/// No dependency on ABP UI packages.
/// </summary>
public class KomThemeBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Register theme layouts and options
        context.Services.AddKomThemeBlazor();

        // Register RTE font-family dropdown (Dirooz, Samim, Gandom, Sahel FD)
        context.Services.AddRteToolbarContributor<FontFamilyToolbarContributor>();

        // Register the theme class as transient
        context.Services.AddTransient<KomBlazorTheme>();

        // Register this assembly for Blazor routing
        Configure<SufiAbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(KomThemeBlazorModule).Assembly);
        });

        // Register the KomTheme with SufiAbp's theming system
        Configure<ThemingOptions>(options =>
        {
            options.Themes.Add<KomBlazorTheme>();

            if (options.DefaultThemeName == null)
            {
                options.DefaultThemeName = KomBlazorTheme.Name;
            }
        });

        // Localize the Administration menu group label (e.g. "مدیریت" for Farsi)
        Configure<SufiAbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new AdministrationMenuLocalizationContributor());
        });
    }
}
