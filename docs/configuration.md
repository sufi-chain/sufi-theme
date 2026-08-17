# SufiTheme Configuration

Host-level knobs for SufiTheme. Configure in the host module with `Configure<SufiThemeBlazorOptions>(...)`.

## `SufiThemeBlazorOptions`

| Option | Type | Default | Purpose |
| --- | --- | --- | --- |
| `Layout` | `Type` | `SufiLayouts.SideMenu` | Default application layout component (`SideMenu`, `TopMenu`, `DualSidebar`, `Mobile`, or custom) |
| `MobileMenuSelector` | `Func<…>` | First 2 items | Filters/transforms main menu for mobile bottom nav |
| `ShowSidebarToggle` | `bool` | `true` | Sidebar collapse button in the main header |
| `CollapsedSidebar` | `bool` | `false` | Sidebar collapsed by default on desktop |
| `ShowBreadcrumbs` | `bool` | `true` | Breadcrumbs in the content area |
| `ShowPageToolbar` | `bool` | `true` | Page title/actions area |
| `ShowFooter` | `bool` | `true` | Theme footer |
| `CopyrightText` | `string` | `"Copyright © SufiChain"` | Footer copyright (also overridable via branding) |
| `IconRailDarkMode` | `bool` | `true` | Dark styling for DualSidebar icon rail |
| `IconRailHomeUrl` | `string` | `"/"` | URL for DualSidebar rail logo click |
| `ExpandOnHover` | `bool` | `true` | DualSidebar expand panel opens on hover |
| `MobileShortcuts` | `List<MobileMenuShortcut>` | empty | Explicit mobile bottom-menu shortcuts (no hardcoded items) |

### Example

```csharp
Configure<SufiThemeBlazorOptions>(options =>
{
    options.Layout = SufiLayouts.DualSidebar;
    options.ShowBreadcrumbs = true;
    options.ExpandOnHover = true;
    options.IconRailHomeUrl = "/panel/admin";
    options.MobileMenuSelector = items => items.Take(3);
    options.MobileShortcuts.Add(new MobileMenuShortcut(
        "Home", "Home", "/", "si-home", order: 0));
});
```

`SufiLayouts` exposes registered layout types: `SideMenu`, `TopMenu`, `DualSidebar`, `Account`, `Empty`, and `Mobile`.

## `SufiThemeOptions` (style registry)

| Style | Constant |
| --- | --- |
| Light | `SufiStyleNames.Light` |
| Dark | `SufiStyleNames.Dark` |
| Dim | `SufiStyleNames.Dim` |
| System | `SufiStyleNames.System` |

Used by the theme-switch toolbar component.

## Branding

Implement `IBrandingProvider` for app name, logos, favicon, and copyright. Tenant-aware hosts resolve the current tenant inside the provider.

## Layout hooks

Use `LayoutHookOptions` to inject shared components into hook points such as `LayoutHooks.Body.First` and `LayoutHooks.Body.Last` without forking the layout.

## CSS and fonts

Override theme variables in host CSS after loading `sufi-theme.css`. For Latin/RTL font overrides, see [Font Override](font-override.md).

## Related

- [Layouts](layouts.md)
- [Toolbars](toolbars.md)
- [Installation](installation.md)
- [Architecture](architecture.md)
