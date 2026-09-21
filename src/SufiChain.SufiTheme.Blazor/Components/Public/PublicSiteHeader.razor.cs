using Microsoft.AspNetCore.Components;
using SufiChain.SufiPlatform.UI.Navigation;

namespace SufiChain.SufiTheme.Blazor.Components.Public;

public partial class PublicSiteHeader
{
    [Parameter]
    public IReadOnlyList<ApplicationMenuItem> MenuItems { get; set; } = [];

    [Parameter]
    public string? CurrentPath { get; set; }

    private string ActiveClass(string? url)
    {
        if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(CurrentPath))
        {
            return string.Empty;
        }

        if (url.StartsWith("#", StringComparison.Ordinal) || url.Contains("#", StringComparison.Ordinal))
        {
            return string.Empty;
        }

        var path = url.Split('#', '?')[0];
        return string.Equals(path, CurrentPath, StringComparison.OrdinalIgnoreCase)
            ? "is-active"
            : string.Empty;
    }
}
