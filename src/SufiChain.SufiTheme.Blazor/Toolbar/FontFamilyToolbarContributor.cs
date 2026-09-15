using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using SufiChain.SufiBlazor.Contracts.Editors;

namespace SufiChain.SufiTheme.Blazor.Toolbar;

/// <summary>
/// Contributes a font-family dropdown to the unified editor toolbar.
/// Visible for RTL cultures so users can apply Dirooz, Samim, Gandom, or Sahel FD.
/// </summary>
public class FontFamilyToolbarContributor : IEditorToolbarContributor
{
    public int Order => 105;
    public SbEditorSurface Surfaces => SbEditorSurface.Toolbar;

    public Task ConfigureAsync(EditorToolbarContext context)
    {
        context.Items.Add(new EditorToolbarItem
        {
            Id = "font",
            Group = "formatting",
            Order = 5,
            Type = SbEditorToolbarItemType.Select,
            Tooltip = "Font",
            LabelKey = "Editor:Font",
            IsVisible = _ => CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft,
            Options = new List<SbEditorToolbarOption>
            {
                new() { Label = "Default", LabelKey = "Rte:FontDefault", Value = false },
                new() { Label = "Dirooz FD", Value = "dirooz-fd" },
                new() { Label = "Samim FD", Value = "samim-fd" },
                new() { Label = "Gandom FD", Value = "gandom-fd" },
                new() { Label = "Sahel FD", Value = "sahel-fd" },
            },
            OnClickAsync = async action =>
            {
                await action.Document.ApplyMarkAsync("textStyle", new Dictionary<string, object?>
                {
                    ["fontFamily"] = action.State?.ActiveMarkAttrs.GetValueOrDefault("fontFamily")
                });
            }
        });

        return Task.CompletedTask;
    }
}
