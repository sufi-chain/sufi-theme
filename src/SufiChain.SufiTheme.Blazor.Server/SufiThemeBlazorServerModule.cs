using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SufiChain.SufiTheme.Blazor.Server.Authorization;
using SufiChain.SufiTheme.Blazor.Server.Branding;
using SufiChain.SufiTheme.Blazor.Server.TenantSelector;
using SufiChain.SufiTheme.Blazor.Server.Bundling;
using SufiChain.SufiTheme.Blazor.Server.Toolbars;
using SufiChain.SufiTheme.Blazor.Server.Users;
using SufiChain.SufiPlatform.UI.Authorization;
using SufiChain.SufiPlatform.UI.Blazor.DependencyInjection;
using SufiChain.SufiPlatform.UI.Blazor.Server.DependencyInjection;
using SufiChain.SufiPlatform.UI.Branding;
using SufiChain.SufiPlatform.UI.Bundling;
using SufiChain.SufiPlatform.UI.MultiTenancy;
using SufiChain.SufiPlatform.UI.Routing;
using SufiChain.SufiPlatform.UI.Services.DependencyInjection;
using SufiChain.SufiPlatform.UI.Toolbars;
using SufiChain.SufiPlatform.UI.Users;
using Volo.Abp.Modularity;

using SufiChain.SufiPlatform.UI;

namespace SufiChain.SufiTheme.Blazor.Server;

/// <summary>
/// ABP Module for SufiTheme Blazor Server hosting.
/// Uses SufiAbp's theming/bundling system with SufiBlazor design system.
/// Multi-tenancy is handled by ABP's built-in middleware (UseMultiTenancy);
/// this module only registers UI-level tenant selector services.
/// </summary>
[DependsOn(
    typeof(SufiThemeBlazorModule),
    typeof(SufiUiDomainSharedModule)
)]
public class SufiThemeBlazorServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Bridge ABP's branding provider to Sufi Platform branding system
        // Registered before AddSufiUIServices() so the adapter takes precedence over the default
        context.Services.AddSingleton<IBrandingProvider, AbpBrandingProviderAdapter>();

        // Override tenant selector visibility with ABP-backed implementation
        context.Services.Replace(ServiceDescriptor.Scoped<ITenantSelectorVisibilityService, AbpTenantSelectorVisibilityService>());

        // Bridge ABP current user, current tenant, and authorization for menu/toolbar filtering
        context.Services.Replace(ServiceDescriptor.Scoped<ICurrentUserAccessor, AbpCurrentUserAccessorAdapter>());
        context.Services.Replace(ServiceDescriptor.Scoped<ISufiPermissionChecker, SufiThemeAuthorizationPermissionChecker>());

        // Register Sufi Platform UI services (menu, toolbar, branding, etc.)
        context.Services.AddSufiUIServices();

        // Register Sufi Platform UI Blazor services (messages, notifications, tenant switch, etc.)
        context.Services.AddSufiUIBlazor();
        context.Services.AddSufiBlazorServerCurrentTenant();

        // Blazor Server: isolate overlay components (toasts, block UI) per circuit/user session
        context.Services.AddSufiBlazorServerCircuitServices();

        // Register this assembly for Blazor routing
        Configure<SufiRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(SufiThemeBlazorServerModule).Assembly);
        });

        // Register toolbar contributor with theme switcher
        Configure<ToolbarOptions>(options =>
        {
            options.Contributors.Add(new SufiThemeBlazorServerToolbarContributor());
        });

        // Configure bundling to include SufiBlazor styles and SufiTheme styles
        Configure<BundleOptions>(options =>
        {
            // SufiBlazor design system (primitives, tokens, utility classes)
            options.StyleBundles.Add(BlazorSufiThemeBundles.Styles.Global,
                "/_content/SufiChain.SufiBlazor/persian-palette.css");
            options.StyleBundles.Add(BlazorSufiThemeBundles.Styles.Global,
                "/_content/SufiChain.SufiBlazor/sufiblazor.css");
            // SufiTheme layout styles
            options.StyleBundles.Add(BlazorSufiThemeBundles.Styles.Global,
                "/_content/SufiChain.SufiTheme.Blazor/sufi-theme.css");

            options.ScriptBundles.Add(BlazorSufiThemeBundles.Scripts.Global,
                "/_content/SufiChain.SufiBlazor/sufiblazor.js");
            options.ScriptBundles.Add(BlazorSufiThemeBundles.Scripts.Global,
                "/_content/SufiChain.SufiTheme.Blazor/sufi-theme-viewport.js");
            // sufi-theme.js is optional/on-demand (ES module); menu expand/collapse is Blazor + CSS only

            options.StyleBundles.Add(BlazorSufiThemeBundles.Styles.Global,
                "/_content/SufiChain.SufiBlazor/sufiblazor-editors.css");

            // Editor runtimes (on-demand ES modules; CSS for hosts that still use named bundles)
            options.StyleBundles.Add(BlazorSufiThemeBundles.SufiBlazor.RichText,
                "/_content/SufiChain.SufiBlazor/sufiblazor-editors.css");
            options.StyleBundles.Add(BlazorSufiThemeBundles.SufiBlazor.Code,
                "/_content/SufiChain.SufiBlazor/sufiblazor-editors.css");
            options.StyleBundles.Add(BlazorSufiThemeBundles.SufiBlazor.Diff,
                "/_content/SufiChain.SufiBlazor/sufiblazor-editors.css");
            options.StyleBundles.Add(BlazorSufiThemeBundles.SufiBlazor.Viewer,
                "/_content/SufiChain.SufiBlazor/sufiblazor-editors.css");
        });
    }
}
