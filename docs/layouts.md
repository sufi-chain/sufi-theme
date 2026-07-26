# SufiTheme Layouts

Layout variants in SufiTheme and when each fits a host.

## Available layouts

| Layout | When to use it |
| --- | --- |
| `SideMenuLayout` | Default application shell with collapsible sidebar, top bar, breadcrumbs, and optional footer |
| `TopMenuLayout` | Smaller navigation surface that lives in the header |
| `DualSidebarLayout` | Two-level navigation: icon rail + expandable panel |
| `AccountLayout` | Authentication and account pages (defined in Sufi Platform UI) |
| `EmptyLayout` | Full-screen, embedded, or print-oriented pages |

Configure the default with `SufiThemeBlazorOptions.Layout = SufiLayouts.SideMenu` (or `TopMenu` / `DualSidebar`). See [Configuration](configuration.md).

## DualSidebar specifics

Controlled by options:

- `IconRailDarkMode`, `IconRailHomeUrl`
- `ExpandOnHover`
- `CollapsedSidebar` / `ShowSidebarToggle`

Shell pieces: `SufiIconRail`, `SufiExpandPanel`, `SufiDualSidebar` — see [Shell components](shell-components.md).

## Account layout (external definition)

`AccountLayout` is **not** defined in the SufiTheme repo. At startup, SufiTheme registers:

```csharp
SufiLayouts.Account = typeof(AccountLayout); // SufiChain.SufiPlatform.UI.Blazor.Layouts
```

Hosts may override account routes. The development host uses `SufiPlatformAccountLayout` for tenant-branded account pages.

## Public layout (same shell as Application)

`StandardLayouts.Public` maps to the same type as Application (`SufiThemeBlazorOptions.Layout`). There is no separate public layout component.

For public-facing **navigation**, use `IPublicMenuProvider`. See [Public navigation](public-navigation.md).

## Common behavior

- Responsive navigation, including mobile header / bottom menu
- Dark mode and RTL through `SbThemeProvider`
- Menu rendering through `IMenuManager`
- Shared layout hooks and toolbar composition

## How to choose

- `SideMenuLayout` — most admin / back-office hosts
- `TopMenuLayout` — simpler products with shallow navigation
- `DualSidebarLayout` — dense module catalogs and dual-level IA
- `AccountLayout` — sign-in, register, recovery
- `EmptyLayout` — pages that must not inherit app chrome

## Related

- [Architecture](architecture.md)
- [Shell components](shell-components.md)
- [Installation](installation.md)
