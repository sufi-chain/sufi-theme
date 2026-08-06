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
                "/_content/SufiChain.SufiBlazor/sufiblazor.css");
            // SufiTheme layout styles
            options.StyleBundles.Add(BlazorSufiThemeBundles.Styles.Global, 
                "/_content/SufiChain.SufiTheme.Blazor/sufi-theme.css");

            options.ScriptBundles.Add(BlazorSufiThemeBundles.Scripts.Global, 
                "/_content/SufiChain.SufiBlazor/sufiblazor.js");
            options.ScriptBundles.Add(BlazorSufiThemeBundles.Scripts.Global, 
                "/_content/SufiChain.SufiTheme.Blazor/sufi-theme-viewport.js");

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
    /// SufiBlazor vendor bundles (Quill, etc.) for on-demand loading.
    /// </summary>
    public static class SufiBlazor
    {
        public const string Quill = "SufiBlazor.Quill";
        public const string MarkdownEditor = "SufiBlazor.MarkdownEditor";
    }
}
