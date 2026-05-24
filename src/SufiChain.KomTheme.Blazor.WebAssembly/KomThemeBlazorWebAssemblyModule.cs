using Microsoft.Extensions.DependencyInjection;
using SufiChain.KomTheme.Blazor.WebAssembly.Bundling;
using SufiChain.KomTheme.Blazor.WebAssembly.Toolbars;
using SufiChain.SufiAbp.UI.Authentication;
using SufiChain.SufiAbp.UI.Routing;
using SufiChain.SufiAbp.UI.Toolbars;
using Volo.Abp.Modularity;

namespace SufiChain.KomTheme.Blazor.WebAssembly;

/// <summary>
/// ABP Module for KomTheme Blazor WebAssembly hosting.
/// </summary>
[DependsOn(
    typeof(KomThemeBlazorWebAssemblyBundlingModule),
    typeof(KomThemeBlazorModule)
)]
public class KomThemeBlazorWebAssemblyModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Register this assembly for Blazor routing
        Configure<SufiAbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(KomThemeBlazorWebAssemblyModule).Assembly);
        });

        // Register toolbar contributor
        Configure<ToolbarOptions>(options =>
        {
            options.Contributors.Add(new KomThemeBlazorWebAssemblyToolbarContributor());
        });

        // Configure authentication options
        Configure<AuthenticationOptions>(options =>
        {
            options.LoginUrl = "Account/Login";
            options.LogoutUrl = "Account/Logout";
        });
    }
}
