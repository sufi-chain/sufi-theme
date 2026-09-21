namespace SufiChain.SufiTheme.Blazor.Layouts;

public partial class TopMenuLayout
{
    private string CurrentPath
    {
        get
        {
            if (!Uri.TryCreate(NavigationManager.Uri, UriKind.Absolute, out var uri))
            {
                return "/";
            }

            return string.IsNullOrWhiteSpace(uri.AbsolutePath) ? "/" : uri.AbsolutePath;
        }
    }
}
