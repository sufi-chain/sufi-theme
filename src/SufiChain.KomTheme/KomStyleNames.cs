namespace SufiChain.KomTheme;

/// <summary>
/// Defines the style name constants for the KomTheme.
/// These correspond to different color schemes/appearances.
/// </summary>
public static class KomStyleNames
{
    /// <summary>
    /// Light theme style - bright background with dark text.
    /// </summary>
    public const string Light = "light";

    /// <summary>
    /// Dark theme style - dark background with light text.
    /// </summary>
    public const string Dark = "dark";

    /// <summary>
    /// Dim theme style - muted/dimmed appearance.
    /// </summary>
    public const string Dim = "dim";

    /// <summary>
    /// System theme style - follows the operating system's theme preference.
    /// </summary>
    public const string System = "system";

    /// <summary>
    /// Gets all available style names.
    /// </summary>
    public static IReadOnlyList<string> All => [Light, Dark, Dim, System];
}
