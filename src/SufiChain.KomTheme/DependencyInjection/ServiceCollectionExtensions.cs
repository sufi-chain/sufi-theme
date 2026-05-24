using Microsoft.Extensions.DependencyInjection;
using SufiChain.SufiAbp.UI.Theming;

namespace SufiChain.KomTheme.DependencyInjection;

/// <summary>
/// Extension methods for registering KomTheme services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the KomTheme core services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddKomTheme(this IServiceCollection services)
    {
        return AddKomTheme(services, _ => { });
    }

    /// <summary>
    /// Adds the KomTheme core services to the service collection with configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">Action to configure theme options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddKomTheme(
        this IServiceCollection services,
        Action<KomThemeOptions> configureOptions)
    {
        return AddKomTheme(services, configureOptions, _ => { });
    }

    /// <summary>
    /// Adds the KomTheme core services to the service collection with full configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureThemeOptions">Action to configure theme options.</param>
    /// <param name="configureBlazorOptions">Action to configure Blazor-specific options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddKomTheme(
        this IServiceCollection services,
        Action<KomThemeOptions> configureThemeOptions,
        Action<KomThemeBlazorOptions> configureBlazorOptions)
    {
        // Configure options
        services.Configure(configureThemeOptions);
        services.Configure(configureBlazorOptions);

        // Register the theme
        services.AddTransient<ITheme, Kom1Theme>();
        services.AddTransient<Kom1Theme>();

        return services;
    }
}
