using SufiChain.SufiAbp.UI.Navigation;

namespace SufiChain.KomTheme;

/// <summary>
/// Model for menu items used in mobile menu selection.
/// </summary>
public class MenuItemInfo
{
    /// <summary>
    /// Gets or sets the menu item data.
    /// </summary>
    public ApplicationMenuItem MenuItem { get; set; } = null!;

    /// <summary>
    /// Gets or sets the nesting level of the menu item (0 = root).
    /// </summary>
    public int Level { get; set; }
}

/// <summary>
/// Represents a host-provided shortcut rendered in the mobile bottom menu.
/// </summary>
public class MobileMenuShortcut
{
    public string Name { get; }

    public string DisplayName { get; }

    public string Url { get; }

    public string Icon { get; }

    public int Order { get; set; }

    public MobileMenuShortcut(string name, string displayName, string url, string icon, int order = 0)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
        Url = url ?? throw new ArgumentNullException(nameof(url));
        Icon = icon ?? throw new ArgumentNullException(nameof(icon));
        Order = order;
    }
}

/// <summary>
/// Blazor-specific options for the KomTheme.
/// Configures the layout type and mobile menu behavior.
/// </summary>
public class KomThemeBlazorOptions
{
    private Type? _layout;

    /// <summary>
    /// Gets or sets the default layout component type.
    /// If not set, defaults to <see cref="KomLayouts.SideMenu"/>.
    /// </summary>
    /// <example>
    /// <code>
    /// Configure&lt;KomThemeBlazorOptions&gt;(options =>
    /// {
    ///     options.Layout = KomLayouts.SideMenu;
    ///     // Or for top menu:
    ///     options.Layout = KomLayouts.TopMenu;
    ///     // Or a custom layout:
    ///     options.Layout = typeof(MyCustomLayout);
    /// });
    /// </code>
    /// </example>
    public Type Layout
    {
        get => _layout ?? (KomLayouts.IsRegistered ? KomLayouts.SideMenu : throw new InvalidOperationException(
            "Layout type is not set and default layouts have not been registered. " +
            "Either set the Layout property explicitly or ensure SufiChain.KomTheme.Blazor is configured."));
        set => _layout = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets or sets the selector function for mobile menu items.
    /// This function filters and/or transforms the main menu items for mobile display.
    /// By default, returns the first 2 menu items.
    /// </summary>
    /// <example>
    /// <code>
    /// Configure&lt;KomThemeBlazorOptions&gt;(options =>
    /// {
    ///     // Show only Home and Dashboard on mobile
    ///     options.MobileMenuSelector = items => items
    ///         .Where(x => x.MenuItem.Name == "Home" || x.MenuItem.Name == "Dashboard");
    ///     
    ///     // Or show all items
    ///     options.MobileMenuSelector = items => items;
    /// });
    /// </code>
    /// </example>
    public Func<IEnumerable<MenuItemInfo>, IEnumerable<MenuItemInfo>> MobileMenuSelector { get; set; }
        = items => items.Take(2);

    /// <summary>
    /// Gets or sets whether to show the sidebar toggle button in the main header.
    /// Defaults to true.
    /// </summary>
    public bool ShowSidebarToggle { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the sidebar should be collapsed by default on desktop.
    /// Defaults to false (expanded).
    /// </summary>
    public bool CollapsedSidebar { get; set; } = false;

    /// <summary>
    /// Gets or sets whether to show breadcrumbs in the content area.
    /// Defaults to true.
    /// </summary>
    public bool ShowBreadcrumbs { get; set; } = true;

    /// <summary>
    /// Gets or sets whether to show the page toolbar in the content area.
    /// Defaults to true.
    /// </summary>
    public bool ShowPageToolbar { get; set; } = true;

    /// <summary>
    /// Gets or sets whether to show the footer.
    /// Defaults to true.
    /// </summary>
    public bool ShowFooter { get; set; } = true;

    /// <summary>
    /// Gets or sets the copyright text shown in the theme footer.
    /// Defaults to "Copyright © SufiChain". Hosts can override via Configure&lt;KomThemeBlazorOptions&gt;(options => options.CopyrightText = "...").
    /// </summary>
    public string CopyrightText { get; set; } = "Copyright © SufiChain";

    /// <summary>
    /// Gets or sets whether the icon rail should use dark mode styling.
    /// Only applies to DualSidebar layout. Defaults to true.
    /// </summary>
    public bool IconRailDarkMode { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the expandable panel should expand on hover.
    /// Only applies to DualSidebar layout. Defaults to true.
    /// </summary>
    public bool ExpandOnHover { get; set; } = true;

    /// <summary>
    /// Gets host-provided shortcuts for the mobile bottom menu.
    /// Layouts render only these explicit shortcuts and never add hardcoded items.
    /// </summary>
    public List<MobileMenuShortcut> MobileShortcuts { get; } = new();
}
