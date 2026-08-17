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
