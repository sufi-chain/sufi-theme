namespace SufiChain.SufiTheme;

/// <summary>
/// Matches a mobile bottom-menu shortcut to the current route.
/// </summary>
public static class MobileShortcutSelection
{
    public static bool IsCurrent(string? shortcutUrl, string? currentPath)
    {
        var target = Normalize(shortcutUrl);
        var current = Normalize(currentPath);
        if (target.Length == 0 || current.Length == 0)
        {
            return false;
        }

        return string.Equals(current, target, StringComparison.OrdinalIgnoreCase);
    }

    public static string Normalize(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return string.Empty;
        }

        var value = path.Trim();
        var cut = value.IndexOfAny(['?', '#']);
        if (cut >= 0)
        {
            value = value[..cut];
        }

        if (value.Length == 0)
        {
            return string.Empty;
        }

        if (!value.StartsWith('/'))
        {
            value = "/" + value;
        }

        return value.Length > 1 ? value.TrimEnd('/') : value;
    }
}
