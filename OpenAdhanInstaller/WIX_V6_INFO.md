# WiX Toolset v6 Information

This installer uses **WiX Toolset v6.0.2**, the latest version of the industry-standard Windows Installer toolset.

## What's New in WiX v6

### Modern .NET Integration
- WiX v6 is built on .NET and distributed as a .NET tool
- No more standalone installers - just run `dotnet tool install --global wix`
- Integrates seamlessly with modern .NET build workflows

### SDK-Style Projects
- Uses modern SDK-style project format (`<Project Sdk="WixToolset.Sdk/6.0.2">`)
- Simpler project files, no more verbose MSBuild XML
- Better NuGet package management

### Improved CLI
- `wix build` command for building installers
- Better error messages and diagnostics
- Cross-platform development support (build on Windows, preview on any OS)

### Updated XML Schema
- New namespace: `http://wixtoolset.org/schemas/v4/wxs`
- Simplified syntax for common scenarios
- Better IntelliSense support in modern editors

## Installation

### Quick Install
```cmd
dotnet tool install --global wix --version 6.0.2
```

### Verify Installation
```cmd
wix --version
```

Expected output: `wix v6.0.2.0+<hash>`

### Update Existing Installation
```cmd
dotnet tool update --global wix --version 6.0.2
```

## Building with WiX v6

### Using MSBuild (Recommended for CI/CD)
```cmd
dotnet build OpenAdhanInstaller.wixproj -c Release
```

### Using WiX CLI Directly
```cmd
wix build Product.wxs -o OpenAdhanSetup.msi
```

### Using Our Build Script
```cmd
cd path\to\OpenAdhan
.\build-installer.ps1
```

The build script automatically:
1. Checks if WiX is installed
2. Installs WiX v6.0.2 if not found
3. Builds the application
4. Builds the MSI installer

## Project Structure

### OpenAdhanInstaller.wixproj (SDK-Style)
```xml
<Project Sdk="WixToolset.Sdk/6.0.2">
  <PropertyGroup>
    <OutputName>OpenAdhanSetup</OutputName>
    <OutputType>Package</OutputType>
  </PropertyGroup>
  
  <ItemGroup>
    <PackageReference Include="WixToolset.UI.wixext" Version="6.0.2" />
  </ItemGroup>
</Project>
```

### Product.wxs (WiX v6 Schema)
- Uses `http://wixtoolset.org/schemas/v4/wxs` namespace
- Modern UI via `<ui:WixUI>` element
- Simplified component definitions

## Key Differences from WiX v3

| Feature | WiX v3 | WiX v6 |
|---------|--------|--------|
| Installation | Standalone MSI | .NET global tool |
| Project Format | Classic MSBuild | SDK-style |
| Build Command | `candle.exe` + `light.exe` | `wix build` or `dotnet build` |
| XML Namespace | `http://schemas.microsoft.com/wix/2006/wi` | `http://wixtoolset.org/schemas/v4/wxs` |
| UI Extension | `<UIRef Id="WixUI_Minimal" />` | `<ui:WixUI Id="WixUI_Minimal" />` |
| Package Element | `InstallScope="perMachine"` | `Scope="perMachine"` |

## NuGet Packages

WiX v6 uses NuGet for extensions:

- **WixToolset.UI.wixext** - Standard UI dialogs
- **WixToolset.Util.wixext** - Utility extensions
- **WixToolset.Firewall.wixext** - Firewall configuration
- And many more...

Add to project:
```xml
<ItemGroup>
  <PackageReference Include="WixToolset.UI.wixext" Version="6.0.2" />
</ItemGroup>
```

## Benefits for Open Adhan

1. **Easier CI/CD**: Just `dotnet build` - no special installers needed on build agents
2. **Modern Tooling**: Better IDE support, IntelliSense, and diagnostics
3. **Reproducible Builds**: Everything defined in code and NuGet packages
4. **Easier Maintenance**: Simpler project format, less boilerplate
5. **Future-Proof**: Active development, modern .NET ecosystem

## Troubleshooting

### "wix: command not found"
```cmd
# Install WiX
dotnet tool install --global wix --version 6.0.2

# If still not found, check PATH
dotnet tool list -g
```

### "Project SDK 'WixToolset.Sdk/6.0.2' could not be found"
```cmd
# The SDK is downloaded automatically via NuGet
# Ensure internet connection and NuGet is working
dotnet restore OpenAdhanInstaller.wixproj
```

### Build Errors
```cmd
# Clean and rebuild
dotnet clean OpenAdhanInstaller.wixproj
dotnet build OpenAdhanInstaller.wixproj -c Release
```

### Get Detailed Build Output
```cmd
dotnet build OpenAdhanInstaller.wixproj -c Release -v detailed
```

## Resources

- **Official Documentation**: https://wixtoolset.org/docs/
- **GitHub Repository**: https://github.com/wixtoolset/wix
- **Migration Guide**: https://wixtoolset.org/docs/fourthree/
- **Release Notes**: https://github.com/wixtoolset/wix/releases/tag/v6.0.2

## Support

For WiX-specific issues:
1. Check official documentation: https://wixtoolset.org/docs/
2. Search GitHub issues: https://github.com/wixtoolset/wix/issues
3. Ask on Discord: https://discord.gg/wix (WiX Community)

For Open Adhan installer issues:
- Check `OpenAdhanInstaller\README.md`
- Check `INSTALLER_MIGRATION.md`
- Open an issue on GitHub
