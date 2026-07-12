namespace SufiChain.SufiTheme;

/// <summary>
/// Global options for the SufiTheme.
/// Configures available styles and default style.
/// </summary>
public class SufiThemeOptions
{
    /// <summary>
    /// Gets or sets the default style name.
    /// Defaults to <see cref="SufiStyleNames.System"/>.
    /// </summary>
    public string DefaultStyle { get; set; } = SufiStyleNames.System;

    /// <summary>
    /// Gets or sets the available styles that can be selected by users.
    /// Key is the style name (e.g., "light", "dark"), value is the style definition.
    /// </summary>
    public Dictionary<string, SufiThemeStyle> Styles { get; set; }

    /// <summary>
    /// Creates a new instance of <see cref="SufiThemeOptions"/> with default styles.
    /// </summary>
    public SufiThemeOptions()
    {
        Styles = new Dictionary<string, SufiThemeStyle>
        {
            { SufiStyleNames.Light, new SufiThemeStyle("Theme:Light", "bi bi-sun") },
            { SufiStyleNames.Dark, new SufiThemeStyle("Theme:Dark", "bi bi-moon-stars") },
            { SufiStyleNames.Dim, new SufiThemeStyle("Theme:Dim", "bi bi-cloud-moon") },
            { SufiStyleNames.System, new SufiThemeStyle("Theme:System", "bi bi-display") }
        };
    }
}
