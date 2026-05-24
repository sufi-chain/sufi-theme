using System.Threading.Tasks;
using SufiChain.KomTheme.Blazor.WebAssembly.Themes.KomTheme.Toolbar;
using SufiChain.SufiAbp.UI.Toolbars;

namespace SufiChain.KomTheme.Blazor.WebAssembly.Toolbars;

/// <summary>
/// WebAssembly-specific toolbar contributor for KomTheme.
/// Adds theme switching and language selection to the main toolbar.
/// User account is handled by the User Avatar at the bottom of the icon rail (SidebarUserAvatar).
/// </summary>
public class KomThemeBlazorWebAssemblyToolbarContributor : IToolbarContributor
{
    public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        // Use KomToolbars.Main to match DualSidebarLayout which requests this toolbar name
        if (context.Toolbar.Name == KomToolbars.Main)
        {
            // Add theme switcher
            context.Toolbar.Items.Add(new ToolbarItem(typeof(ThemeSwitchComponent), order: 100));
            
            // Add language switcher
            context.Toolbar.Items.Add(new ToolbarItem(typeof(LanguageSwitchComponent), order: 200));
        }

        return Task.CompletedTask;
    }
}
