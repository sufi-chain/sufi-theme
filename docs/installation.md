# SufiTheme Installation

## Choose the host package

| Host | Package |
| --- | --- |
| Blazor Server / WebApp | `SufiChain.SufiTheme.Blazor.Server` |
| Blazor WebAssembly | `SufiChain.SufiTheme.Blazor.WebAssembly` (+ Bundling) |

Both pull in `SufiChain.SufiTheme.Blazor` and `SufiChain.SufiTheme`.

## Module registration

```csharp
[DependsOn(typeof(SufiThemeBlazorServerModule))] // or SufiThemeBlazorWebAssemblyModule
public class MyHostModule : SufiModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<SufiThemeBlazorOptions>(options =>
        {
            options.Layout = SufiLayouts.DualSidebar;
        });
    }
}
```

Ensure Sufi Platform UI Blazor packages are already in the host dependency graph (`SufiChain.SufiPlatform.UI.Blazor` and Server/WASM variants as needed).

## Routes / layout binding

Hosts typically bind the active layout from options (or a zone resolver), for example:

```razor
@inject IOptions<SufiThemeBlazorOptions> ThemeOptions
@* Use ThemeOptions.Value.Layout or a host ZoneLayoutResolver *@
```

Account pages often use a host-specific account layout instead of the default `AccountLayout`. See [Layouts](layouts.md).

## Related

- [Configuration](configuration.md)
- [Package map](package-map.md)
- [Overview](sufi-theme-overview.md)
