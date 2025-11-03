# Open Adhan Installer

This directory contains the WiX Toolset-based installer for Open Adhan for Windows.

## Prerequisites

To build the installer, you need:

1. **Visual Studio 2022** (or Build Tools for Visual Studio 2022)
2. **.NET 6.0 SDK or later**
   - Download from: https://dotnet.microsoft.com/download
3. **WiX Toolset v6.0.2**
   - Install via: `dotnet tool install --global wix --version 6.0.2`
   - Or the build script will auto-install it

## Building the Installer

### Command Line (Recommended)

From the repository root directory:

```cmd
build-installer.bat
```

Or with custom configuration and version:

```cmd
build-installer.bat Release 1.0.0.0
```

### Using PowerShell

```powershell
.\build-installer.ps1 -Configuration Release -Version "1.0.0.0"
```

### Manual Build

1. Build OpenAdhanForWindowsX project
2. Build OpenAdhanRegistrySetupApp project
3. Build OpenAdhanInstaller.wixproj using MSBuild

## Output

The installer will be created at:
```
OpenAdhanInstaller\bin\Release\OpenAdhanSetup.msi
```

## Features

The installer includes:

- Installs Open Adhan for Windows to Program Files
- Creates Start Menu shortcuts
- Runs registry setup automatically on first install
- Supports automatic updates (detects newer versions)
- Proper uninstall support
- All required dependencies (NAudio, PowerPacks, etc.)
- Prayer resources (Bismillah audio, icons)

## GitHub Actions Integration

The installer build is integrated into GitHub Actions via the `.github/workflows/build-release.yml` workflow.

## Customization

### Changing Product Information

Edit the variables in `Product.wxs`:

```xml
<?define ProductVersion="1.0.0.0" ?>
<?define ProductName="Open Adhan for Windows" ?>
<?define Manufacturer="Open Adhan" ?>
```

### Adding Files

Add new `<Component>` entries in the `ProductComponents` or `ResourceComponents` component groups in `Product.wxs`.

### Modifying UI

The installer uses `WixUI_Minimal` UI. To change it, update the `<UI>` section in `Product.wxs`.

## Troubleshooting

### "WiX Toolset not found"

Install WiX Toolset v6 as a .NET global tool:
```cmd
dotnet tool install --global wix --version 6.0.2
```

Verify installation:
```cmd
wix --version
```

### "MSBuild not found"

Install Visual Studio 2022 or Build Tools for Visual Studio 2022.

### Build Errors

1. Ensure all projects build successfully in Visual Studio first
2. Check that all file paths in `Product.wxs` are correct
3. Verify GUIDs are unique for each component
