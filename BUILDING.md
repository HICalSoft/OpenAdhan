# Building Open Adhan for Windows

This guide explains how to build Open Adhan from source and create the installer.

## Prerequisites

### Required
1. **Visual Studio 2022** or **Build Tools for Visual Studio 2022**
   - Workload: .NET desktop development
   - Download: https://visualstudio.microsoft.com/downloads/

2. **.NET Framework 4.7.2 or later** (included with VS 2022)

### Required for Installer
3. **.NET 6.0 SDK or later**
   - Download: https://dotnet.microsoft.com/download
   
4. **WiX Toolset v6.0.2** (installed via dotnet tool)
   - Install command: `dotnet tool install --global wix --version 6.0.2`
   - Or run the build script which will auto-install it

## Building the Application

### Using Visual Studio

1. Open `OpenAdhanForWindowsX.sln`
2. Select `Release` configuration
3. Build > Rebuild Solution (Ctrl+Shift+B)

Output: `OpenAdhanForWindowsX\bin\Release\OpenAdhanForWindows.exe`

### Using Command Line

```cmd
cd "C:\path\to\OpenAdhan"
msbuild OpenAdhanForWindowsX.sln /p:Configuration=Release /t:Rebuild
```

## Building the Installer

### Prerequisites Check

Run this to verify all prerequisites are installed:

```cmd
where msbuild
dotnet --version
wix --version
```

### Build Installer

From the repository root:

```cmd
build-installer.bat
```

Or with specific version:

```cmd
build-installer.bat Release 1.0.0.0
```

Output: `OpenAdhanInstaller\bin\Release\OpenAdhanSetup.msi`

### What the Build Script Does

1. Builds `OpenAdhanForWindowsX` project
2. Builds `OpenAdhanRegistrySetupApp` project
3. Compiles WiX source files (.wxs)
4. Links and creates MSI installer

## Project Structure

```
OpenAdhan/
├── OpenAdhanForWindowsX/          # Main application
│   ├── Form1.cs                   # Main form
│   ├── PrayerTimesControl.cs      # Prayer times logic
│   ├── RegistrySettingsHandler.cs # Settings management
│   └── Resources/                 # Icons, audio files
├── OpenAdhanRegistrySetupApp/     # Registry setup utility
│   └── Program.cs                 # Initializes registry keys
├── OpenAdhanInstaller/            # WiX installer project
│   ├── Product.wxs                # Installer definition
│   └── license.rtf                # License agreement
└── OpenAdhanUnitTests/            # Unit tests
```

## Running Tests

### Using Visual Studio

1. Test > Run All Tests

### Using Command Line

```cmd
vstest.console OpenAdhanUnitTests\bin\Release\OpenAdhanUnitTests.dll
```

## Common Issues

### Missing NuGet Packages

**Error**: Cannot find NAudio or other packages

**Solution**:
```cmd
nuget restore OpenAdhanForWindowsX.sln
```

### WiX Not Found

**Error**: `WiX Toolset not found`

**Solution**: 
```cmd
dotnet tool install --global wix --version 6.0.2
```

### MSBuild Not Found

**Error**: `msbuild is not recognized`

**Solution**: 
1. Install Visual Studio 2022 or Build Tools
2. Use Developer Command Prompt for VS 2022

### Application Won't Start

**Common causes**:
- Missing dependencies (NAudio DLLs)
- Missing Resources folder
- Registry keys not initialized

**Solution**: Run the installer instead of running the exe directly

## Development Workflow

1. Make code changes in Visual Studio
2. Test locally (F5 to debug)
3. Build Release version
4. Build installer
5. Test installer on clean system
6. Commit changes
7. Push tag to trigger GitHub Actions release

## Debugging

### Debug Mode

1. Set configuration to `Debug`
2. Press F5 in Visual Studio
3. Use breakpoints as needed

### Registry Issues

Check registry keys at:
```
HKEY_LOCAL_MACHINE\SOFTWARE\OpenAdhan
```

### Audio Issues

Verify audio files exist:
```
OpenAdhanForWindowsX\bin\Release\Resources\Mishary97Bismillah.wav
```

## Performance

The application is lightweight:
- Executable: ~240 KB
- With dependencies: ~2 MB
- Installer: ~5 MB (includes all dependencies and resources)

## Next Steps

- Read [INSTALLER_MIGRATION.md](INSTALLER_MIGRATION.md) for installer details
- See [.github/workflows/build-release.yml](.github/workflows/build-release.yml) for CI/CD setup
- Check [OpenAdhanInstaller/README.md](OpenAdhanInstaller/README.md) for installer customization
