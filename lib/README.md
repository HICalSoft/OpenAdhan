# Library Dependencies

This folder contains third-party DLL files that are required for building the project but are not available via NuGet.

## Microsoft.VisualBasic.PowerPacks.dll

**Version:** 9.0.0.0  
**Size:** ~344 KB  
**Purpose:** Provides shape and line drawing controls used in the Windows Forms UI

### Why is this here?

Visual Basic PowerPacks is a legacy library from Microsoft that is no longer available through NuGet or the Visual Studio installer. However, it is still required for the OpenAdhan UI which uses `OvalShape` controls for drawing sun/moon animations.

### Source

This DLL was originally installed via the Visual Basic PowerPacks 3.0 installer, which is included in the `redist` folder for reference.

### License

Microsoft Visual Basic Power Packs is provided under Microsoft's standard software license terms. The library is freely redistributable with applications.

### Alternative

In future versions, we may replace the PowerPacks shapes with native drawing code or images to remove this dependency.

## Usage in Build

The project file (`OpenAdhanForWindowsX.csproj`) references this DLL with:

```xml
<Reference Include="Microsoft.VisualBasic.PowerPacks, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL">
  <HintPath>..\lib\Microsoft.VisualBasic.PowerPacks.dll</HintPath>
  <Private>True</Private>
</Reference>
```

The `<Private>True</Private>` ensures the DLL is copied to the output directory during build.

## For Developers

If you're building this project:
1. This DLL will be automatically referenced from the `lib` folder
2. No additional installation is required
3. The DLL will be automatically copied to `bin\Release` during build
4. The installer includes this DLL for distribution

## For CI/CD (GitHub Actions)

This folder ensures the build works in CI/CD environments like GitHub Actions where the Visual Basic PowerPacks cannot be pre-installed.
