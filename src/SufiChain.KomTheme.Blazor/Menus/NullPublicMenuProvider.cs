using SufiChain.SufiAbp.UI.Navigation;

namespace SufiChain.KomTheme.Blazor.Menus;

/// <summary>
/// Default no-op implementation. Returns null so layouts fall back to <c>IMenuManager</c>.
/// Hosts override this via <c>context.Services.Replace(typeof(IPublicMenuProvider), typeof(...))</c>.
/// </summary>
public class NullPublicMenuProvider : IPublicMenuProvider
{
    public Task<List<ApplicationMenuItem>?> GetMenuItemsAsync(string uri)
    {
        return Task.FromResult<List<ApplicationMenuItem>?>(null);
    }
}
