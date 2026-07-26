# SufiTheme Public Navigation

SufiTheme does **not** ship a separate public layout component. `StandardLayouts.Public` resolves to the same shell as `StandardLayouts.Application` (see [Layouts](layouts.md)).

Public-facing products differentiate UX through **navigation data**, not a different layout file.

## IPublicMenuProvider

| Implementation | Behavior |
| --- | --- |
| `NullPublicMenuProvider` | Default — empty menu |
| Host replacement | `Replace(ServiceDescriptor…)` with a host provider |

Registration in `SufiThemeBlazorModule`:

```csharp
services.AddSingleton<IPublicMenuProvider, NullPublicMenuProvider>();
```

## Development host example

`.dev/hosts/SufiChane.SufiPlatform` registers `SufiPlatformPublicMenuProvider` in `SufiPlatformModule`:

```csharp
context.Services.Replace(
    ServiceDescriptor.Scoped<IPublicMenuProvider, SufiPlatformPublicMenuProvider>());
```

That provider loads public/KB menu items while admin navigation continues to use module `IMenuContributor` entries.

## When to use Public vs Application

Use `StandardLayouts.Public` when a page should be tagged public for layout resolution — it still gets the same `SideMenu` / `TopMenu` / `DualSidebar` component from `SufiThemeBlazorOptions.Layout`.

Combine with:

- `IPublicMenuProvider` for public nav content
- Host zone/routing logic for account vs app vs public areas

## Related

- [Layouts](layouts.md)
- [Configuration](configuration.md)
- [Architecture](architecture.md)
- SufiBlazor shell ownership: [architecture decisions](../sufi-blazor/docs/architecture/decisions.md)
