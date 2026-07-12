using Microsoft.Extensions.DependencyInjection;
using SufiChain.SufiPlatform.UI.Theming;

namespace SufiChain.SufiTheme.DependencyInjection;

/// <summary>
/// Extension methods for registering SufiTheme services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the SufiTheme core services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSufiTheme(this IServiceCollection services)
    {
        return AddSufiTheme(services, _ => { });
    }

    /// <summary>
    /// Adds the SufiTheme core services to the service collection with configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">Action to configure theme options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSufiTheme(
        this IServiceCollection services,
        Action<SufiThemeOptions> configureOptions)
    {
        return AddSufiTheme(services, configureOptions, _ => { });
    }

    /// <summary>
    /// Adds the SufiTheme core services to the service collection with full configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureThemeOptions">Action to configure theme options.</param>
    /// <param name="configureBlazorOptions">Action to configure Blazor-specific options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSufiTheme(
        this IServiceCollection services,
        Action<SufiThemeOptions> configureThemeOptions,
        Action<SufiThemeBlazorOptions> configureBlazorOptions)
    {
        // Configure options
        services.Configure(configureThemeOptions);
        services.Configure(configureBlazorOptions);

        // Register the theme
        services.AddTransient<ITheme, Sufi1Theme>();
        services.AddTransient<Sufi1Theme>();

        return services;
    }
}
