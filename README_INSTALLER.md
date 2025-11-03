# Open Adhan Installer Documentation

## Quick Start

### For End Users
Download the latest `OpenAdhanSetup.msi` from the [Releases](https://github.com/AdhanForWindows/OpenAdhan/releases) page and run it.

### For Developers

**Prerequisites:**
```cmd
# 1. Check .NET SDK
dotnet --version  # Should be 6.0 or later

# 2. Install WiX Toolset v6
dotnet tool install --global wix --version 6.0.2

# 3. Verify
wix --version
```

**Build Installer:**
```cmd
.\build-installer.bat
```

**Output:** `OpenAdhanInstaller\bin\Release\**\OpenAdhanSetup.msi`

## Documentation Files

### Getting Started
- **[QUICK_START.md](QUICK_START.md)** - Quick reference for developers
- **[BUILDING.md](BUILDING.md)** - Complete build instructions

### Installer-Specific
- **[OpenAdhanInstaller/README.md](OpenAdhanInstaller/README.md)** - Installer project documentation
- **[INSTALLER_MIGRATION.md](INSTALLER_MIGRATION.md)** - Migration from InstallForge to WiX
- **[WIX_V6_MIGRATION.md](WIX_V6_MIGRATION.md)** - WiX v3 to v6 migration details
- **[OpenAdhanInstaller/WIX_V6_INFO.md](OpenAdhanInstaller/WIX_V6_INFO.md)** - WiX v6 features and usage

### All Changes
- **[CHANGES_SUMMARY.md](CHANGES_SUMMARY.md)** - Summary of all project changes

## Technology Stack

### Application
- **.NET Framework 4.7.2** - Application runtime
- **Windows Forms** - UI framework
- **NAudio** - Audio playback
- **Visual Basic PowerPacks** - UI shapes/lines

### Installer
- **WiX Toolset v6.0.2** - MSI installer creation
- **.NET 6+** - WiX tooling runtime
- **MSBuild** - Build automation

### CI/CD
- **GitHub Actions** - Automated builds
- **Windows Server 2022** - Build environment

## Features

### Application Features
- ✅ Islamic prayer time calculations
- ✅ Automatic Adhan playback
- ✅ Windows notifications
- ✅ System tray integration
- ✅ Custom minimize button
- ✅ Minimizes to taskbar
- ✅ Win+Down keyboard shortcut support
- ✅ Customizable prayer time adjustments
- ✅ Multiple calculation methods

### Installer Features
- ✅ Professional MSI package
- ✅ Automatic registry setup
- ✅ Start Menu shortcuts
- ✅ Upgrade detection
- ✅ Clean uninstallation
- ✅ CLI build support
- ✅ GitHub Actions integration

## Build Commands

### Build Everything
```cmd
.\build-installer.bat
```

### Build with Version
```cmd
.\build-installer.bat Release 1.2.3.0
```

### Build Application Only
```cmd
msbuild OpenAdhanForWindowsX.sln /p:Configuration=Release /t:Rebuild
```

### Build Installer Only
```cmd
dotnet build OpenAdhanInstaller\OpenAdhanInstaller.wixproj -c Release
```

### Clean Build
```cmd
msbuild OpenAdhanForWindowsX.sln /t:Clean
dotnet clean OpenAdhanInstaller\OpenAdhanInstaller.wixproj
```

## GitHub Actions

### Automatic Releases

The installer is automatically built and released when you push a version tag:

```bash
# 1. Commit changes
git add .
git commit -m "Release v1.2.3"

# 2. Create and push tag
git tag v1.2.3
git push origin v1.2.3

# 3. GitHub Actions automatically:
#    - Builds the application
#    - Creates the MSI installer
#    - Publishes a GitHub release
#    - Uploads the MSI as a release asset
```

### Manual Trigger

You can also trigger builds manually:
1. Go to the "Actions" tab on GitHub
2. Select "Build and Release" workflow
3. Click "Run workflow"
4. Enter version number
5. Click "Run workflow" button

## Installation Details

### What Gets Installed

```
C:\Program Files\Open Adhan for Windows\
├── OpenAdhanForWindows.exe       # Main application
├── OpenAdhanRegistrySetupApp.exe # Registry setup utility
├── 3dkaaba_48x48.ico             # Application icon
├── OpenAdhanForWindows.exe.config
├── NAudio.Core.dll               # Audio library
├── NAudio.dll
├── NAudio.Asio.dll
├── NAudio.Midi.dll
├── NAudio.Wasapi.dll
├── NAudio.WinForms.dll
├── NAudio.WinMM.dll
├── Microsoft.VisualBasic.PowerPacks.dll
├── Microsoft.Win32.Registry.dll
├── System.Security.AccessControl.dll
├── System.Security.Principal.Windows.dll
└── Resources\
    ├── Mishary97Bismillah.wav    # Bismillah audio
    └── system_tray.ico           # Tray icon
```

### Registry Keys Created

```
HKEY_LOCAL_MACHINE\SOFTWARE\OpenAdhan\
├── Latitude
├── Longitude
├── TimeZone
├── CalculationMethod
├── JuristicMethod
├── FajrAdjustment
├── ShurookAdjustment
├── DhuhrAdjustment
├── AsrAdjustment
├── MaghribAdjustment
├── IshaAdjustment
├── PlayAdhanAtPrayerTimes
├── SendNotificationAtPrayerTimes
├── MinimizeOnStartup
├── BismillahOnStartup
├── AutomaticDaylightSavingsAdjustment
├── NormalAdhan
├── FajrAdhan
└── OpenAdhanInstalledVersion
```

### Start Menu Shortcuts

```
Start Menu\Programs\Open Adhan for Windows\
└── Open Adhan for Windows.lnk
```

## System Requirements

### For End Users
- **OS**: Windows 10 or later
- **Architecture**: x64
- **.NET Framework**: 4.7.2 or later (usually pre-installed)
- **RAM**: 50 MB
- **Disk Space**: 10 MB

### For Developers
- **OS**: Windows 10 or later
- **Visual Studio**: 2022 or later (Community Edition works)
- **.NET SDK**: 6.0 or later
- **WiX Toolset**: v6.0.2
- **Disk Space**: 2 GB for development

## Troubleshooting

### For End Users

**Installer won't run:**
- Right-click MSI → Run as Administrator
- Check Windows Installer service is running

**Application won't launch:**
- Check if .NET Framework 4.7.2 is installed
- Try reinstalling the application

**No prayer times showing:**
- Open Settings and configure your location
- Check latitude/longitude values

### For Developers

**Build fails:**
```cmd
# Clean and rebuild
msbuild OpenAdhanForWindowsX.sln /t:Clean
msbuild OpenAdhanForWindowsX.sln /t:Rebuild
```

**WiX not found:**
```cmd
dotnet tool install --global wix --version 6.0.2
```

**Missing dependencies:**
```cmd
nuget restore OpenAdhanForWindowsX.sln
```

## Support

### For Users
- **Website**: https://www.openadhan.com
- **GitHub Issues**: https://github.com/AdhanForWindows/OpenAdhan/issues

### For Developers
- **Documentation**: See files listed at top of this document
- **GitHub Discussions**: https://github.com/AdhanForWindows/OpenAdhan/discussions
- **WiX Documentation**: https://wixtoolset.org/docs/

## License

Open Adhan is licensed under the MIT License. See [LICENSE](LICENSE) for details.

## Contributing

Contributions are welcome! Please:
1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test the installer build
5. Submit a pull request

See [BUILDING.md](BUILDING.md) for development setup.

## Version History

- **v1.0.0** - Initial WiX v6 installer release
  - Migrated from InstallForge to WiX Toolset
  - Added minimize button with Win+Down support
  - Automated GitHub Actions builds

## Maintainers

- **iysaleh** - Original Developer, Maintainer
- **abdullahbaa5** - UI Developer

## Acknowledgments

- WiX Toolset team for the excellent installer framework
- NAudio library for audio playback capabilities
- All contributors to the Open Adhan project
