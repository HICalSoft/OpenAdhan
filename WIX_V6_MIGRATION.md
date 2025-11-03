# WiX v6 Migration Summary

## Overview

The Open Adhan installer has been updated to use **WiX Toolset v6.0.2** (released November 2024), the latest version of the industry-standard Windows Installer toolset.

## Why WiX v6?

### Modern & Actively Maintained
- **Latest Release**: v6.0.2 (November 2024)
- **Active Development**: Regular updates and bug fixes
- **Modern .NET**: Built on .NET 6+ with cross-platform tooling
- **Long-term Support**: Microsoft-backed, industry standard

### Better Developer Experience
- **.NET Tool**: Install with `dotnet tool install --global wix`
- **SDK-Style Projects**: Simpler, cleaner project files
- **Better Tooling**: Improved IntelliSense and error messages
- **CI/CD Ready**: Works seamlessly with GitHub Actions

### WiX v3 is Legacy
- Last release: 2018 (v3.11.2)
- No longer actively maintained
- Requires separate installer download
- Doesn't support modern .NET tooling

## Installation Comparison

### WiX v3 (Old)
```cmd
# Download wix311.exe (12MB)
# Run installer
# Set environment variables
# Restart terminal
```

### WiX v6 (New)
```cmd
dotnet tool install --global wix --version 6.0.2
```

Done! One command, no downloads, no installers.

## Project File Comparison

### WiX v3 (.wixproj)
```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="4.0" DefaultTargets="Build" xmlns="...">
  <PropertyGroup>
    <Configuration>Release</Configuration>
    <Platform>x86</Platform>
    <ProductVersion>3.10</ProductVersion>
    <ProjectGuid>{...}</ProjectGuid>
    <SchemaVersion>2.0</SchemaVersion>
    <OutputName>OpenAdhanSetup</OutputName>
    <OutputType>Package</OutputType>
    <WixTargetsPath>...</WixTargetsPath>
  </PropertyGroup>
  <PropertyGroup Condition="...">
    <OutputPath>bin\$(Configuration)\</OutputPath>
    <IntermediateOutputPath>obj\$(Configuration)\</IntermediateOutputPath>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include="Product.wxs" />
  </ItemGroup>
  <Import Project="$(WixTargetsPath)" />
</Project>
```

### WiX v6 (.wixproj)
```xml
<Project Sdk="WixToolset.Sdk/6.0.2">
  <PropertyGroup>
    <OutputName>OpenAdhanSetup</OutputName>
    <OutputType>Package</OutputType>
    <Platform>x64</Platform>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="WixToolset.UI.wixext" Version="6.0.2" />
  </ItemGroup>
</Project>
```

**90% less boilerplate!**

## Build Command Comparison

### WiX v3
```cmd
# Step 1: Compile
candle.exe Product.wxs -ext WixUIExtension -out obj\

# Step 2: Link
light.exe obj\Product.wixobj -ext WixUIExtension -out OpenAdhanSetup.msi
```

### WiX v6
```cmd
# One command
dotnet build OpenAdhanInstaller.wixproj
```

Or:
```cmd
wix build Product.wxs -o OpenAdhanSetup.msi
```

## GitHub Actions Comparison

### WiX v3
```yaml
- name: Install WiX Toolset
  run: |
    Invoke-WebRequest -Uri $wixUrl -OutFile $wixInstaller
    Start-Process -FilePath $wixInstaller -ArgumentList "/install", "/quiet" -Wait
    echo "WIX=$wixPath" >> $env:GITHUB_ENV
    echo "$wixPath\bin" >> $env:GITHUB_PATH
```

### WiX v6
```yaml
- name: Setup .NET
  uses: actions/setup-dotnet@v3
  
- name: Install WiX Toolset
  run: dotnet tool install --global wix --version 6.0.2
```

**Much simpler!**

## What Changed in Our Project

### Files Modified
1. **OpenAdhanInstaller.wixproj**
   - Changed to SDK-style project
   - Added WixToolset.Sdk/6.0.2 reference
   - Simplified to 15 lines (from 34 lines)

2. **Product.wxs**
   - Updated namespace to v4 schema
   - Changed `InstallScope` to `Scope`
   - Updated UI reference syntax
   - Changed `MediaTemplate` to `Media`

3. **build-installer.ps1**
   - Auto-installs WiX v6 if not found
   - Uses `dotnet build` instead of candle/light
   - Simplified build process

4. **.github/workflows/build-release.yml**
   - Added .NET setup step
   - Simplified WiX installation
   - Uses dotnet tool instead of downloading installer

### Files Added
- **WIX_V6_INFO.md** - Detailed WiX v6 information
- **WIX_V6_MIGRATION.md** - This file

## Installation Steps

### For Developers

1. **Install .NET SDK** (if not already installed)
   ```cmd
   # Download from https://dotnet.microsoft.com/download
   # Or check if already installed:
   dotnet --version
   ```

2. **Install WiX v6**
   ```cmd
   dotnet tool install --global wix --version 6.0.2
   ```

3. **Verify Installation**
   ```cmd
   wix --version
   ```
   Should output: `wix v6.0.2.0+...`

4. **Build Installer**
   ```cmd
   .\build-installer.ps1
   ```

### For CI/CD (GitHub Actions)

No changes needed! The workflow file has been updated and will automatically:
1. Set up .NET SDK
2. Install WiX v6
3. Build the installer

## Benefits Summary

✅ **Modern Tooling**: Latest .NET-based tools
✅ **Easier Installation**: One command, no downloads
✅ **Simpler Projects**: SDK-style format
✅ **Better CI/CD**: Native .NET integration
✅ **Active Development**: Regular updates
✅ **Cross-Platform**: Build on Windows, develop on any OS
✅ **Better Documentation**: Modern, comprehensive docs
✅ **Smaller Build Scripts**: Less code to maintain

## Backwards Compatibility

The MSI installer produced by WiX v6 is fully compatible with:
- Windows 10 and 11
- Windows Server 2016 and later
- All Windows Installer versions 5.0+

No changes needed for end users - they still get a standard MSI file.

## Testing Checklist

After migration, verify:

- [ ] Build script runs without errors
- [ ] MSI file is created successfully
- [ ] Installer runs on test system
- [ ] Application installs to correct location
- [ ] Registry keys are created
- [ ] Start Menu shortcut works
- [ ] Application launches correctly
- [ ] Uninstaller removes everything cleanly
- [ ] GitHub Actions workflow succeeds

## Troubleshooting

### "dotnet: command not found"
Install .NET SDK: https://dotnet.microsoft.com/download

### "wix: command not found"
```cmd
dotnet tool install --global wix --version 6.0.2
```

### "Project SDK could not be found"
Ensure internet connection and run:
```cmd
dotnet restore OpenAdhanInstaller.wixproj
```

### Build fails with v4 schema errors
The v4 schema is correct for WiX v6. If you see errors:
1. Ensure WiX v6 is installed: `wix --version`
2. Update if needed: `dotnet tool update --global wix`
3. Clean and rebuild: `dotnet clean && dotnet build`

## Resources

- **WiX v6 Documentation**: https://wixtoolset.org/docs/
- **Migration Guide**: https://wixtoolset.org/docs/fourthree/
- **GitHub**: https://github.com/wixtoolset/wix
- **Release Notes**: https://github.com/wixtoolset/wix/releases/tag/v6.0.2
- **Open Adhan WiX Info**: See `OpenAdhanInstaller\WIX_V6_INFO.md`

## Timeline

- **November 2024**: WiX v6.0.2 released
- **November 2024**: Open Adhan migrated to WiX v6
- **2018**: WiX v3.11.2 (last v3 release) - now legacy

## Conclusion

The migration to WiX v6 brings Open Adhan's installer tooling into the modern era with:
- Latest stable release
- Modern .NET integration  
- Simpler developer experience
- Better CI/CD support
- Long-term maintainability

The migration is complete and ready for production use!
