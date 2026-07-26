# SufiTheme

![SufiTheme](docs/assets/sufi-theme-baner.png)

[![GitHub release](https://img.shields.io/github/v/release/sufi-chain/sufi-theme?include_prereleases&sort=semver)](https://github.com/sufi-chain/sufi-theme/releases/latest)
[![License: LGPL-3.0](https://img.shields.io/github/license/sufi-chain/sufi-theme)](LICENSE)
[![GitHub stars](https://img.shields.io/github/stars/sufi-chain/sufi-theme)](https://github.com/sufi-chain/sufi-theme/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/sufi-chain/sufi-theme)](https://github.com/sufi-chain/sufi-theme/network/members)
[![GitHub issues](https://img.shields.io/github/issues/sufi-chain/sufi-theme)](https://github.com/sufi-chain/sufi-theme/issues)
[![GitHub last commit](https://img.shields.io/github/last-commit/sufi-chain/sufi-theme)](https://github.com/sufi-chain/sufi-theme/commits)

Official Blazor theme and application shell for **Sufi Platform**. SufiTheme owns layouts, navigation chrome, and toolbars. Interactive `Sb*` controls come from **[SufiBlazor](../sufi-blazor/)**.

| | |
| --- | --- |
| License | LGPL-3.0 |
| Target | .NET 10 |
| Packages | `SufiChain.SufiTheme*` (5 packages) |
| Docs | [`docs/README.md`](docs/README.md) |

## Quick start

1. Reference `SufiChain.SufiTheme.Blazor.Server` (or `.WebAssembly`) from the host.
2. `[DependsOn(typeof(SufiThemeBlazorServerModule))]`.
3. Configure the layout:

```csharp
Configure<SufiThemeBlazorOptions>(options =>
{
    options.Layout = SufiLayouts.DualSidebar;
});
```

Full steps: [docs/installation.md](docs/installation.md).

## Packages

| Package | Role |
| --- | --- |
| `SufiChain.SufiTheme` | Options and layout registry |
| `SufiChain.SufiTheme.Blazor` | Layouts and shell components |
| `SufiChain.SufiTheme.Blazor.Server` | Server host integration |
| `SufiChain.SufiTheme.Blazor.WebAssembly` | WASM host integration |
| `SufiChain.SufiTheme.Blazor.WebAssembly.Bundling` | WASM asset contributors |

Dependencies are **`SufiChain.SufiPlatform.UI.*`** (and related platform packages), not a SufiBlazor-only stack. See [docs/package-map.md](docs/package-map.md).

## Layouts

Application shells: `SideMenuLayout`, `TopMenuLayout`, `DualSidebarLayout`. Account uses platform `AccountLayout`. Public routes reuse the application shell and supply menus via `IPublicMenuProvider`. Details: [docs/layouts.md](docs/layouts.md).

## Related

- [SufiBlazor](../sufi-blazor/) — component library
- [Sufi Platform docs](../../sufi-platform/docs/)
