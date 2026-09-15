namespace SufiChain.SufiTheme.Blazor.Server.Bundling;

/// <summary>
/// Bundle names for SufiTheme Blazor Server (using SufiBlazor design system).
/// Uses different names than legacy theme to allow coexistence during migration.
/// </summary>
public static class BlazorSufiThemeBundles
{
    public static class Styles
    {
        public const string Global = "Blazor.SufiTheme.SufiBlazor.Global";
    }

    public static class Scripts
    {
        public const string Global = "Blazor.SufiTheme.SufiBlazor.Global";
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
