using Microsoft.AspNetCore.Components;

namespace SufiChain.SufiTheme.Blazor.Components.Public;

public partial class PublicSiteFooter
{
    [Parameter]
    public string Description { get; set; } = string.Empty;

    [Parameter]
    public string Notice { get; set; } = string.Empty;

    [Parameter]
    public string ContactLabel { get; set; } = string.Empty;

    [Parameter]
    public string Release { get; set; } = string.Empty;
}
