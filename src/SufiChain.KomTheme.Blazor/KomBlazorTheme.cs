using SufiChain.KomTheme.Blazor.Layouts;
using SufiChain.SufiAbp.UI.Blazor.Components;
using SufiChain.SufiAbp.UI.Blazor.Layouts;
using SufiChain.SufiAbp.UI.Layout;
using SufiChain.SufiAbp.UI.Theming;

namespace SufiChain.KomTheme.Blazor;

/// <summary>
/// KomTheme implementation using SufiBlazor design system.
/// Maps standard layout names to our SufiBlazor-based layouts.
/// </summary>
[ThemeName(Name)]
public class KomBlazorTheme : ITheme
{
    public const string Name = "KomBlazor";

    /// <inheritdoc/>
    public virtual Type? GetLayout(string name, bool fallbackToDefault = true)
    {
        return name switch
        {
            StandardLayouts.Application => typeof(SideMenuLayout),
            StandardLayouts.Account => typeof(AccountLayout),
            StandardLayouts.Empty => typeof(EmptyLayout),
            _ => fallbackToDefault ? typeof(SideMenuLayout) : typeof(SufiAbpNullLayout)
        };
    }

    /// <inheritdoc/>
    public virtual string? GetLayoutPath(string name, bool fallbackToDefault = true)
    {
        // This is a Blazor-only theme; MVC layout paths provided by MVC theme
        return name switch
        {
            StandardLayouts.Application => "~/Themes/KomTheme/Layouts/Application.cshtml",
            StandardLayouts.Account => "~/Themes/KomTheme/Layouts/Account.cshtml",
            StandardLayouts.Empty => "~/Themes/KomTheme/Layouts/Empty.cshtml",
            _ => fallbackToDefault ? "~/Themes/KomTheme/Layouts/Application.cshtml" : null
        };
    }
}
