using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using SufiChain.SufiBlazor.Contracts.Editors;

namespace SufiChain.SufiTheme.Blazor.Toolbar;

/// <summary>
/// Contributes a font-family dropdown to the rich text editor toolbar.
/// Provides the four Persian fonts (Dirooz FD, Samim FD, Gandom FD, Sahel FD)
/// defined in sufi-theme.css so users can override the default font for selected content.
/// Only visible when the culture is RTL (e.g. Farsi, Arabic), since these fonts are for RTL scripts.
/// </summary>
public class FontFamilyToolbarContributor : IRteToolbarContributor
{
    /// <summary>
    /// Items appear after default formatting (bold, italic, etc.).
    /// </summary>
    public int Order => 105;

    public Task ConfigureToolbarAsync(RteToolbarContext context)
    {
        context.Items.Add(new RteToolbarContributedItem
        {
            Id = "font",
            Group = "formatting",
            Order = 5, // After bold, italic, underline, strike
            Type = SbEditorToolbarItemType.Select,
            Format = "font",
            Tooltip = "Font",
            IsVisible = () => CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft,
            Options = new List<SbEditorToolbarOption>
            {
                new() { Label = "Default", LabelKey = "Rte:FontDefault", Value = false },
                new() { Label = "Dirooz FD", Value = "dirooz-fd" },
                new() { Label = "Samim FD", Value = "samim-fd" },
                new() { Label = "Gandom FD", Value = "gandom-fd" },
                new() { Label = "Sahel FD", Value = "sahel-fd" },
            }
        });

        return Task.CompletedTask;
    }
}
