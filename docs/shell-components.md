# SufiTheme Shell Components

Chrome components under `SufiChain.SufiTheme.Blazor`. Platform-owned pieces are noted separately.

## Layouts

| Component | Role |
| --- | --- |
| `SideMenuLayout` | Classic sidebar + top bar shell |
| `TopMenuLayout` | Header-first navigation |
| `DualSidebarLayout` | Icon rail + expandable panel |
| `MobileLayout` | Full-screen mobile workspace with bottom shortcuts and drawer navigation |
| `EmptyLayout` | Minimal wrapper (no chrome) |
| `SufiLayoutBase` | Shared layout base |

Account: `AccountLayout` from Sufi Platform UI (registered as `SufiLayouts.Account`).

## Shell / chrome

| Component | Role |
| --- | --- |
| `SufiAppShell` | Outer shell composition |
| `SufiSidebar` | Primary sidebar |
| `SufiDualSidebar` | Dual-sidebar composition |
| `SufiIconRail` | DualSidebar icon rail |
| `SufiExpandPanel` | DualSidebar expandable panel |
| `SufiTopBar` | Main top bar (toolbars, page toolbar slot) |
| `SufiFooterCopyright` | Footer copyright |
| `MobileHeader` | Mobile header |
| `MobileBottomMenu` | Mobile bottom navigation |
| `MobileTopBarLogo` | Mobile logo slot |

## Navigation & user

| Component | Role |
| --- | --- |
| `MenuItemRenderer` | Renders menu trees |
| `IconRailNavItem` | Rail nav item |
| `SidebarUserAvatar` | User affordance in sidebar/rail footer (not a top-toolbar user menu) |
| `SidebarUserProfile` | Profile section in sidebar |

## Host wiring helpers

`Routes`, `App`, `RedirectToLogin` (and WASM auth pages in the WebAssembly package).

## Platform-owned (not in this repo)

- `SufiBreadcrumbs` — rendered when `ShowBreadcrumbs` is true
- `AccountLayout` — account/auth pages

## Related

- [Layouts](layouts.md)
- [Toolbars](toolbars.md)
- [Architecture](architecture.md)
