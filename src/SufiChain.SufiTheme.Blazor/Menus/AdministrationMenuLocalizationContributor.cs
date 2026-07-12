using SufiChain.SufiAbp.UI.Localization;
using SufiChain.SufiAbp.UI.Navigation;

namespace SufiChain.SufiTheme.Blazor.Menus;

/// <summary>
/// Sets the Administration menu group display name from the framework localization resource
/// so it appears in the current language (e.g. "مدیریت" for Farsi).
/// </summary>
public class AdministrationMenuLocalizationContributor : IMenuContributor
{
    public Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.Main)
        {
            return Task.CompletedTask;
        }

        var l = context.GetLocalizer<SufiAbpFrameworkResource>();
        var administration = context.Menu.GetAdministration();
        administration.DisplayName = l["Administration"];
        return Task.CompletedTask;
    }
}
