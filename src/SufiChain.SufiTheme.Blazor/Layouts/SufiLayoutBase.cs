using Microsoft.AspNetCore.Components;
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

    private string? _currentUrl;
    private bool _needsBreadcrumbUpdate;

    protected override async Task OnInitializedAsync()
    {
        
        // Subscribe to PageLayout changes to update UI when page sets title/toolbar
        PageLayout.PropertyChanged += OnPageLayoutChanged;

        // Subscribe to navigation changes to handle breadcrumb/toolbar updates
        _currentUrl = NavigationManager.Uri;
        NavigationManager.LocationChanged += OnLocationChanged;

        // Load menu items (DB-driven public menu when a provider supplies one, else contributor-based main menu)
        MenuItems = await LoadMenuItemsAsync();

        // Load toolbar items
        var toolbar = await ToolbarManager.GetAsync(SufiToolbars.Main);
        ToolbarItems = toolbar.Items.ToList();

        // Detect RTL from current culture
        var culture = System.Globalization.CultureInfo.CurrentUICulture;
        Direction = culture.TextInfo.IsRightToLeft ? SbDirection.Rtl : SbDirection.Ltr;

        // Allow derived layouts to perform additional initialization
        await OnInitializedLayoutAsync();

        // Sync theme from ThemeSwitchService (handles prerender - JS may fail, defaults to Light)
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
            CurrentTheme = ThemeSwitchService.IsDarkMode ? SbTheme.Dark : SbTheme.Light;
        }
        catch
        {
            CurrentTheme = SbTheme.Light;
        }
    }

    private void OnThemeChanged(ThemeMode _)
    {
        CurrentTheme = ThemeSwitchService.IsDarkMode ? SbTheme.Dark : SbTheme.Light;
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
                await Task.Yield();
                // Public (DB-driven) menus are URL-contextual (e.g. KB category menu depends on the project slug),
                // so reload menu items on navigation. Contributor-based menus are static and unaffected.
                MenuItems = await LoadMenuItemsAsync();
                StateHasChanged();
            });
        }
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
    /// Does not replace breadcrumbs already set by the menu service or the page.
    /// </summary>
    private void EnsureMinimumBreadcrumb()
    {
        if (PageLayout.BreadcrumbItems.Any())
        {
            return;
        }

        var text = PageLayout.Title;
        if (string.IsNullOrWhiteSpace(text))
        {
            text = GetTitleFromUrlPath();
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            text = BrandingProvider.AppName;
        }

        PageLayout.BreadcrumbItems.Add(new BreadcrumbItem(text));
    }

    private string GetTitleFromUrlPath()
    {
        var uri = NavigationManager.Uri;
        if (string.IsNullOrEmpty(uri))
        {
            return string.Empty;
        }

        var path = uri.StartsWith('/')
            ? uri.Split('?', '#')[0]
            : (Uri.TryCreate(uri, UriKind.Absolute, out var absolute) ? absolute.AbsolutePath : uri);

        path = path.Trim('/');
        if (string.IsNullOrEmpty(path))
        {
            return string.Empty;
        }

        var segment = path.Split('/', StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
        if (string.IsNullOrEmpty(segment))
        {
            return string.Empty;
        }

        return segment.Replace('-', ' ').Replace('_', ' ');
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
    }
}
