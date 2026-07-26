# SufiTheme Toolbars

Toolbars are assembled from `IToolbarContributor` implementations. Contributors decide whether to add components to a named toolbar and in what order.

## Default main toolbar

Registered on `SufiToolbars.Main`:

| Component | Typical order | Package |
| --- | --- | --- |
| `ThemeSwitchComponent` | 100 | Server / WASM theme packages |
| `LanguageSwitchComponent` | 200 | Server / WASM theme packages |

**User account is not a top-toolbar item.** User affordances live in the sidebar / DualSidebar rail footer (`SidebarUserAvatar`).

## Page toolbar

Modules set page actions via `IPageLayout.ToolbarContent`. SufiTheme renders that content in `SufiTopBar` when `ShowPageToolbar` is true.

## Custom contributors

Implement `IToolbarContributor`:

1. Check the toolbar name (usually `SufiToolbars.Main`)
2. Add your component through the toolbar context
3. Pick an order that keeps the top bar predictable

Use custom contributors for permission-aware actions, tenant shortcuts, or Server vs WASM differences.

## Rich-text font contributor

`FontFamilyToolbarContributor` (registered on the shared Blazor theme module) adds Persian font-family options to the rich-text editor toolbar when the current culture is RTL. See also [Font Override](font-override.md).

## Related

- [Configuration](configuration.md)
- [Shell components](shell-components.md)
- [Layouts](layouts.md)
