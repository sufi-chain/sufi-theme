using Microsoft.AspNetCore.Components.Routing;
using SufiChain.SufiTheme;

namespace SufiChain.SufiTheme.Blazor.Layouts;

public partial class MobileLayout
{
    private bool _menuOpen;
    private bool _actionsOpen;

    private IReadOnlyList<MobileBottomLink> BottomLinks =>
        MobileBottomMenuComposer.Compose(
            BlazorOptions.Value.MobileShortcuts,
            MenuItems,
            BlazorOptions.Value.MobileMenuSelector);

    private string CurrentPath
    {
        get
        {
            try
            {
                return new Uri(NavigationManager.Uri).AbsolutePath;
            }
            catch (UriFormatException)
            {
                return string.Empty;
            }
        }
    }

    private Task NavigateShortcut(string? url)
    {
        if (!string.IsNullOrWhiteSpace(url))
        {
            // A plain anchor is an enhanced document load. That updates the URL and
            // keeps the previous interactive view, so Home can leave Calendar on screen.
            NavigationManager.NavigateTo(url);
        }

        return Task.CompletedTask;
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
