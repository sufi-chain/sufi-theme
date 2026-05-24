using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using SufiChain.SufiBlazor.Components;
using SufiChain.SufiBlazor.Theming;
using SufiChain.SufiAbp.UI.Branding;
using SufiChain.SufiAbp.UI.Layout;
using SufiChain.SufiAbp.UI.Localization;
using SufiChain.SufiAbp.UI.Navigation;
using SufiChain.SufiAbp.UI.Theming;
using SufiChain.SufiAbp.UI.Toolbars;

namespace SufiChain.KomTheme.Blazor.Layouts;

/// <summary>
/// Base class for KomTheme layouts containing shared functionality for:
/// - Menu and toolbar loading
/// - Breadcrumb management
/// - Navigation change handling
/// - Page layout property change handling
/// - RTL detection
/// </summary>
public abstract class KomLayoutBase : LayoutComponentBase, IDisposable
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
    protected IOptions<KomThemeBlazorOptions> BlazorOptions { get; set; } = default!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    protected IThemeSwitchService ThemeSwitchService { get; set; } = default!;

    [Inject]
    protected IStringLocalizer<SufiAbpFrameworkResource> L { get; set; } = default!;

    // Common state fields
    protected SbTheme CurrentTheme { get; set; } = SbTheme.Light;
    protected SbDirection Direction { get; set; } = SbDirection.Ltr;
    protected List<ApplicationMenuItem> MenuItems { get; set; } = new();
    protected List<ToolbarItem> ToolbarItems { get; set; } = new();
    
    /// <summary>
    /// Version counter that increments when page layout properties change.
    /// Used to force child components (like KomTopBar) to re-render when
    /// their RenderFragment parameters would produce different output.
    /// </summary>
    protected int LayoutContentVersion { get; private set; }

    /// <summary>
    /// Copyright text for the footer. Uses branding provider if set, otherwise KomTheme default ("Copyright © SufiChain").
    /// Hosts can override via IBrandingProvider.CopyrightText or KomThemeBlazorOptions.CopyrightText.
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

        // Load menu items
        var menu = await MenuManager.GetMainMenuAsync();
        MenuItems = menu.Items.ToList();

        // Load toolbar items
        var toolbar = await ToolbarManager.GetAsync(KomToolbars.Main);
        ToolbarItems = toolbar.Items.ToList();

        // Detect RTL from current culture
        var culture = System.Globalization.CultureInfo.CurrentUICulture;
        Direction = culture.TextInfo.IsRightToLeft ? SbDirection.Rtl : SbDirection.Ltr;

        // Allow derived layouts to perform additional initialization
        await OnInitializedLayoutAsync();

        // Sync theme from ThemeSwitchService (handles prerender - JS may fail, defaults to Light)
        await SyncThemeFromServiceAsync();
        ThemeSwitchService.ThemeChanged += OnThemeChanged;

        // Generate initial breadcrumbs
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
        // After page content renders, check if we need to update the layout
        if (_needsBreadcrumbUpdate)
        {
            _needsBreadcrumbUpdate = false;

            // Give the page a chance to set its own breadcrumbs (they set in OnInitialized)
            // If page hasn't set any, auto-generate from menu
            if (!PageLayout.BreadcrumbItems.Any())
            {
                await UpdateBreadcrumbsAsync();
            }
            
            // Always re-render after navigation to pick up the new page's title/toolbar
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
                StateHasChanged();
            });
        }
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
        }
        catch
        {
            // Ignore errors in breadcrumb generation
        }
    }

    private void OnPageLayoutChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        // Increment version to force child components (like KomTopBar) to re-render
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
