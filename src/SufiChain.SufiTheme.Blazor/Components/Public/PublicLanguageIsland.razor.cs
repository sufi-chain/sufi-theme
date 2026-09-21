using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SufiChain.SufiPlatform.UI.Browser;
using SufiChain.SufiPlatform.UI.Localization;

namespace SufiChain.SufiTheme.Blazor.Components.Public;

public partial class PublicLanguageIsland
{
    private IReadOnlyList<LanguageInfo>? _languages;
    private string _currentCulture = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        _languages = await LanguageProvider.GetLanguagesAsync();
        var cultureName = System.Globalization.CultureInfo.CurrentCulture.Name;
        var uiCultureName = System.Globalization.CultureInfo.CurrentUICulture.Name;
        var current = _languages.FirstOrDefault(l =>
                l.CultureName == cultureName || l.UiCultureName == uiCultureName)
            ?? _languages.FirstOrDefault();
        _currentCulture = current?.CultureName ?? string.Empty;
    }

    private async Task OnLanguageChangedAsync(ChangeEventArgs args)
    {
        var cultureName = args.Value?.ToString();
        if (string.IsNullOrWhiteSpace(cultureName) || _languages == null)
        {
            return;
        }

        var language = _languages.FirstOrDefault(item =>
            string.Equals(item.CultureName, cultureName, StringComparison.OrdinalIgnoreCase));
        if (language == null)
        {
            return;
        }

        _currentCulture = language.CultureName;
        await CookieService.SetAsync(
            ".AspNetCore.Culture",
            $"c={language.CultureName}|uic={language.UiCultureName}",
            new CookieOptions { Path = "/", MaxAge = (int)TimeSpan.FromDays(365).TotalSeconds });

        try
        {
            await PreferredLanguageService.SetAsync(language.CultureName);
        }
        catch
        {
            // Cookie still applies after reload.
        }

        await JSRuntime.InvokeVoidAsync("location.reload");
    }
}
