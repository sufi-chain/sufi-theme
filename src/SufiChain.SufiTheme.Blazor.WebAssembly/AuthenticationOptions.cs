namespace SufiChain.SufiTheme.Blazor.WebAssembly;

/// <summary>
/// Configuration options for authentication URLs.
/// </summary>
public class AuthenticationOptions
{
    /// <summary>
    /// The URL to redirect to for login.
    /// </summary>
    public string LoginUrl { get; set; } = "authentication/login";

    /// <summary>
    /// The URL to redirect to for logout.
    /// </summary>
    public string LogoutUrl { get; set; } = "authentication/logout";
}
