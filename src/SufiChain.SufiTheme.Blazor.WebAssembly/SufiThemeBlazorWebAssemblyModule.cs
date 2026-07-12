using Microsoft.Extensions.DependencyInjection;
using SufiChain.SufiTheme.Blazor.WebAssembly.Bundling;
using SufiChain.SufiTheme.Blazor.WebAssembly.Toolbars;
using SufiChain.SufiAbp.UI.Authentication;
using SufiChain.SufiAbp.UI.Routing;
using SufiChain.SufiAbp.UI.Toolbars;
using Volo.Abp.Modularity;

namespace SufiChain.SufiTheme.Blazor.WebAssembly;

/// <summary>
/// ABP Module for SufiTheme Blazor WebAssembly hosting.
/// </summary>
[DependsOn(
    typeof(SufiThemeBlazorWebAssemblyBundlingModule),
    typeof(SufiThemeBlazorModule)
)]
public class SufiThemeBlazorWebAssemblyModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Register this assembly for Blazor routing
        Configure<SufiAbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(SufiThemeBlazorWebAssemblyModule).Assembly);
        });

        // Register toolbar contributor
        Configure<ToolbarOptions>(options =>
        {
            options.Contributors.Add(new SufiThemeBlazorWebAssemblyToolbarContributor());
        });

        // Configure authentication options
        Configure<AuthenticationOptions>(options =>
        {
            options.LoginUrl = "Account/Login";
            options.LogoutUrl = "Account/Logout";
        });
    }
}
