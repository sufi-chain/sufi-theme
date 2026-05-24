namespace SufiChain.KomTheme;

/// <summary>
/// Global options for the KomTheme.
/// Configures available styles and default style.
/// </summary>
public class KomThemeOptions
{
    /// <summary>
    /// Gets or sets the default style name.
    /// Defaults to <see cref="KomStyleNames.System"/>.
    /// </summary>
    public string DefaultStyle { get; set; } = KomStyleNames.System;

    /// <summary>
    /// Gets or sets the available styles that can be selected by users.
    /// Key is the style name (e.g., "light", "dark"), value is the style definition.
    /// </summary>
    public Dictionary<string, KomThemeStyle> Styles { get; set; }

    /// <summary>
    /// Creates a new instance of <see cref="KomThemeOptions"/> with default styles.
    /// </summary>
    public KomThemeOptions()
    {
        Styles = new Dictionary<string, KomThemeStyle>
        {
            { KomStyleNames.Light, new KomThemeStyle("Theme:Light", "bi bi-sun") },
            { KomStyleNames.Dark, new KomThemeStyle("Theme:Dark", "bi bi-moon-stars") },
            { KomStyleNames.Dim, new KomThemeStyle("Theme:Dim", "bi bi-cloud-moon") },
            { KomStyleNames.System, new KomThemeStyle("Theme:System", "bi bi-display") }
        };
    }
}
