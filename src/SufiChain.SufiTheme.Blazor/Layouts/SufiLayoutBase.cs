using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using SufiChain.SufiTheme.Blazor.Menus;
using SufiChain.SufiBlazor.Components;
using SufiChain.SufiBlazor.Theming;
using SufiChain.SufiPlatform.UI.Branding;
using SufiChain.SufiPlatform.UI.Layout;
using SufiChain.SufiPlatform.UI.Localization;
using SufiChain.SufiPlatform.UI.Navigation;
using SufiChain.SufiPlatform.UI.Theming;
using SufiChain.SufiPlatform.UI.Toolbars;
using SufiChain.SufiPlatform.UI.Users;

namespace SufiChain.SufiTheme.Blazor.Layouts;

/// <summary>
/// Base class for SufiTheme layouts containing shared functionality for:
/// - Menu and toolbar loading
/// - Breadcrumb management
/// - Navigation change handling
/// - Page layout property change handling
/// - RTL detection
/// </summary>
public abstract class SufiLayoutBase : LayoutComponentBase, IDisposable
{
    [Inject]
    protected IBrandingProvider BrandingProvider { get; set; } = default!;

    [Inject]
    protected IMenuManager MenuManager { get; set; } = default!;

    [Inject]
    protected IToolbarManager ToolbarManager { get; set; } = default!;

    [Inject]
    protected IPageLayout PageLayout { get; set; } = default!;

    [Inject]
    protected IBreadcrumbService BreadcrumbService { get; set; } = default!;

    [Inject]
    protected IOptions<SufiThemeBlazorOptions> BlazorOptions { get; set; } = default!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    protected IThemeSwitchService ThemeSwitchService { get; set; } = default!;

    [Inject]
    protected IStringLocalizer<SufiFrameworkResource> L { get; set; } = default!;

    [Inject]
    protected IPublicMenuProvider PublicMenuProvider { get; set; } = default!;

    [Inject]
    protected PersistentComponentState PersistentState { get; set; } = default!;

    [Inject]
    protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    [Inject]
    protected ICurrentUserAccessor MenuUser { get; set; } = default!;

    // Common state fields
    protected SbTheme CurrentTheme { get; set; } = SbTheme.Light;
    protected SbDirection Direction { get; set; } = SbDirection.Ltr;
    protected List<ApplicationMenuItem> MenuItems { get; set; } = new();
    protected List<ToolbarItem> ToolbarItems { get; set; } = new();
    
    /// <summary>
    /// Version counter that increments when page layout properties change.
    /// Used to force child components (like SufiTopBar) to re-render when
    /// their RenderFragment parameters would produce different output.
    /// </summary>
    protected int LayoutContentVersion { get; private set; }

    /// <summary>
    /// Copyright text for the footer. Uses branding provider if set, otherwise SufiTheme default ("Copyright © SufiChain").
    /// Hosts can override via IBrandingProvider.CopyrightText or SufiThemeBlazorOptions.CopyrightText.
    /// </summary>
    protected string CopyrightText => BrandingProvider.CopyrightText ?? BlazorOptions.Value.CopyrightText;

    /// <summary>
    /// Whether the header should show breadcrumbs. A single-item trail that repeats
    /// the page title is hidden.
    /// </summary>
    protected bool ShouldShowPageBreadcrumbs
    {
        get
        {
            if (!BlazorOptions.Value.ShowBreadcrumbs)
            {
                return false;
            }

            var items = PageLayout.BreadcrumbItems;
            if (items.Count == 0)
            {
                return false;
            }

            if (items.Count == 1
                && !string.IsNullOrWhiteSpace(PageLayout.Title)
                && string.Equals(items[0].Text?.Trim(), PageLayout.Title.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }

    private string? _currentUrl;
    private bool _needsBreadcrumbUpdate;
    private PersistingComponentStateSubscription? _shellPersist;
    private bool _authenticationStateSubscribed;

    [CascadingParameter(Name = "SufiResolvedThemeIsDark")]
    public bool? ResolvedThemeIsDark { get; set; }

    protected override async Task OnInitializedAsync()
    {
        // Before the first await. The interactive circuit otherwise paints an empty rail
        // over the prerendered icons and the shell looks collapsed until this method finishes.
        RestoreShellChrome();
        try
        {
            _shellPersist = PersistentState.RegisterOnPersisting(PersistShellChrome);
        }
        catch (InvalidOperationException)
        {
            // Persistence is already registered for this render mode.
        }

        AuthenticationStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
        _authenticationStateSubscribed = true;

        if (ResolvedThemeIsDark is bool isDark)
        {
            CurrentTheme = isDark ? SbTheme.Dark : SbTheme.Light;
            ThemeSwitchService.SeedResolved(isDark);
        }

        var culture = System.Globalization.CultureInfo.CurrentUICulture;
        Direction = culture.TextInfo.IsRightToLeft ? SbDirection.Rtl : SbDirection.Ltr;

        // Subscribe to PageLayout changes to update UI when page sets title/toolbar
        PageLayout.PropertyChanged += OnPageLayoutChanged;

        // Subscribe to navigation changes to handle breadcrumb/toolbar updates
        _currentUrl = NavigationManager.Uri;
        NavigationManager.LocationChanged += OnLocationChanged;

        try
        {
            await ReloadShellChromeAsync();
        }
        catch (ObjectDisposedException)
        {
            return;
        }
        catch (OperationCanceledException)
        {
            return;
        }

        // Allow derived layouts to perform additional initialization
        await OnInitializedLayoutAsync();

        await SyncThemeFromServiceAsync();
        ThemeSwitchService.ThemeChanged += OnThemeChanged;

        // Generate initial breadcrumbs from menu hierarchy
        await UpdateBreadcrumbsAsync();
    }

    private async Task SyncThemeFromServiceAsync()
    {
        try
        {
            await ThemeSwitchService.GetStoredThemeAsync();
            ApplyThemeFromServiceIfChanged();
        }
        catch
        {
            // Keep the theme already chosen from the saved preference.
        }
    }

    private void ApplyThemeFromServiceIfChanged()
    {
        var theme = ThemeSwitchService.IsDarkMode ? SbTheme.Dark : SbTheme.Light;
        if (CurrentTheme == theme)
        {
            return;
        }

        CurrentTheme = theme;
    }

    private void OnThemeChanged(ThemeMode _)
    {
        var theme = ThemeSwitchService.IsDarkMode ? SbTheme.Dark : SbTheme.Light;
        if (CurrentTheme == theme)
        {
            return;
        }

        CurrentTheme = theme;
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Override this method in derived layouts to perform additional initialization
    /// after common initialization is complete but before breadcrumbs are generated.
    /// </summary>
    protected virtual Task OnInitializedLayoutAsync()
    {
        return Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_needsBreadcrumbUpdate)
        {
            _needsBreadcrumbUpdate = false;

            // Give the page a chance to set its own breadcrumbs (they set in OnInitialized).
            // If page hasn't set any, auto-generate from menu.
            if (!PageLayout.BreadcrumbItems.Any())
            {
                await UpdateBreadcrumbsAsync();
            }

            StateHasChanged();
        }
        else if (firstRender && !PageLayout.BreadcrumbItems.Any())
        {
            // Menu breadcrumbs were empty after layout init; page Title may now be available.
            EnsureMinimumBreadcrumb();
            StateHasChanged();
        }
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        // Only handle actual navigation (not SSR-to-interactive transitions)
        if (_currentUrl != e.Location)
        {
            _currentUrl = e.Location;
            PageLayout.Reset();
            _needsBreadcrumbUpdate = true;
            _ = InvokeAsync(async () =>
            {
                try
                {
                    await Task.Yield();
                    // Public (DB-driven) menus are URL-contextual (e.g. KB category menu depends on the project slug),
                    // so reload menu items on navigation. Contributor-based menus are static and unaffected.
                    // Keep the visible rail when the reload is an anonymous principal miss.
                    await ReloadShellChromeAsync();
                    StateHasChanged();
                }
                catch (ObjectDisposedException)
                {
                }
                catch (OperationCanceledException)
                {
                }
            });
        }
    }

    private void RestoreShellChrome()
    {
        try
        {
            if (!PersistentState.TryTakeFromJson<ShellChromeSnapshot>(ShellMenuContinuity.PersistenceKey, out var snapshot)
                || snapshot == null)
            {
                return;
            }

            var menu = ShellMenuContinuity.RestoreMenu(snapshot.Menu);
            if (menu.Count > 0)
            {
                MenuItems = menu;
            }

            var toolbar = ShellMenuContinuity.RestoreToolbar(snapshot.Toolbar);
            if (toolbar.Count > 0)
            {
                ToolbarItems = toolbar;
            }
        }
        catch (InvalidOperationException)
        {
            // No persisted shell for this render.
        }
    }

    private Task PersistShellChrome()
    {
        if (MenuItems.Count == 0 && ToolbarItems.Count == 0)
        {
            return Task.CompletedTask;
        }

        PersistentState.PersistAsJson(
            ShellMenuContinuity.PersistenceKey,
            ShellMenuContinuity.Capture(MenuItems, ToolbarItems));
        return Task.CompletedTask;
    }

    private async Task ReloadShellChromeAsync()
    {
        var loaded = await LoadMenuItemsAsync();
        if (await ShouldKeepVisibleMenuAsync(loaded.Count))
        {
            return;
        }

        MenuItems = loaded;
        var toolbar = await ToolbarManager.GetAsync(SufiToolbars.Main);
        ToolbarItems = toolbar.Items.ToList();
    }

    private async Task<bool> ShouldKeepVisibleMenuAsync(int loadedCount)
    {
        var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        return ShellMenuContinuity.ShouldKeepVisibleMenu(
            MenuItems.Count,
            loadedCount,
            state.User.Identity?.IsAuthenticated == true,
            MenuUser.IsAuthenticated);
    }

    private void OnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        _ = InvokeAsync(async () =>
        {
            try
            {
                await task;
                await ReloadShellChromeAsync();
                StateHasChanged();
            }
            catch (ObjectDisposedException)
            {
            }
            catch (OperationCanceledException)
            {
            }
        });
    }

    private async Task<List<ApplicationMenuItem>> LoadMenuItemsAsync()
    {
        var publicItems = await PublicMenuProvider.GetMenuItemsAsync(NavigationManager.Uri);
        if (publicItems != null)
        {
            return publicItems;
        }

        var menu = await MenuManager.GetMainMenuAsync();
        return menu.Items.ToList();
    }

    private async Task UpdateBreadcrumbsAsync()
    {
        try
        {
            var breadcrumbs = await BreadcrumbService.GetBreadcrumbsForUrlAsync(NavigationManager.Uri);
            foreach (var item in breadcrumbs)
            {
                PageLayout.BreadcrumbItems.Add(item);
            }

            // Auto-set page title from the last breadcrumb (current page's menu DisplayName)
            // if the page hasn't explicitly set its own title
            if (string.IsNullOrEmpty(PageLayout.Title) && breadcrumbs.Count > 0)
            {
                PageLayout.Title = breadcrumbs[^1].Text;
            }

            // Fallback only when menu-based generation produced nothing
            EnsureMinimumBreadcrumb();
        }
        catch
        {
            EnsureMinimumBreadcrumb();
        }
    }

    /// <summary>
    /// Fallback when menu breadcrumbs and page-defined breadcrumbs are both empty.
    /// Uses the page title only — never a raw URL segment.
    /// </summary>
    private void EnsureMinimumBreadcrumb()
    {
        if (PageLayout.BreadcrumbItems.Any())
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(PageLayout.Title))
        {
            return;
        }

        PageLayout.BreadcrumbItems.Add(new BreadcrumbItem(PageLayout.Title));
    }

    private void OnPageLayoutChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        // Increment version to force child components (like SufiTopBar) to re-render
        // when their RenderFragment parameters would produce different output
        LayoutContentVersion++;
        
        // Re-render when page layout properties change (title, breadcrumbs, toolbar content)
        InvokeAsync(StateHasChanged);
    }

    public virtual void Dispose()
    {
        PageLayout.PropertyChanged -= OnPageLayoutChanged;
        NavigationManager.LocationChanged -= OnLocationChanged;
        ThemeSwitchService.ThemeChanged -= OnThemeChanged;
        if (_authenticationStateSubscribed)
        {
            AuthenticationStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
        }

        _shellPersist?.Dispose();
    }
}
