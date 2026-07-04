using Microsoft.Extensions.DependencyInjection;
using SufiChain.KomTheme.Blazor.Layouts;
using SufiChain.SufiAbp.UI.Blazor.Layouts;
using SufiChain.KomTheme.DependencyInjection;
using SufiChain.KomTheme.Blazor.Menus;

namespace SufiChain.KomTheme.Blazor.DependencyInjection;

/// <summary>
/// Extension methods for registering KomTheme Blazor services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the KomTheme Blazor services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddKomThemeBlazor(this IServiceCollection services)
    {
        return AddKomThemeBlazor(services, _ => { }, _ => { });
    }

    /// <summary>
    /// Adds the KomTheme Blazor services with Blazor options configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureBlazorOptions">Action to configure Blazor-specific options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddKomThemeBlazor(
        this IServiceCollection services,
        Action<KomThemeBlazorOptions> configureBlazorOptions)
    {
        return AddKomThemeBlazor(services, _ => { }, configureBlazorOptions);
    }

    /// <summary>
    /// Adds the KomTheme Blazor services with full configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureThemeOptions">Action to configure theme options.</param>
    /// <param name="configureBlazorOptions">Action to configure Blazor-specific options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddKomThemeBlazor(
        this IServiceCollection services,
        Action<KomThemeOptions> configureThemeOptions,
        Action<KomThemeBlazorOptions> configureBlazorOptions)
    {
        // Register layout types
        KomLayouts.SideMenu = typeof(SideMenuLayout);
        KomLayouts.TopMenu = typeof(TopMenuLayout);
        KomLayouts.DualSidebar = typeof(DualSidebarLayout);
        KomLayouts.Account = typeof(AccountLayout);
        KomLayouts.Empty = typeof(EmptyLayout);

        // Add core theme services
        services.AddKomTheme(configureThemeOptions, configureBlazorOptions);

        // Default public-menu provider (no-op). Hosts replace this to supply
        // database-driven menus for landing/KB layout zones.
        services.AddSingleton<IPublicMenuProvider, NullPublicMenuProvider>();

        return services;
    }
}
