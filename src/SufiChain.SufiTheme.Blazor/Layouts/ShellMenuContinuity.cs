using SufiChain.SufiPlatform.UI.Navigation;
using SufiChain.SufiPlatform.UI.Toolbars;

namespace SufiChain.SufiTheme.Blazor.Layouts;

/// <summary>
/// Keeps the prerendered shell rail visible across the interactive circuit's first paint.
/// A full navigation shows the server-rendered icons, then the new circuit renders the
/// layout with an empty menu until <c>OnInitializedAsync</c> finishes. That gap is the
/// rail collapse (app-grid and avatar only). The snapshot is applied before the first
/// await so the first interactive render still has the icons.
/// An empty reload is ignored when Blazor already has an authenticated user but the
/// menu filter's principal accessor does not. That read is a flow miss, not an empty menu.
/// </summary>
public static class ShellMenuContinuity
{
    public const string PersistenceKey = "sufi-shell-chrome";

    public static bool ShouldKeepVisibleMenu(
        int visibleCount,
        int loadedCount,
        bool authenticationStateAuthenticated,
        bool menuAccessorAuthenticated)
    {
        return visibleCount > 0
            && loadedCount == 0
            && authenticationStateAuthenticated
            && !menuAccessorAuthenticated;
    }

    public static ShellChromeSnapshot Capture(
        IEnumerable<ApplicationMenuItem> menuItems,
        IEnumerable<ToolbarItem> toolbarItems)
    {
        return new ShellChromeSnapshot
        {
            Menu = menuItems.Select(CaptureItem).ToList(),
            Toolbar = toolbarItems.Select(item => new ShellToolbarNode
            {
                ComponentTypeName = item.ComponentType.AssemblyQualifiedName ?? string.Empty,
                Order = item.Order
            }).ToList()
        };
    }

    public static List<ApplicationMenuItem> RestoreMenu(IEnumerable<ShellMenuNode>? nodes)
    {
        return (nodes ?? []).Select(RestoreItem).ToList();
    }

    public static List<ToolbarItem> RestoreToolbar(IEnumerable<ShellToolbarNode>? nodes)
    {
        var items = new List<ToolbarItem>();
        foreach (var node in nodes ?? [])
        {
            if (string.IsNullOrWhiteSpace(node.ComponentTypeName))
            {
                continue;
            }

            var type = Type.GetType(node.ComponentTypeName, throwOnError: false);
            if (type == null)
            {
                continue;
            }

            items.Add(new ToolbarItem(type, node.Order));
        }

        return items;
    }

    private static ShellMenuNode CaptureItem(ApplicationMenuItem item)
    {
        return new ShellMenuNode
        {
            Name = item.Name,
            DisplayName = item.DisplayName,
            Url = item.Url,
            Icon = item.Icon,
            Order = item.Order,
            Target = item.Target,
            IsCollapsed = item.IsCollapsed,
            Items = item.Items.Select(CaptureItem).ToList()
        };
    }

    private static ApplicationMenuItem RestoreItem(ShellMenuNode node)
    {
        var item = new ApplicationMenuItem(
            node.Name,
            node.DisplayName,
            node.Url,
            node.Icon,
            node.Order,
            node.Target)
        {
            IsCollapsed = node.IsCollapsed
        };

        foreach (var child in node.Items)
        {
            item.AddItem(RestoreItem(child));
        }

        return item;
    }
}

public sealed class ShellChromeSnapshot
{
    public List<ShellMenuNode> Menu { get; set; } = [];

    public List<ShellToolbarNode> Toolbar { get; set; } = [];
}

public sealed class ShellMenuNode
{
    public string Name { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string? Url { get; set; }

    public string? Icon { get; set; }

    public int Order { get; set; }

    public string? Target { get; set; }

    public bool IsCollapsed { get; set; } = true;

    public List<ShellMenuNode> Items { get; set; } = [];
}

public sealed class ShellToolbarNode
{
    public string ComponentTypeName { get; set; } = string.Empty;

    public int Order { get; set; }
}
