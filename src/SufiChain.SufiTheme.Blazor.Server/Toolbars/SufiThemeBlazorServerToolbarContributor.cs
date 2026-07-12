using System.Threading.Tasks;
using SufiChain.SufiTheme.Blazor.Server.Themes.SufiTheme.Toolbar;
using SufiChain.SufiPlatform.UI.Toolbars;

namespace SufiChain.SufiTheme.Blazor.Server.Toolbars;

/// <summary>
/// Server-specific toolbar contributor for SufiTheme.
/// Adds theme switching and language selection to the main toolbar.
/// User account is handled by the User Avatar at the bottom of the icon rail (SidebarUserAvatar).
/// </summary>
public class SufiThemeBlazorServerToolbarContributor : IToolbarContributor
{
    public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        // Use SufiToolbars.Main to match DualSidebarLayout which requests this toolbar name
        if (context.Toolbar.Name == SufiToolbars.Main)
        {
            // Add theme switcher
            context.Toolbar.Items.Add(new ToolbarItem(typeof(ThemeSwitchComponent), order: 100));
            
            // Add language switcher
            context.Toolbar.Items.Add(new ToolbarItem(typeof(LanguageSwitchComponent), order: 200));
        }

        return Task.CompletedTask;
    }
}
