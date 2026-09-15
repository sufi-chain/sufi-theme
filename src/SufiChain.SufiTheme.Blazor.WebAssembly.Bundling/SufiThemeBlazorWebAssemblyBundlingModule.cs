using Microsoft.Extensions.DependencyInjection;
using SufiChain.SufiPlatform.UI.Bundling;
using Volo.Abp.Modularity;

namespace SufiChain.SufiTheme.Blazor.WebAssembly.Bundling;

/// <summary>
/// ABP Module for SufiTheme WebAssembly bundling.
/// Uses SufiAbp's bundling system - no ABP UI dependencies.
/// </summary>
public class SufiThemeBlazorWebAssemblyBundlingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
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

            options.StyleBundles.Add(BlazorSufiThemeBundles.Styles.Global,
                "/_content/SufiChain.SufiBlazor/sufiblazor-editors.css");

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

/// <summary>
/// Bundle names for SufiTheme WebAssembly.
/// </summary>
public static class BlazorSufiThemeBundles
{
    public static class Styles
    {
        public const string Global = "SufiTheme.Blazor.WebAssembly.Global";
    }

    public static class Scripts
    {
        public const string Global = "SufiTheme.Blazor.WebAssembly.Global";
    }

    /// <summary>
    /// SufiBlazor editor style bundles for on-demand loading.
    /// </summary>
    public static class SufiBlazor
    {
        public const string RichText = "SufiBlazor.RichText";
        public const string Code = "SufiBlazor.Code";
        public const string Diff = "SufiBlazor.Diff";
        public const string Viewer = "SufiBlazor.Viewer";
    }
}
