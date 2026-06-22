using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SufiChain.KomTheme.Blazor.Server.Branding;
using SufiChain.KomTheme.Blazor.Server.TenantSelector;
using SufiChain.KomTheme.Blazor.Server.Bundling;
using SufiChain.KomTheme.Blazor.Server.Toolbars;
using SufiChain.SufiAbp.UI;
using SufiChain.SufiAbp.UI.Blazor.DependencyInjection;
using SufiChain.SufiAbp.UI.Blazor.Server.DependencyInjection;
using SufiChain.SufiAbp.UI.Branding;
using SufiChain.SufiAbp.UI.Bundling;
using SufiChain.SufiAbp.UI.MultiTenancy;
using SufiChain.SufiAbp.UI.Routing;
using SufiChain.SufiAbp.UI.Services.DependencyInjection;
using SufiChain.SufiAbp.UI.Toolbars;
using Volo.Abp.Modularity;

namespace SufiChain.KomTheme.Blazor.Server;

/// <summary>
/// ABP Module for KomTheme Blazor Server hosting.
/// Uses SufiAbp's theming/bundling system with SufiBlazor design system.
/// Multi-tenancy is handled by ABP's built-in middleware (UseMultiTenancy);
/// this module only registers UI-level tenant selector services.
/// </summary>
[DependsOn(
    typeof(KomThemeBlazorModule),
    typeof(SufiAbpUiDomainSharedModule)
)]
public class KomThemeBlazorServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Bridge ABP's branding provider to SufiAbp's branding system
        // Registered before AddSufiAbpUIServices() so the adapter takes precedence over the default
        context.Services.AddSingleton<IBrandingProvider, AbpBrandingProviderAdapter>();

        // Override tenant selector visibility with ABP-backed implementation
        context.Services.Replace(ServiceDescriptor.Scoped<ITenantSelectorVisibilityService, AbpTenantSelectorVisibilityService>());

        // Register SufiAbp UI services (menu, toolbar, branding, etc.)
        context.Services.AddSufiAbpUIServices();

        // Register SufiAbp UI Blazor services (messages, notifications, tenant switch, etc.)
        context.Services.AddSufiAbpUIBlazor();

        // Blazor Server: isolate overlay components (toasts, block UI) per circuit/user session
        context.Services.AddSufiAbpBlazorServerCircuitServices();

        // Register this assembly for Blazor routing
        Configure<SufiAbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(KomThemeBlazorServerModule).Assembly);
        });

        // Register toolbar contributor with theme switcher
        Configure<ToolbarOptions>(options =>
        {
            options.Contributors.Add(new KomThemeBlazorServerToolbarContributor());
        });

        // Configure bundling to include SufiBlazor styles and KomTheme styles
        Configure<BundleOptions>(options =>
        {
            // SufiBlazor design system (primitives, tokens, utility classes)
            options.StyleBundles.Add(BlazorKomThemeBundles.Styles.Global, 
                "/_content/SufiChain.SufiBlazor/sufiblazor.css");
            // KomTheme layout styles
            options.StyleBundles.Add(BlazorKomThemeBundles.Styles.Global, 
                "/_content/SufiChain.KomTheme.Blazor/kom-theme.css");

            options.ScriptBundles.Add(BlazorKomThemeBundles.Scripts.Global, 
                "/_content/SufiChain.SufiBlazor/sufiblazor.js");
            options.ScriptBundles.Add(BlazorKomThemeBundles.Scripts.Global, 
                "/_content/SufiChain.KomTheme.Blazor/kom-theme-viewport.js");
            // kom-theme.js is an ES module - loaded on demand by MenuItemRenderer via import(), not in global bundle

            // Quill.js for SbRichTextEditor (on-demand, not in global bundle)
            options.StyleBundles.Add(BlazorKomThemeBundles.SufiBlazor.Quill, 
                "/_content/SufiChain.SufiBlazor/vendor/quill.snow.css");
            options.ScriptBundles.Add(BlazorKomThemeBundles.SufiBlazor.Quill, 
                "/_content/SufiChain.SufiBlazor/vendor/quill.min.js");

            options.StyleBundles.Add(BlazorKomThemeBundles.SufiBlazor.MarkdownEditor,
                "/_content/SufiChain.SufiBlazor/vendor/easymde/easymde.min.css");
            options.ScriptBundles.Add(BlazorKomThemeBundles.SufiBlazor.MarkdownEditor,
                "/_content/SufiChain.SufiBlazor/vendor/easymde/easymde.min.js");
        });
    }
}
