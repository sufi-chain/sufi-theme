using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SufiChain.SufiTheme.Blazor.Server.Authorization;
using SufiChain.SufiTheme.Blazor.Server.Branding;
using SufiChain.SufiTheme.Blazor.Server.TenantSelector;
using SufiChain.SufiTheme.Blazor.Server.Bundling;
using SufiChain.SufiTheme.Blazor.Server.Toolbars;
using SufiChain.SufiTheme.Blazor.Server.Users;
using SufiChain.SufiAbp.UI.Authorization;
using SufiChain.SufiAbp.UI.Blazor.DependencyInjection;
using SufiChain.SufiAbp.UI.Blazor.Server.DependencyInjection;
using SufiChain.SufiAbp.UI.Branding;
using SufiChain.SufiAbp.UI.Bundling;
using SufiChain.SufiAbp.UI.MultiTenancy;
using SufiChain.SufiAbp.UI.Routing;
using SufiChain.SufiAbp.UI.Services.DependencyInjection;
using SufiChain.SufiAbp.UI.Toolbars;
using SufiChain.SufiAbp.UI.Users;
using Volo.Abp.Modularity;

using SufiChain.SufiAbp.UI;

namespace SufiChain.SufiTheme.Blazor.Server;

/// <summary>
/// ABP Module for SufiTheme Blazor Server hosting.
/// Uses SufiAbp's theming/bundling system with SufiBlazor design system.
/// Multi-tenancy is handled by ABP's built-in middleware (UseMultiTenancy);
/// this module only registers UI-level tenant selector services.
/// </summary>
[DependsOn(
    typeof(SufiThemeBlazorModule),
    typeof(SufiAbpUiDomainSharedModule)
)]
public class SufiThemeBlazorServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Bridge ABP's branding provider to SufiAbp's branding system
        // Registered before AddSufiAbpUIServices() so the adapter takes precedence over the default
        context.Services.AddSingleton<IBrandingProvider, AbpBrandingProviderAdapter>();

        // Override tenant selector visibility with ABP-backed implementation
        context.Services.Replace(ServiceDescriptor.Scoped<ITenantSelectorVisibilityService, AbpTenantSelectorVisibilityService>());

        // Bridge ABP current user and authorization for menu/toolbar permission filtering
        context.Services.Replace(ServiceDescriptor.Scoped<ICurrentUserAccessor, AbpCurrentUserAccessorAdapter>());
        context.Services.Replace(ServiceDescriptor.Scoped<ISufiAbpPermissionChecker, SufiThemeAuthorizationPermissionChecker>());

        // Register SufiAbp UI services (menu, toolbar, branding, etc.)
        context.Services.AddSufiAbpUIServices();

        // Register SufiAbp UI Blazor services (messages, notifications, tenant switch, etc.)
        context.Services.AddSufiAbpUIBlazor();

        // Blazor Server: isolate overlay components (toasts, block UI) per circuit/user session
        context.Services.AddSufiAbpBlazorServerCircuitServices();

        // Register this assembly for Blazor routing
        Configure<SufiAbpRouterOptions>(options =>
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
                "/_content/SufiChain.SufiBlazor/sufiblazor.css");
            // SufiTheme layout styles
            options.StyleBundles.Add(BlazorSufiThemeBundles.Styles.Global, 
                "/_content/SufiChain.SufiTheme.Blazor/sufi-theme.css");

            options.ScriptBundles.Add(BlazorSufiThemeBundles.Scripts.Global, 
                "/_content/SufiChain.SufiBlazor/sufiblazor.js");
            options.ScriptBundles.Add(BlazorSufiThemeBundles.Scripts.Global, 
                "/_content/SufiChain.SufiTheme.Blazor/sufi-theme-viewport.js");
            // sufi-theme.js is optional/on-demand (ES module); menu expand/collapse is Blazor + CSS only

            // Quill.js for SbRichTextEditor (on-demand, not in global bundle)
            options.StyleBundles.Add(BlazorSufiThemeBundles.SufiBlazor.Quill, 
                "/_content/SufiChain.SufiBlazor/vendor/quill.snow.css");
            options.ScriptBundles.Add(BlazorSufiThemeBundles.SufiBlazor.Quill, 
                "/_content/SufiChain.SufiBlazor/vendor/quill.min.js");

            options.StyleBundles.Add(BlazorSufiThemeBundles.SufiBlazor.MarkdownEditor,
                "/_content/SufiChain.SufiBlazor/vendor/easymde/easymde.min.css");
            options.ScriptBundles.Add(BlazorSufiThemeBundles.SufiBlazor.MarkdownEditor,
                "/_content/SufiChain.SufiBlazor/vendor/easymde/easymde.min.js");
        });
    }
}
