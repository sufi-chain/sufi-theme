# SufiTheme v1.0.0-alpha.1.0 Release Notes

**Release Date:** 2025-05-24  
**Status:** Alpha Release  
**Target Framework:** .NET 10.0

---

## 🎉 First Release

This is the **first alpha release** of SufiTheme, the official theme system for Sufi Platform. SufiTheme provides a modern, dual-layout Blazor theme with comprehensive support for both collapsed and expanded navigation shells, LTR/RTL layouts, and seamless integration with SufiAbp framework.

---

## 📦 Packages

This release includes **5 NuGet packages**:

1. **SufiChain.SufiTheme** `v1.0.0-alpha.1.0`
   - Core theme options, layouts, and style definitions
   
2. **SufiChain.SufiTheme.Blazor** `v1.0.0-alpha.1.0`
   - Shared Blazor layouts and components
   
3. **SufiChain.SufiTheme.Blazor.Server** `v1.0.0-alpha.1.0`
   - Blazor Server-specific components and layouts
   
4. **SufiChain.SufiTheme.Blazor.WebAssembly** `v1.0.0-alpha.1.0`
   - Blazor WebAssembly-specific components and layouts
   
5. **SufiChain.SufiTheme.Blazor.WebAssembly.Bundling** `v1.0.0-alpha.1.0`
   - WebAssembly bundling and style contributors

---

## ✨ Key Features

### Dual-Layout System
- **Collapsed Shell** - Compact navigation with icon rail
- **Expanded Shell** - Full navigation panel with labels
- Smooth transitions between layouts
- User preference persistence

### Multi-Language Support
- **LTR (Left-to-Right)** - English, French, Spanish, etc.
- **RTL (Right-to-Left)** - Arabic, Persian, Hebrew, etc.
- Automatic layout mirroring for RTL languages
- Font optimization for each direction

### Responsive Design
- Mobile-first approach
- Adaptive navigation for small screens
- Touch-friendly controls
- Breakpoint-based layout adjustments

### Layout Types

| Layout name | Implementation | Notes |
| --- | --- | --- |
| **Application** | `SideMenuLayout`, `TopMenuLayout`, or `DualSidebarLayout` (via `SufiThemeBlazorOptions.Layout`) | Main app shell with navigation, toolbar, breadcrumbs |
| **Account** | `AccountLayout` from `SufiChain.SufiAbp.UI.Blazor.Layouts` | Registered at startup as `SufiLayouts.Account`; hosts may use a custom layout (e.g. Console `ConsoleAccountLayout`) |
| **Empty** | `EmptyLayout` in SufiTheme | Minimal wrapper — no navigation chrome |
| **Public** | Same component as **Application** (`StandardLayouts.Public` → `_options.Layout` in `Sufi1Theme`) | Not a separate shell file. Public *navigation* via `IPublicMenuProvider` — see platform doc `sufi-abp/docs/sufi-theme/public-navigation.md` |

### Integration

- Requires **SufiAbp UI** packages (`SufiChain.SufiAbp.UI.*`) — not a SufiBlazor-only add-on
- Built on **SufiBlazor** for all in-content `Sb*` components and design tokens
- Support for Blazor Server and WebAssembly

### Theming & Customization
- CSS variable-based theming
- Customizable color schemes
- Flexible layout options
- Extensible component system

---

## 🔧 Technical Details

### Dependencies

**SufiAbp Framework Packages (v1.0.0-alpha.1.0):**
- SufiChain.SufiPlatform.SufiAI.Abstractions
- SufiChain.SufiAbp.Core
- SufiChain.SufiAbp.Features
- SufiChain.SufiAbp.MultiTenancy
- SufiChain.SufiAbp.Security
- SufiChain.SufiAbp.UI
- SufiChain.SufiAbp.UI.Abstractions
- SufiChain.SufiAbp.UI.Blazor
- SufiChain.SufiAbp.UI.Blazor.Server
- SufiChain.SufiAbp.UI.Blazor.WebAssembly

**Microsoft Packages:**
- Microsoft.AspNetCore.Components.Web v10.0.2
- Microsoft.AspNetCore.Components.Authorization v10.0.2
- Microsoft.AspNetCore.Components.WebAssembly.Authentication v10.0.2

### Target Framework
- .NET 10.0

### Language Features
- C# Latest (C# 13)
- Nullable reference types enabled
- Implicit usings enabled

---

## 📥 Installation

### NuGet Package Manager

```bash
# For Blazor Server applications
dotnet add package SufiChain.SufiTheme.Blazor.Server --version 1.0.0-alpha.1.0

# For Blazor WebAssembly applications
dotnet add package SufiChain.SufiTheme.Blazor.WebAssembly --version 1.0.0-alpha.1.0
```

### Package Manager Console

```powershell
# For Blazor Server
Install-Package SufiChain.SufiTheme.Blazor.Server -Version 1.0.0-alpha.1.0

# For Blazor WebAssembly
Install-Package SufiChain.SufiTheme.Blazor.WebAssembly -Version 1.0.0-alpha.1.0
```

---

## 🚀 Quick Start

### 1. Install the Package

Choose the appropriate package for your Blazor hosting model (Server or WebAssembly).

### 2. Configure Module Dependencies

Add the SufiTheme module dependency to your Blazor module:

```csharp
[DependsOn(typeof(SufiThemeBlazorServerModule))] // or SufiThemeBlazorWebAssemblyModule
public class YourBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<SufiThemeBlazorOptions>(options =>
        {
            // Configure your theme options
            options.DefaultLayout = SufiThemeLayouts.Collapsed; // or Expanded
        });
    }
}
```

### 3. Use SufiTheme Layouts

Reference SufiTheme layouts in your Blazor pages:

```razor
@layout SufiApplicationLayout

<h1>Welcome to SufiTheme</h1>
```

---

## 🎨 Configuration

### Theme Options

```csharp
Configure<SufiThemeBlazorOptions>(options =>
{
    // Default layout (Collapsed or Expanded)
    options.DefaultLayout = SufiThemeLayouts.Collapsed;
    
    // Enable layout switching
    options.AllowLayoutSwitch = true;
    
    // Custom menu items
    options.MenuItems.Add(new ApplicationMenuItem(
        "MyApp.Dashboard",
        "Dashboard",
        "/dashboard",
        icon: "fas fa-home"
    ));
});
```

### Layout Selection

```csharp
public class Sufi1Theme : ITheme
{
    public virtual Type? GetLayout(string name, bool fallbackToDefault = true)
    {
        return name switch
        {
            StandardLayouts.Application => _options.Layout,
            StandardLayouts.Account => SufiLayouts.Account,
            StandardLayouts.Empty => SufiLayouts.Empty,
            StandardLayouts.Public => _options.Layout,
            _ => fallbackToDefault ? _options.Layout : null
        };
    }
}
```

---

## 🐛 Known Issues

### Alpha Release Limitations

1. **Documentation** - Comprehensive documentation is in progress
2. **Examples** - Sample applications coming in beta release
3. **Customization Guide** - Advanced theming guide under development
4. **Performance** - Some optimizations pending for production use

### Compatibility

- Requires SufiAbp Framework v1.0.0-alpha.1.0 or higher
- .NET 10.0 SDK required
- Not backward compatible with .NET 8.0 or earlier

---

## 🔄 Migration Guide

This is the first release, so no migration is needed. For future releases, migration guides will be provided.

---

## 📝 Breaking Changes

None (first release).

---

## 🛠️ Development Notes

### Package Structure

All packages now use **PackageReference** instead of **ProjectReference** to SufiAbp framework packages. This allows:
- Independent development and versioning
- Easier distribution via NuGet
- Simplified CI/CD pipelines
- Clear dependency management

### Build Requirements

- .NET 10.0 SDK
- Visual Studio 2022 17.12+ or Rider 2024.3+
- Node.js 20+ (for frontend tooling)

---

## 🔮 Roadmap

### Beta Release (v1.0.0-beta.1)
- Comprehensive documentation
- Sample applications
- Advanced customization guide
- Performance optimizations
- Additional layout variants

### RC Release (v1.0.0-rc.1)
- Production-ready optimizations
- Accessibility improvements (WCAG 2.1 AA)
- Extended browser compatibility testing
- Migration tools for custom themes

### Stable Release (v1.0.0)
- Full production support
- Long-term support (LTS) commitment
- Complete API stability
- Enterprise-ready features

---

## 📚 Resources

- **Documentation:** https://docs.sufiabp.com/themes/sufi-theme
- **GitHub:** https://github.com/sufichain/sufi-theme
- **NuGet:** https://www.nuget.org/packages/SufiChain.SufiTheme
- **Support:** support@sufichain.ir
- **Community:** https://discord.gg/sufiabp

---

## 🤝 Contributing

SufiTheme is part of the Sufi Platform open-source ecosystem. Contributions are welcome!

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

See [CONTRIBUTING.md](CONTRIBUTING.md) for detailed guidelines.

---

## 📄 License

SufiTheme is licensed under **LGPL-3.0**.

You can use SufiTheme in both open-source and commercial projects without restrictions.

---

## 🙏 Acknowledgments

SufiTheme is built on top of:
- **SufiAbp Framework** - Modular application framework
- **ABP Framework** - Foundation for enterprise applications
- **Blazor** - Microsoft's modern web UI framework
- **.NET** - Cross-platform development platform

Special thanks to the Sufi Platform community for feedback and contributions.

---

## 📞 Support

### Community Support
- Discord: https://discord.gg/sufiabp
- Stack Overflow: Tag questions with `komtheme` or `sufiabp`
- GitHub Issues: https://github.com/sufichain/sufi-theme/issues

### Commercial Support
- Email: support@sufichain.ir
- Priority support packages available
- Custom theme development services
- Training and consulting

---

**Built with ❤️ by the Sufi Chain Team**

---

## Version History

- **v1.0.0-alpha.1.0** (2025-05-24) - First alpha release
