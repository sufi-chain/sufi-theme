using Microsoft.AspNetCore.Components.Routing;
using SufiChain.SufiTheme;

namespace SufiChain.SufiTheme.Blazor.Layouts;

public partial class MobileLayout
{
    private bool _menuOpen;
    private bool _actionsOpen;

    private IReadOnlyList<MobileMenuShortcut> MobileShortcuts =>
        BlazorOptions.Value.MobileShortcuts
            .OrderBy(shortcut => shortcut.Order)
            .Take(4)
            .ToList();

    private bool IsShortcutCurrent(MobileMenuShortcut shortcut)
    {
        string path;
        try
        {
            path = new Uri(NavigationManager.Uri).AbsolutePath;
        }
        catch (UriFormatException)
        {
            path = string.Empty;
        }

        return MobileShortcutSelection.IsCurrent(shortcut.Url, path);
    }

    private string ShortcutClass(MobileMenuShortcut shortcut) =>
        IsShortcutCurrent(shortcut)
            ? "sufi-mobile-bottom-menu__shortcut sufi-mobile-bottom-menu__shortcut--active"
            : "sufi-mobile-bottom-menu__shortcut";

    private void NavigateShortcut(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return;
        }

        // A plain anchor is an enhanced document load. That updates the URL and
        // keeps the previous interactive view, so Home can leave Calendar on screen.
        NavigationManager.NavigateTo(url);
    }

    protected override Task OnInitializedLayoutAsync()
    {
        NavigationManager.LocationChanged += CloseDrawersOnNavigate;
        return Task.CompletedTask;
    }

    private void OpenMenu()
    {
        _menuOpen = true;
    }

    private void OpenActions()
    {
        _actionsOpen = true;
    }

    private void CloseDrawersOnNavigate(object? sender, LocationChangedEventArgs args)
    {
        _menuOpen = false;
        _actionsOpen = false;
        _ = InvokeAsync(StateHasChanged);
    }

    public override void Dispose()
    {
        NavigationManager.LocationChanged -= CloseDrawersOnNavigate;
        base.Dispose();
    }
}
