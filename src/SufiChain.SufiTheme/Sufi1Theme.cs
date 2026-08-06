using SufiChain.SufiPlatform.UI.Layout;
using SufiChain.SufiPlatform.UI.Theming;
using Microsoft.Extensions.Options;

namespace SufiChain.SufiTheme;

/// <summary>
/// The Sufi1 theme implementation.
/// Provides layout selection based on the configured theme options.
/// </summary>
[ThemeName(Name)]
public class Sufi1Theme : ITheme
{
    /// <summary>
    /// The name of the Sufi1 theme.
    /// </summary>
    public const string Name = "Sufi1";

    private readonly SufiThemeBlazorOptions _options;

    /// <summary>
    /// Creates a new instance of the Sufi1 theme.
    /// </summary>
    /// <param name="options">The Blazor theme options.</param>
    public Sufi1Theme(IOptions<SufiThemeBlazorOptions> options)
    {
        _options = options.Value;
    }

    /// <inheritdoc />
    public virtual Type? GetLayout(string name, bool fallbackToDefault = true)
    {
        return name switch
        {
            StandardLayouts.Application => _options.Layout,
            StandardLayouts.Account => SufiLayouts.IsRegistered ? SufiLayouts.Account : _options.Layout,
            StandardLayouts.Empty => SufiLayouts.IsRegistered ? SufiLayouts.Empty : _options.Layout,
            StandardLayouts.Public => _options.Layout,
            _ => fallbackToDefault ? _options.Layout : GetNullLayoutType()
        };
    }

    /// <inheritdoc />
    public virtual string? GetLayoutPath(string name, bool fallbackToDefault = true)
    {
        return name switch
        {
            StandardLayouts.Application => "~/Themes/SufiTheme/Layouts/Application.cshtml",
            StandardLayouts.Account => "~/Themes/SufiTheme/Layouts/Account.cshtml",
            StandardLayouts.Empty => "~/Themes/SufiTheme/Layouts/Empty.cshtml",
            _ => fallbackToDefault ? "~/Themes/SufiTheme/Layouts/Application.cshtml" : null
        };
    }

    /// <summary>
    /// Gets a null/empty layout type for unknown layout names when fallbackToDefault is false.
    /// </summary>
    private static Type GetNullLayoutType()
    {
        // Return the Empty layout if available, otherwise fall back to a basic type
        return SufiLayouts.IsRegistered ? SufiLayouts.Empty : typeof(object);
    }
}
