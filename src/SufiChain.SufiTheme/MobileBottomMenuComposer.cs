using SufiChain.SufiPlatform.UI.Navigation;

namespace SufiChain.SufiTheme;

/// <summary>
/// One destination in the mobile bottom menu.
/// </summary>
public sealed class MobileBottomLink
{
    public MobileBottomLink(string name, string label, bool localizeLabel, string url, string icon)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Label = label ?? throw new ArgumentNullException(nameof(label));
        LocalizeLabel = localizeLabel;
        Url = url ?? throw new ArgumentNullException(nameof(url));
        Icon = string.IsNullOrWhiteSpace(icon) ? "circle" : icon;
    }

    public string Name { get; }

    /// <summary>
    /// Visible name, or a localization key when <see cref="LocalizeLabel"/> is true.
    /// </summary>
    public string Label { get; }

    public bool LocalizeLabel { get; }

    public string Url { get; }

    public string Icon { get; }
}

/// <summary>
/// Builds the phone bottom menu from host shortcuts plus the host's main-menu selector.
/// The icon rail is not copied in.
/// </summary>
public static class MobileBottomMenuComposer
{
    public static IReadOnlyList<MobileBottomLink> Compose(
        IEnumerable<MobileMenuShortcut>? shortcuts,
        IEnumerable<ApplicationMenuItem>? mainItems,
        Func<IEnumerable<MenuItemInfo>, IEnumerable<MenuItemInfo>>? selector)
    {
        var links = new List<MobileBottomLink>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var shortcut in (shortcuts ?? []).OrderBy(item => item.Order))
        {
            TryAdd(links, seen, shortcut.Name, shortcut.DisplayName, localizeLabel: true, shortcut.Url, shortcut.Icon);
        }

        var roots = (mainItems ?? [])
            .Select(item => new MenuItemInfo { MenuItem = item, Level = 0 })
            .ToList();
        IEnumerable<MenuItemInfo> chosen = selector is null ? roots.Take(2) : selector(roots);
        foreach (var info in chosen)
        {
            var item = info?.MenuItem;
            if (item is null)
            {
                continue;
            }

            TryAdd(links, seen, item.Name, item.DisplayName, localizeLabel: false, FirstUrl(item), item.Icon);
        }

        return links;
    }

    private static void TryAdd(
        List<MobileBottomLink> links,
        HashSet<string> seen,
        string name,
        string label,
        bool localizeLabel,
        string? url,
        string? icon)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return;
        }

        var key = MobileShortcutSelection.Normalize(url);
        if (key.Length == 0 || !seen.Add(key))
        {
            return;
        }

        links.Add(new MobileBottomLink(name, label, localizeLabel, url, icon ?? "circle"));
    }

    private static string? FirstUrl(ApplicationMenuItem item)
    {
        if (!string.IsNullOrWhiteSpace(item.Url))
        {
            return item.Url;
        }

        foreach (var child in item.Items)
        {
            var url = FirstUrl(child);
            if (!string.IsNullOrWhiteSpace(url))
            {
                return url;
            }
        }

        return null;
    }
}
