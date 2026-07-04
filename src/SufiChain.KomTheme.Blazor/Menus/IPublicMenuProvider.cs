using SufiChain.SufiAbp.UI.Navigation;

namespace SufiChain.KomTheme.Blazor.Menus;

/// <summary>
/// Resolves dynamic, database-driven menu items for public layout zones
/// (e.g. the landing top-nav and the KB side-nav). Returns null to indicate
/// that the caller should fall back to the contributor-based <c>IMenuManager</c>.
/// </summary>
public interface IPublicMenuProvider
{
    Task<List<ApplicationMenuItem>?> GetMenuItemsAsync(string uri);
}
