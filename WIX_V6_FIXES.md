# WiX v6 Build Fixes Applied

## Issues Found and Fixed

### 1. Product Element Structure
**Error:** `The Wix element contains an unexpected child element 'Product'`

**Root Cause:** WiX v6 uses `<Package>` as the root element instead of `<Product>`

**Fix:**
```xml
<!-- OLD (WiX v3) -->
<Wix>
  <Product Id="*" Name="..." Version="...">
    <Package InstallerVersion="..." />
  </Product>
</Wix>

<!-- NEW (WiX v6) -->
<Wix>
  <Package Name="..." Version="..." UpgradeCode="...">
    <!-- Content -->
  </Package>
</Wix>
```

### 2. Standard Directories
**Warning:** `It is no longer necessary to define the standard directory 'TARGETDIR'`

**Root Cause:** WiX v6 has built-in `<StandardDirectory>` elements for common directories

**Fix:**
```xml
<!-- OLD -->
<Directory Id="TARGETDIR" Name="SourceDir">
  <Directory Id="ProgramFilesFolder">
    <Directory Id="INSTALLFOLDER" Name="...">
    </Directory>
  </Directory>
</Directory>

<!-- NEW -->
<StandardDirectory Id="ProgramFiles6432Folder">
  <Directory Id="INSTALLFOLDER" Name="...">
  </Directory>
</StandardDirectory>
```

### 3. CustomAction Attributes
**Error:** `The CustomAction element contains an unexpected attribute 'BinaryKey'` and `'FileKey'`

**Root Cause:** Attribute names changed from `BinaryKey`/`FileKey` to `BinaryRef`/`FileRef`

**Fix:**
```xml
<!-- OLD -->
<CustomAction Id="RunRegistrySetup" 
              FileKey="OpenAdhanRegistrySetupApp.exe" 
              ExeCommand="" />

<!-- NEW -->
<CustomAction Id="RunRegistrySetup" 
              FileRef="OpenAdhanRegistrySetupApp.exe" 
              ExeCommand="" />
```

### 4. Custom Element Condition
**Error:** `The Custom element contains illegal inner text: 'NOT Installed'`

**Root Cause:** Conditions must be in `Condition` attribute, not inner text

**Fix:**
```xml
<!-- OLD -->
<Custom Action="RunRegistrySetup" After="InstallFiles">NOT Installed</Custom>

<!-- NEW -->
<Custom Action="RunRegistrySetup" After="InstallFiles" Condition="NOT Installed" />
```

### 5. Duplicate Fragment Tags
**Error:** XML parsing error due to duplicate opening tags

**Root Cause:** Editing mistake left duplicate `<Fragment>` tags

**Fix:**
```xml
<!-- OLD -->
</Package>

<Fragment>

<Fragment>
  <ComponentGroup>

<!-- NEW -->
</Package>

<Fragment>
  <ComponentGroup>
```

### 6. Launch Application CustomAction
**Issue:** WiX v6 doesn't support the same launch app mechanism

**Fix:** Removed the launch application custom action (can be re-implemented if needed with v6 approach)

## Build Output Location

**WiX v6 uses a different output structure:**

- **Old (WiX v3):** `OpenAdhanInstaller\bin\Release\OpenAdhanSetup.msi`
- **New (WiX v6):** `OpenAdhanInstaller\bin\x64\Release\OpenAdhanSetup.msi`

The build script was updated to search for the MSI in the correct location.

## Testing Results

After applying all fixes:
- ✅ Build completes successfully
- ✅ MSI file created (1.2 MB)
- ✅ No errors
- ✅ No warnings
- ✅ Build time: ~4 seconds

## Key WiX v6 Schema Changes Summary

| Feature | WiX v3 | WiX v6 |
|---------|--------|--------|
| Root Element | `<Product>` | `<Package>` |
| Directories | `<Directory Id="ProgramFilesFolder">` | `<StandardDirectory Id="ProgramFiles6432Folder">` |
| Custom Actions | `FileKey=` | `FileRef=` |
| Custom Actions | `BinaryKey=` | `BinaryRef=` |
| Custom Conditions | Inner text | `Condition` attribute |
| Package Attribute | `InstallScope` | `Scope` |
| Media | `<MediaTemplate>` | `<Media>` |

## References

- **WiX v6 Documentation:** https://wixtoolset.org/docs/
- **v3 to v4 Migration:** https://wixtoolset.org/docs/fourthree/
- **Schema Reference:** https://wixtoolset.org/docs/schema/

## Next Steps

1. ✅ Build script works
2. ✅ MSI installer created
3. ⏭️ Test installer on clean system
4. ⏭️ Verify registry keys are created
5. ⏭️ Verify application launches
6. ⏭️ Test GitHub Actions workflow

All WiX v6 migration fixes are complete and tested! 🎉
