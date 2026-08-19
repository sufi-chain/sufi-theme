# SufiTheme Architecture

## Ownership

| Layer | Owns |
| --- | --- |
| SufiBlazor | `Sb*` interactive components, design tokens, theming provider |
| SufiTheme | App shell, layouts, navigation chrome, toolbars |
| Sufi Platform UI | Menu/toolbar contracts, `AccountLayout`, breadcrumbs (`SufiBreadcrumbs`) |

Shell components formerly in SufiBlazor (`SbAppShell`, `SbSidebar`, `SbDualSidebar`, `SbTopBar`, `SbIconRail`, `SbExpandPanel`) were removed. See SufiBlazor [architecture decisions](../sufi-blazor/docs/architecture/decisions.md).

## Layout resolution

Hosts usually resolve layouts in one of these ways:

1. **Options binding** — `SufiThemeBlazorOptions.Layout` set to `SufiLayouts.SideMenu` / `TopMenu` / `DualSidebar`
2. **Zone / route resolver** — host maps account vs admin vs public areas to layout types (common in the development host)
3. **`ITheme.GetLayout`** — theme classes map `StandardLayouts.*` names to components

### Theme classes

| Class | Theme name | Notes |
| --- | --- | --- |
| `SufiBlazorTheme` | `"SufiBlazor"` | Default registration in `ThemingOptions`; Application layout historically tied to SideMenu |
| `Sufi1Theme` | `"Sufi1"` | Respects `SufiThemeBlazorOptions.Layout`; Public maps to the same shell as Application |

Prefer configuring `SufiThemeBlazorOptions.Layout` (and host resolvers) rather than assuming `ITheme` alone drives the shell.

## Public layout

There is **no** separate public layout file. `StandardLayouts.Public` resolves to the same application shell. Differentiate public UX with [IPublicMenuProvider](public-navigation.md).

## Account layout

`AccountLayout` lives in **Sufi Platform UI** (`SufiChain.SufiPlatform.UI.Blazor.Layouts`). SufiTheme registers it as `SufiLayouts.Account`. Hosts may replace it (for example `SufiPlatformAccountLayout` in `hosts/SufiChane.SufiPlatform`).

## Related

- [Layouts](layouts.md)
- [Shell components](shell-components.md)
- [Package map](package-map.md)
- [Configuration](configuration.md)
