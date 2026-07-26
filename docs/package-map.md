# SufiTheme Package Map

Five packages under `independent-projects/sufi-theme/src/`. Versions are pinned in the product `versions.props`.

| Package | Depends on (high level) |
| --- | --- |
| `SufiChain.SufiTheme` | `SufiChain.SufiPlatform.UI.Abstractions` |
| `SufiChain.SufiTheme.Blazor` | Core theme + `SufiPlatform.Core`, `SufiPlatform.UI.Blazor`, `Volo.Abp.Security` |
| `SufiChain.SufiTheme.Blazor.Server` | Theme Blazor + `SufiPlatform.Features`, `SufiPlatform.UI.Blazor.Server`, `Volo.Abp.UI`, `Volo.Abp.MultiTenancy` |
| `SufiChain.SufiTheme.Blazor.WebAssembly` | Theme Blazor + `SufiPlatform.UI.Blazor.WebAssembly` + Bundling |
| `SufiChain.SufiTheme.Blazor.WebAssembly.Bundling` | `SufiPlatform.Core`, `SufiPlatform.UI.Abstractions` |

```
Host (Server or WASM)
  └─ SufiTheme.Blazor.Server  or  SufiTheme.Blazor.WebAssembly
       └─ SufiTheme.Blazor
            ├─ SufiTheme (options, layouts registry)
            └─ SufiPlatform.UI.Blazor (+ Core)
                 └─ SufiBlazor (Sb* components, design tokens)
```

## Server-only adapters (typical)

- Branding adapter bridging ABP / platform branding
- Tenant selector visibility
- Permission-aware toolbar contributions
- Quill / EasyMDE on-demand bundles plus `sufi-theme.css` / `sufi-theme-viewport.js`

## Related

- [Overview](sufi-theme-overview.md)
- [Installation](installation.md)
- [Architecture](architecture.md)
