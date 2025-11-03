# Installer Migration Guide

This document describes the migration from InstallForge to WiX Toolset for Open Adhan's installer.

## What Changed

### Before (InstallForge)
- Manual installer creation using InstallForge GUI
- Not suitable for CI/CD automation
- Limited customization options
- Proprietary format (.ifp files)

### After (WiX Toolset v6)
- Industry-standard MSI installer
- Modern .NET-based tooling
- Full CLI support for automation
- Easily integrated with GitHub Actions
- SDK-style project format
- Better Windows integration

## New Installer Features

1. **Professional MSI Installer**: Industry-standard Windows Installer package
2. **Automatic Registry Setup**: Runs `OpenAdhanRegistrySetupApp.exe` during installation
3. **Start Menu Integration**: Automatically creates Start Menu shortcuts
4. **Proper Upgrade Handling**: Detects and handles upgrades automatically
5. **Clean Uninstall**: Removes all files and registry entries on uninstall
6. **CLI Build Support**: Can be built from command line for CI/CD

## Building the Installer

### Prerequisites

1. **Visual Studio 2022** or **Build Tools for Visual Studio 2022**
2. **.NET 6.0 SDK or later**
   - Download: https://dotnet.microsoft.com/download
3. **WiX Toolset v6.0.2**
   - Install: `dotnet tool install --global wix --version 6.0.2`

### Quick Start

From the repository root:

```cmd
build-installer.bat
```

The installer will be created at: `OpenAdhanInstaller\bin\Release\OpenAdhanSetup.msi`

### Advanced Usage

Build with specific version:
```cmd
build-installer.bat Release 1.2.3.0
```

Using PowerShell:
```powershell
.\build-installer.ps1 -Configuration Release -Version "1.2.3.0"
```

## GitHub Actions Integration

The installer is automatically built on GitHub Actions when:

1. **Tag Push**: Create a tag starting with `v` (e.g., `v1.0.0`)
   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```

2. **Manual Trigger**: Use the "Actions" tab on GitHub and run "Build and Release" workflow

The workflow will:
- Build the application
- Build the MSI installer
- Upload the installer as an artifact
- Create a GitHub release (for tag pushes)

## File Structure

```
OpenAdhan/
├── OpenAdhanInstaller/          # New installer project
│   ├── Product.wxs              # WiX installer definition
│   ├── OpenAdhanInstaller.wixproj
│   ├── license.rtf              # License shown during install
│   └── README.md                # Installer documentation
├── build-installer.ps1          # PowerShell build script
├── build-installer.bat          # Batch file wrapper
└── .github/
    └── workflows/
        └── build-release.yml    # GitHub Actions workflow
```

## Customizing the Installer

### Change Version

Edit in `build-installer.ps1` or pass as parameter:
```cmd
build-installer.bat Release 2.0.0.0
```

### Change Product Information

Edit `OpenAdhanInstaller\Product.wxs`:
```xml
<?define ProductVersion="1.0.0.0" ?>
<?define ProductName="Open Adhan for Windows" ?>
<?define Manufacturer="Open Adhan" ?>
```

### Add/Remove Files

In `Product.wxs`, add components to the `ProductComponents` group:
```xml
<Component Id="NewFile" Guid="{NEW-GUID-HERE}">
  <File Id="NewFile.dll" Source="..\path\to\file.dll" />
</Component>
```

### Modify Installation Directory

Change the `INSTALLFOLDER` in `Product.wxs`:
```xml
<Directory Id="INSTALLFOLDER" Name="Open Adhan for Windows">
```

## Testing the Installer

1. Build the installer: `build-installer.bat`
2. Run the MSI: `OpenAdhanInstaller\bin\Release\OpenAdhanSetup.msi`
3. Verify:
   - Application installs to Program Files
   - Start Menu shortcut is created
   - Registry keys are created (check `HKLM\Software\OpenAdhan`)
   - Application launches successfully
4. Uninstall from Control Panel and verify clean removal

## Troubleshooting

### WiX Toolset Not Found

**Error**: `WiX Toolset not found`

**Solution**: Install WiX Toolset v6 as a .NET global tool:
```cmd
dotnet tool install --global wix --version 6.0.2
```

Verify installation:
```cmd
wix --version
```
Should output: `wix v6.0.2.0+...`

### MSBuild Not Found

**Error**: `MSBuild not found`

**Solution**: Install Visual Studio 2022 or Build Tools for Visual Studio 2022

### Build Fails on GitHub Actions

**Solution**: Check the Actions log for specific errors. Common issues:
- NuGet package restore failed
- WiX installation failed
- Missing files in Release folder

### Registry Setup Doesn't Run

**Solution**: Ensure `OpenAdhanRegistrySetupApp.exe` is built and in the Release folder before building the installer.

## Migration Checklist

- [x] Create WiX installer project
- [x] Add all application files to installer
- [x] Add all dependencies (NAudio, PowerPacks, etc.)
- [x] Add Resources folder (audio files, icons)
- [x] Configure registry setup to run after install
- [x] Add Start Menu shortcuts
- [x] Create build scripts (PowerShell and batch)
- [x] Create GitHub Actions workflow
- [x] Test installation on clean system
- [ ] Update documentation
- [x] Remove old InstallForge files
- [ ] Update release process documentation

## Benefits of WiX

1. **Professional**: Industry-standard MSI format
2. **Automatable**: Full CLI support for CI/CD
3. **Flexible**: XML-based, easy to customize
4. **Maintainable**: Version controlled, no binary formats
5. **Reliable**: Built on Windows Installer technology
6. **Free**: Open-source, no licensing costs

## Next Steps

1. Test the new installer thoroughly
2. Update README.md with new installation instructions
3. Remove old InstallForge files from repository
4. Create a release using the new installer
5. Update documentation to reflect new build process
