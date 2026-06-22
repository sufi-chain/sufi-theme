using Microsoft.Extensions.DependencyInjection;
using SufiChain.SufiAbp.UI.Bundling;
using Volo.Abp.Modularity;

namespace SufiChain.KomTheme.Blazor.WebAssembly.Bundling;

/// <summary>
/// ABP Module for KomTheme WebAssembly bundling.
/// Uses SufiAbp's bundling system - no ABP UI dependencies.
/// </summary>
public class KomThemeBlazorWebAssemblyBundlingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
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

/// <summary>
/// Bundle names for KomTheme WebAssembly.
/// </summary>
public static class BlazorKomThemeBundles
{
    public static class Styles
    {
        public const string Global = "KomTheme.Blazor.WebAssembly.Global";
    }

    public static class Scripts
    {
        public const string Global = "KomTheme.Blazor.WebAssembly.Global";
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
