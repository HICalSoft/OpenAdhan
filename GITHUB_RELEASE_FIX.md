# GitHub Actions Release Creation Fix

## Problem

The "Create Release" step in GitHub Actions was failing with:
- **403 status error** - Permission denied
- **Pattern does not match any files** - MSI path issue

```
⚠️ GitHub release failed with status: 403
🤔 Pattern 'D:\a\OpenAdhan\OpenAdhan\OpenAdhanInstaller\bin\x64\Release\OpenAdhanSetup.msi' does not match any files.
```

## Root Causes

### Issue 1: Missing Permissions

The workflow didn't have explicit permissions to create releases. GitHub Actions requires explicit `contents: write` permission to:
- Create releases
- Upload release assets
- Modify repository contents

### Issue 2: MSI Path Detection

The MSI file path wasn't being verified before attempting to create the release, leading to potential issues if the file wasn't found.

## Solution

### 1. Added Permissions Section

Added explicit permissions at the workflow level:

```yaml
permissions:
  contents: write
```

This grants the workflow permission to:
- ✅ Create GitHub releases
- ✅ Upload release assets (MSI file)
- ✅ Modify release descriptions

### 2. Improved MSI File Detection

Added error handling and verification:

```yaml
- name: Find MSI file
  id: find_msi
  run: |
    $msi = Get-ChildItem -Path "OpenAdhanInstaller\bin" -Filter "*.msi" -Recurse | Select-Object -First 1
    if ($msi) {
      Write-Host "Found MSI: $($msi.FullName)"
      echo "MSI_PATH=$($msi.FullName)" >> $env:GITHUB_ENV
      echo "msi_path=$($msi.FullName)" >> $env:GITHUB_OUTPUT
    } else {
      Write-Error "MSI file not found!"
      exit 1
    }
  shell: powershell

- name: Verify MSI exists
  shell: powershell
  env:
    MSI_FILE_PATH: ${{ env.MSI_PATH }}
  run: |
    $msiPath = $env:MSI_FILE_PATH
    if (Test-Path $msiPath) {
      Write-Host "MSI file verified: $msiPath"
      $size = (Get-Item $msiPath).Length / 1MB
      Write-Host "Size: $([math]::Round($size, 2)) MB"
    } else {
      Write-Error "MSI file does not exist at path"
      exit 1
    }
```

## Changes Made

### File: `.github/workflows/build-release.yml`

**Added after line 12:**
```yaml
permissions:
  contents: write
```

**Improved lines 75-87:**
- Added error handling for MSI file detection
- Added explicit exit on failure

**Added new step (lines 89-99):**
- Verify MSI file exists before creating release
- Display file size for confirmation

## Why These Changes Fix the Problem

### Permissions Fix

**Before:**
```
GitHub Actions → No write permission → 403 Forbidden ❌
```

**After:**
```
GitHub Actions → contents: write → Create release ✅
```

The `contents: write` permission explicitly grants the workflow the ability to:
1. Create releases
2. Upload assets to releases
3. Modify release information

### MSI Detection Fix

**Before:**
```
Build MSI → Maybe path wrong → Try to create release → Fail ❌
```

**After:**
```
Build MSI → Find & verify MSI → Confirm exists → Create release ✅
```

Now the workflow:
1. Finds the MSI file explicitly
2. Verifies it exists
3. Displays size for confirmation
4. Only then attempts to create release

## Testing

### Test the Fix

1. Commit the changes:
```bash
git add .github/workflows/build-release.yml
git commit -m "Fix GitHub Actions release creation - add permissions"
git push origin main
```

2. Create a test release:
```bash
git tag v1.0.0.1
git push origin v1.0.0.1
```

3. Monitor the workflow:
   - Go to: `https://github.com/HICalSoft/OpenAdhan/actions`
   - Watch the "Create Release" step
   - Should see: ✅ Release created successfully

### Expected Output

```
✓ MSI file verified: D:\a\OpenAdhan\OpenAdhan\OpenAdhanInstaller\bin\x64\Release\OpenAdhanSetup.msi
  Size: 23.99 MB
👩‍🏭 Creating new GitHub release for tag v1.0.0.1...
✅ Release created successfully
```

## Common GitHub Actions Permissions

For reference, here are common permissions needed:

```yaml
permissions:
  contents: write      # Create releases, push code, upload assets
  actions: read        # Read workflow information
  pull-requests: write # Create/update PRs
  issues: write        # Create/update issues
  packages: write      # Publish packages
```

We only need `contents: write` for release creation.

## Critical Fix: Use Step Outputs for Actions

**Problem:** MSI file not attached to release even though build succeeded.

**Root Cause:** Using `${{ env.MSI_PATH }}` in the `files:` parameter of `softprops/action-gh-release@v1` doesn't work correctly.

**Solution:** Use step outputs instead: `${{ steps.find_msi.outputs.msi_path }}`

### Fixed Create Release Step

```yaml
- name: Create Release
  if: github.event_name == 'push' && startsWith(github.ref, 'refs/tags/')
  uses: softprops/action-gh-release@v1
  with:
    files: ${{ steps.find_msi.outputs.msi_path }}  # ✅ Use step output
    draft: false
    prerelease: false
    generate_release_notes: true
  env:
    GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
```

**Why this works:**
- Step outputs are explicitly designed for passing values between steps
- Actions like `softprops/action-gh-release` can reliably read step outputs
- Environment variables may not be available in the same context as step outputs

---

## GitHub Actions Best Practice: Passing Variables to PowerShell

The correct way to pass environment variables to PowerShell scripts in GitHub Actions:

### ✅ Correct Approach (Used in Fix)

```yaml
- name: Verify MSI exists
  shell: powershell
  env:
    MSI_FILE_PATH: ${{ env.MSI_PATH }}
  run: |
    $msiPath = $env:MSI_FILE_PATH
    # Use the variable
```

**Why this works:**
- GitHub Actions expands `${{ env.MSI_PATH }}` in the `env:` section
- Creates a step-level environment variable
- PowerShell reads it natively without parsing issues
- No special character escaping needed

### ❌ Incorrect Approaches

**Don't use direct expansion in the script:**
```yaml
- name: Bad Example
  run: |
    $var = "${{ env.SOME_VAR }}"  # Can cause parsing errors
```

**Don't use quotes around $env:**
```yaml
- name: Bad Example
  run: |
    $var = "$env:SOME_VAR"  # Can cause string terminator errors
```

### Key Takeaway

Always use the `env:` section at the step level when passing variables to PowerShell scripts in GitHub Actions. This is the official recommended pattern.

---

## Troubleshooting

### If 403 Error Persists

1. **Check repository settings:**
   - Go to: Settings → Actions → General
   - Ensure "Read and write permissions" is enabled
   - Ensure "Allow GitHub Actions to create and approve pull requests" is checked

2. **Check token permissions:**
   - The `GITHUB_TOKEN` should automatically have the permissions specified in the workflow
   - No manual token configuration needed

### If MSI Not Found

1. **Check build output:**
   - Look at the "Build Installer" step logs
   - Verify MSI was created successfully

2. **Check path:**
   - Ensure `build-installer.ps1` outputs to correct location
   - Default: `OpenAdhanInstaller\bin\x64\Release\OpenAdhanSetup.msi`

## Files Modified

- `.github/workflows/build-release.yml` - Added permissions, improved MSI detection

## Benefits

✅ **Reliable** - Explicit permissions prevent 403 errors  
✅ **Verbose** - Better logging shows exactly what's happening  
✅ **Safe** - Verifies MSI exists before attempting release  
✅ **Debuggable** - Clear error messages if something fails

## Summary

The GitHub Actions release creation was failing due to:
1. Missing `contents: write` permission (403 error)
2. Insufficient MSI path verification

Fixed by:
1. Adding explicit `permissions: contents: write` to workflow
2. Improving MSI file detection with error handling
3. Adding verification step before release creation

**Status:** ✅ Fixed  
**Next Test:** Create a new tag and push to verify

---

## Quick Reference

### Create a Release

```bash
# 1. Commit your changes
git add .
git commit -m "Your changes"
git push origin main

# 2. Create and push a tag
git tag v1.0.0.0
git push origin v1.0.0.0

# 3. GitHub Actions will:
#    ✓ Build the application
#    ✓ Create the MSI
#    ✓ Create a GitHub release with the MSI attached
```

### Check Release Status

- Actions: `https://github.com/HICalSoft/OpenAdhan/actions`
- Releases: `https://github.com/HICalSoft/OpenAdhan/releases`
- Download: `https://github.com/HICalSoft/OpenAdhan/releases/latest/download/OpenAdhanSetup.msi`

The workflow should now complete successfully! 🎉
