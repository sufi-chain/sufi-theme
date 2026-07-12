using Microsoft.Extensions.DependencyInjection;
using SufiChain.SufiTheme.Blazor.WebAssembly.Bundling;
using SufiChain.SufiTheme.Blazor.WebAssembly.Toolbars;
using SufiChain.SufiPlatform.UI.Authentication;
using SufiChain.SufiPlatform.UI.Routing;
using SufiChain.SufiPlatform.UI.Toolbars;
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
        Configure<SufiRouterOptions>(options =>
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
