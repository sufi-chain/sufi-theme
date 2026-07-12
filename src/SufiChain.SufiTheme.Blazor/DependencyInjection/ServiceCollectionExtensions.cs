using Microsoft.Extensions.DependencyInjection;
using SufiChain.SufiTheme.Blazor.Layouts;
using SufiChain.SufiAbp.UI.Blazor.Layouts;
using SufiChain.SufiTheme.DependencyInjection;
using SufiChain.SufiTheme.Blazor.Menus;

namespace SufiChain.SufiTheme.Blazor.DependencyInjection;

/// <summary>
/// Extension methods for registering SufiTheme Blazor services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the SufiTheme Blazor services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSufiThemeBlazor(this IServiceCollection services)
    {
        return AddSufiThemeBlazor(services, _ => { }, _ => { });
    }

    /// <summary>
    /// Adds the SufiTheme Blazor services with Blazor options configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureBlazorOptions">Action to configure Blazor-specific options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSufiThemeBlazor(
        this IServiceCollection services,
        Action<SufiThemeBlazorOptions> configureBlazorOptions)
    {
        return AddSufiThemeBlazor(services, _ => { }, configureBlazorOptions);
    }

    /// <summary>
    /// Adds the SufiTheme Blazor services with full configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureThemeOptions">Action to configure theme options.</param>
    /// <param name="configureBlazorOptions">Action to configure Blazor-specific options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSufiThemeBlazor(
        this IServiceCollection services,
        Action<SufiThemeOptions> configureThemeOptions,
        Action<SufiThemeBlazorOptions> configureBlazorOptions)
    {
        // Register layout types
        SufiLayouts.SideMenu = typeof(SideMenuLayout);
        SufiLayouts.TopMenu = typeof(TopMenuLayout);
        SufiLayouts.DualSidebar = typeof(DualSidebarLayout);
        SufiLayouts.Account = typeof(AccountLayout);
        SufiLayouts.Empty = typeof(EmptyLayout);

        // Add core theme services
        services.AddSufiTheme(configureThemeOptions, configureBlazorOptions);

        // Default public-menu provider (no-op). Hosts replace this to supply
        // database-driven menus for landing/KB layout zones.
        services.AddSingleton<IPublicMenuProvider, NullPublicMenuProvider>();

        return services;
    }
}
