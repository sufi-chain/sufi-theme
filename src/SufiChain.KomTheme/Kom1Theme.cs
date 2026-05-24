using SufiChain.SufiAbp.UI.Layout;
using SufiChain.SufiAbp.UI.Theming;
using Microsoft.Extensions.Options;

namespace SufiChain.KomTheme;

/// <summary>
/// The Kom1 theme implementation.
/// Provides layout selection based on the configured theme options.
/// </summary>
[ThemeName(Name)]
public class Kom1Theme : ITheme
{
    /// <summary>
    /// The name of the Kom1 theme.
    /// </summary>
    public const string Name = "Kom1";

    private readonly KomThemeBlazorOptions _options;

    /// <summary>
    /// Creates a new instance of the Kom1 theme.
    /// </summary>
    /// <param name="options">The Blazor theme options.</param>
    public Kom1Theme(IOptions<KomThemeBlazorOptions> options)
    {
        _options = options.Value;
    }

    /// <inheritdoc />
    public virtual Type? GetLayout(string name, bool fallbackToDefault = true)
    {
        return name switch
        {
            StandardLayouts.Application => _options.Layout,
            StandardLayouts.Account => KomLayouts.IsRegistered ? KomLayouts.Account : _options.Layout,
            StandardLayouts.Empty => KomLayouts.IsRegistered ? KomLayouts.Empty : _options.Layout,
            StandardLayouts.Public => _options.Layout,
            _ => fallbackToDefault ? _options.Layout : GetNullLayoutType()
        };
    }

    /// <inheritdoc />
    public virtual string? GetLayoutPath(string name, bool fallbackToDefault = true)
    {
        return name switch
        {
            StandardLayouts.Application => "~/Themes/KomTheme/Layouts/Application.cshtml",
            StandardLayouts.Account => "~/Themes/KomTheme/Layouts/Account.cshtml",
            StandardLayouts.Empty => "~/Themes/KomTheme/Layouts/Empty.cshtml",
            _ => fallbackToDefault ? "~/Themes/KomTheme/Layouts/Application.cshtml" : null
        };
    }

    /// <summary>
    /// Gets a null/empty layout type for unknown layout names when fallbackToDefault is false.
    /// </summary>
    private static Type GetNullLayoutType()
    {
        // Return the Empty layout if available, otherwise fall back to a basic type
        return KomLayouts.IsRegistered ? KomLayouts.Empty : typeof(object);
    }
}
