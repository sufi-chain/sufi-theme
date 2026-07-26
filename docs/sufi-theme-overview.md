# SufiTheme Overview

SufiTheme is the preferred shell and layout layer for Sufi Platform hosts. Use it when you need top bars, side navigation, layout variants, branding, and toolbar composition on top of Sufi Platform UI and SufiBlazor.

SufiTheme is not a replacement for SufiBlazor:

- `SufiBlazor` — reusable interactive `Sb*` components (standalone — no ABP required)
- `SufiTheme` — host shell, page layout, and branded navigation frame

## Dependencies

SufiTheme **requires Sufi Platform UI**, not SufiBlazor alone:

- `SufiChain.SufiPlatform.UI.Blazor` — menus, toolbars, page layout, `AccountLayout`
- `SufiChain.SufiPlatform.Core` — module infrastructure
- Additional packages per host shape (Features, MultiTenancy, UI.Blazor.Server / WebAssembly)

Security primitives come from **`Volo.Abp.Security`** on the Blazor/Server packages — there is no `SufiChain.SufiPlatform.Security` package.

Product source: `independent-projects/sufi-theme/` (independently versioned NuGet packages).

## Packages

| Package | Responsibility |
| --- | --- |
| `SufiChain.SufiTheme` | Core options and shared constants (`SufiThemeBlazorOptions`, `SufiLayouts`) |
| `SufiChain.SufiTheme.Blazor` | Layouts, `SufiAppShell`, top bar, sidebar, navigation rendering |
| `SufiChain.SufiTheme.Blazor.Server` | Server toolbar contributors, branding adapters, bundling |
| `SufiChain.SufiTheme.Blazor.WebAssembly` | WASM toolbar contributors and host integration |
| `SufiChain.SufiTheme.Blazor.WebAssembly.Bundling` | WASM style/script contributors |

See [Package map](package-map.md) for the dependency graph.

## What it gives a host

- standard layouts for app, account, and minimal pages
- contributor-driven toolbars
- navigation rendering on top of `IMenuManager` and `IMenuContributor`
- branding through `IBrandingProvider`
- layout hook points for shared host composition
- integration with `SbThemeProvider` for dark mode and RTL

## Where to start

- [Installation](installation.md) — add packages and `DependsOn`
- [Layouts](layouts.md) — choose the shell
- [Configuration](configuration.md) — options and branding
- [Public navigation](public-navigation.md) — public/KB menus
- [Font override](font-override.md) — custom fonts
- [Architecture](architecture.md) — how layout resolution works
