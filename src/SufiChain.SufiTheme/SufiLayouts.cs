namespace SufiChain.SufiTheme;

/// <summary>
/// Layout constants for the SufiTheme.
/// Defines the different layout variants available.
/// </summary>
public static class SufiLayoutNames
{
    /// <summary>
    /// Side menu layout - menu items are displayed in a sidebar on the left.
    /// </summary>
    public const string SideMenu = "SideMenu";

    /// <summary>
    /// Top menu layout - menu items are displayed in a horizontal navigation bar at the top.
    /// </summary>
    public const string TopMenu = "TopMenu";
    
    /// <summary>
    /// Dual sidebar layout - icon rail + expandable panel pattern.
    /// </summary>
    public const string DualSidebar = "DualSidebar";
}

/// <summary>
/// Static helper class providing layout type references.
/// Use this when configuring <see cref="SufiThemeBlazorOptions.Layout"/>.
/// </summary>
/// <remarks>
/// The actual layout types (SideMenuLayout, TopMenuLayout) are defined in the
/// SufiChain.SufiTheme.Blazor package. This class provides a convenient way to
/// reference them without hardcoding type names.
/// </remarks>
public static class SufiLayouts
{
    private static Type? _sideMenuLayout;
    private static Type? _topMenuLayout;
    private static Type? _dualSidebarLayout;
    private static Type? _accountLayout;
    private static Type? _emptyLayout;

    /// <summary>
    /// Gets or sets the SideMenu layout type.
    /// Set by the Blazor theme package during initialization.
    /// </summary>
    public static Type SideMenu
    {
        get => _sideMenuLayout ?? throw new InvalidOperationException(
            "SideMenuLayout type has not been registered. Ensure SufiChain.SufiTheme.Blazor is properly configured.");
        set => _sideMenuLayout = value;
    }

    /// <summary>
    /// Gets or sets the TopMenu layout type.
    /// Set by the Blazor theme package during initialization.
    /// </summary>
    public static Type TopMenu
    {
        get => _topMenuLayout ?? throw new InvalidOperationException(
            "TopMenuLayout type has not been registered. Ensure SufiChain.SufiTheme.Blazor is properly configured.");
        set => _topMenuLayout = value;
    }

    /// <summary>
    /// Gets or sets the DualSidebar layout type (icon rail + expandable panel).
    /// Set by the Blazor theme package during initialization.
    /// </summary>
    public static Type DualSidebar
    {
        get => _dualSidebarLayout ?? throw new InvalidOperationException(
            "DualSidebarLayout type has not been registered. Ensure SufiChain.SufiTheme.Blazor is properly configured.");
        set => _dualSidebarLayout = value;
    }

    /// <summary>
    /// Gets or sets the Account layout type (for login/register pages).
    /// Set by the Blazor theme package during initialization.
    /// </summary>
    public static Type Account
    {
        get => _accountLayout ?? throw new InvalidOperationException(
            "AccountLayout type has not been registered. Ensure SufiChain.SufiTheme.Blazor is properly configured.");
        set => _accountLayout = value;
    }

    /// <summary>
    /// Gets or sets the Empty layout type (minimal layout without navigation).
    /// Set by the Blazor theme package during initialization.
    /// </summary>
    public static Type Empty
    {
        get => _emptyLayout ?? throw new InvalidOperationException(
            "EmptyLayout type has not been registered. Ensure SufiChain.SufiTheme.Blazor is properly configured.");
        set => _emptyLayout = value;
    }

    /// <summary>
    /// Checks if the layouts have been registered.
    /// </summary>
    public static bool IsRegistered => _sideMenuLayout != null && _topMenuLayout != null;
}
