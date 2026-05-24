namespace SufiChain.KomTheme.Blazor.Server.Bundling;

/// <summary>
/// Bundle names for KomTheme Blazor Server (using SufiBlazor design system).
/// Uses different names than legacy theme to allow coexistence during migration.
/// </summary>
public static class BlazorKomThemeBundles
{
    public static class Styles
    {
        public const string Global = "Blazor.KomTheme.SufiBlazor.Global";
    }

    public static class Scripts
    {
        public const string Global = "Blazor.KomTheme.SufiBlazor.Global";
    }

    /// <summary>
    /// SufiBlazor vendor bundles (Quill, etc.) for on-demand loading.
    /// </summary>
    public static class SufiBlazor
    {
        public const string Quill = "SufiBlazor.Quill";
    }
}
