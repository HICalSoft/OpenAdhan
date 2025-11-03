# Quick Start Guide

## Prerequisites Installation

### 1. Install Visual Studio 2022
Download from: https://visualstudio.microsoft.com/downloads/
- Choose: Community Edition (free)
- Workload: .NET desktop development

### 2. Install .NET SDK
Download from: https://dotnet.microsoft.com/download
- Install .NET 6.0 or later

### 3. Install WiX Toolset v6
```cmd
dotnet tool install --global wix --version 6.0.2
```

### 4. Verify Installation
```cmd
where msbuild
dotnet --version
wix --version
```

## Building the Application

### Quick Build
```cmd
cd "C:\path\to\OpenAdhan"
msbuild OpenAdhanForWindowsX.sln /p:Configuration=Release /t:Rebuild
```

### Using Visual Studio
1. Open `OpenAdhanForWindowsX.sln`
2. Select "Release" configuration
3. Press Ctrl+Shift+B

## Building the Installer

### One Command
```cmd
build-installer.bat
```

Output: `OpenAdhanInstaller\bin\Release\OpenAdhanSetup.msi`

### With Version Number
```cmd
build-installer.bat Release 1.0.0.0
```

## Testing the Installer

1. Double-click `OpenAdhanSetup.msi`
2. Follow installation wizard
3. Launch application from Start Menu
4. Verify prayer times display correctly
5. Test minimize button

## Creating a Release

### Local Testing
```cmd
# 1. Build everything
build-installer.bat Release 1.0.0.0

# 2. Test installer
start OpenAdhanInstaller\bin\Release\OpenAdhanSetup.msi

# 3. Verify installation
```

### GitHub Release
```bash
# 1. Commit your changes
git add .
git commit -m "Release version 1.0.0"

# 2. Create and push tag
git tag v1.0.0
git push origin v1.0.0

# 3. GitHub Actions will automatically:
#    - Build the application
#    - Create the installer
#    - Create a GitHub release
#    - Upload the MSI
```

## Common Commands

```cmd
# Build application only
msbuild OpenAdhanForWindowsX.sln /p:Configuration=Release /t:Rebuild

# Build installer only (assumes app is built)
cd OpenAdhanInstaller
candle Product.wxs -ext WixUIExtension
light -out OpenAdhanSetup.msi Product.wixobj -ext WixUIExtension

# Clean build
msbuild OpenAdhanForWindowsX.sln /t:Clean
rmdir /s /q OpenAdhanInstaller\bin
rmdir /s /q OpenAdhanInstaller\obj

# Run tests
vstest.console OpenAdhanUnitTests\bin\Release\OpenAdhanUnitTests.dll
```

## Troubleshooting

### "WiX not found"
```cmd
# Check if installed
wix --version

# If not found, install it
dotnet tool install --global wix --version 6.0.2
```

### "MSBuild not found"
```cmd
# Use Developer Command Prompt for VS 2022
# Or add MSBuild to PATH:
set PATH=%PATH%;C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin
```

### Build fails
```cmd
# Clean and rebuild
msbuild OpenAdhanForWindowsX.sln /t:Clean
msbuild OpenAdhanForWindowsX.sln /t:Rebuild
```

### Installer won't run
- Right-click MSI → Run as Administrator
- Check Windows Installer service is running
- Try: `msiexec /i OpenAdhanSetup.msi /l*v install.log`

## File Locations

```
Application EXE:
  OpenAdhanForWindowsX\bin\Release\OpenAdhanForWindows.exe

Installer MSI:
  OpenAdhanInstaller\bin\Release\OpenAdhanSetup.msi

Logs:
  %TEMP%\OpenAdhan*.log
  C:\Windows\Installer\*.log
```

## Development Workflow

1. **Make Changes** - Edit code in Visual Studio
2. **Test Debug** - Press F5 to run in debug mode
3. **Build Release** - Switch to Release, rebuild
4. **Build Installer** - Run `build-installer.bat`
5. **Test Installer** - Install on test system
6. **Commit** - Commit to git
7. **Tag Release** - Push tag to trigger GitHub Actions

## Key Files to Know

- `Form1.cs` - Main application logic
- `Form1.Designer.cs` - UI designer code
- `Product.wxs` - Installer definition
- `build-installer.ps1` - Build script
- `.github/workflows/build-release.yml` - CI/CD config

## Get Help

- Build issues: Check BUILDING.md
- Installer issues: Check INSTALLER_MIGRATION.md
- All changes: Check CHANGES_SUMMARY.md
- WiX help: https://wixtoolset.org/documentation/
