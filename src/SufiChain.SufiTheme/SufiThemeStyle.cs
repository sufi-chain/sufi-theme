namespace SufiChain.SufiTheme;

/// <summary>
/// Represents a theme style definition with display name and icon.
/// </summary>
public class SufiThemeStyle
{
    /// <summary>
    /// Gets or sets the display name of the theme style.
    /// Can be a localization key or direct text.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the icon CSS class for the theme style.
    /// Example: "bi bi-sun" for light, "bi bi-moon" for dark.
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// Creates a new instance of <see cref="SufiThemeStyle"/>.
    /// </summary>
    public SufiThemeStyle()
    {
        DisplayName = string.Empty;
        Icon = string.Empty;
    }

    /// <summary>
    /// Creates a new instance of <see cref="SufiThemeStyle"/> with specified values.
    /// </summary>
    /// <param name="displayName">The display name or localization key.</param>
    /// <param name="icon">The icon CSS class.</param>
    public SufiThemeStyle(string displayName, string icon)
    {
        DisplayName = displayName;
        Icon = icon;
    }
}
