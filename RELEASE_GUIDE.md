# Release Guide - Creating Official Releases

## Overview

The Open Adhan project uses GitHub Actions to automatically build and release the installer. There are **two ways** to create a release:

1. **Automatic Release** (Recommended) - Push a git tag
2. **Manual Build** - Run workflow manually via GitHub UI

---

## Method 1: Automatic Release (Recommended) ⭐

This method automatically builds, creates a GitHub release, and uploads the installer.

### Step-by-Step Instructions

#### 1. Commit and Push Your Changes

```bash
git add .
git commit -m "Your commit message"
git push origin main
```

#### 2. Create and Push a Version Tag

The tag **must start with `v`** followed by the version number in format `Major.Minor.Patch.Build`

```bash
# Create tag
git tag v1.0.0.0

# Push tag to GitHub
git push origin v1.0.0.0
```

**Tag Format Examples:**
```bash
git tag v1.0.0.0     # First release
git tag v1.1.0.0     # Minor update  
git tag v2.0.0.0     # Major release
git tag v1.0.1.0     # Patch/bug fix
git tag v1.2.3.4     # Full version number
```

#### 3. Watch the Build Process

1. Go to: `https://github.com/HICalSoft/OpenAdhan/actions`
2. You'll see the "Build and Release" workflow running
3. Click on it to watch progress
4. Build takes approximately 5-10 minutes

#### 4. Release is Created Automatically ✅

Once the workflow completes successfully:
- ✅ A new GitHub Release is created
- ✅ Release title matches your tag (e.g., `v1.0.0.0`)
- ✅ Release notes are auto-generated from commits
- ✅ The MSI installer is attached to the release
- ✅ Release is published (not draft)
- ✅ Users can download immediately

#### 5. View Your Release

Go to: `https://github.com/HICalSoft/OpenAdhan/releases`

You'll see:
```
v1.0.0.0
Latest • 2 minutes ago

What's Changed
• Add minimize button feature
• Fix uninstall issue with audio files  
• Update to WiX v6

Assets
📦 OpenAdhanSetup.msi (23.99 MB)
```

---

## Method 2: Manual Build (No Release)

Use this if you want to build an installer **without** creating a public GitHub release. Good for testing.

### Step-by-Step Instructions

#### 1. Go to GitHub Actions

Navigate to: `https://github.com/HICalSoft/OpenAdhan/actions`

#### 2. Select the Workflow

Click on **"Build and Release"** workflow in the left sidebar

#### 3. Run Workflow Manually

1. Click the **"Run workflow"** dropdown button (top right)
2. Keep branch as `main` (or select another branch)
3. Enter **version number** (e.g., `1.0.0.0`)
4. Click **"Run workflow"**

#### 4. Wait for Build to Complete

The workflow will:
- Build the application
- Create the MSI installer
- Upload it as an artifact (NO release created)

#### 5. Download the Installer

1. Click on the completed workflow run
2. Scroll down to **"Artifacts"** section
3. Download: `OpenAdhanSetup-1.0.0.0.zip`
4. Extract the ZIP to get `OpenAdhanSetup.msi`

**Note:** Artifacts are kept for 90 days, then automatically deleted.

---

## Version Numbering Guide

### Format: `Major.Minor.Patch.Build`

```
v1.2.3.4
  │ │ │ └─ Build number (usually 0)
  │ │ └─── Patch (bug fixes only)
  │ └───── Minor (new features, backwards compatible)
  └─────── Major (breaking changes)
```

### When to Increment Each Number

| Version Change | When to Use | Example |
|---------------|-------------|---------|
| Major (v**2**.0.0.0) | Breaking changes, major redesign | v1.0.0.0 → v2.0.0.0 |
| Minor (v1.**1**.0.0) | New features, backwards compatible | v1.0.0.0 → v1.1.0.0 |
| Patch (v1.0.**1**.0) | Bug fixes, no new features | v1.0.0.0 → v1.0.1.0 |
| Build (v1.0.0.**1**) | Automated builds (usually 0) | v1.0.0.0 → v1.0.0.1 |

### Real-World Examples

```bash
v1.0.0.0    # Initial release
v1.0.1.0    # Fixed audio file uninstall bug
v1.1.0.0    # Added minimize button feature
v1.2.0.0    # Added dark mode
v2.0.0.0    # Complete UI redesign
```

---

## Complete Example: Creating v1.0.0.0

Let's walk through creating your first official release.

### Commands

```bash
# 1. Make sure you're on main branch
git checkout main
git pull

# 2. Check for uncommitted changes
git status

# 3. Commit any pending changes
git add .
git commit -m "Prepare for v1.0.0.0 release"

# 4. Push to GitHub
git push origin main

# 5. Create the version tag
git tag v1.0.0.0

# 6. Push the tag (this triggers the release)
git push origin v1.0.0.0
```

### What Happens Next

```
GitHub detects the tag push
       ↓
GitHub Actions workflow starts
       ↓
Sets up Windows environment
       ↓
Installs MSBuild, .NET SDK, WiX v6
       ↓
Builds OpenAdhanForWindows.exe
       ↓
Builds OpenAdhanSetup.msi (with version 1.0.0.0)
       ↓
Creates GitHub Release "v1.0.0.0"
       ↓
Uploads MSI to the release
       ↓
Publishes the release
       ↓
✅ Done! Users can download
```

### Verify the Release

1. Go to: `https://github.com/HICalSoft/OpenAdhan/releases`
2. You should see "v1.0.0.0" at the top
3. Click on it to view details
4. Download and test the MSI

---

## What Happens During the Build?

The GitHub Actions workflow (`build-release.yml`) performs these steps:

```
Step 1: Checkout code
  ✓ Clones your repository

Step 2: Setup MSBuild  
  ✓ Installs Visual Studio build tools

Step 3: Setup NuGet
  ✓ Installs NuGet package manager

Step 4: Restore NuGet packages
  ✓ Downloads all dependencies

Step 5: Setup .NET SDK
  ✓ Installs .NET 8.0 SDK

Step 6: Install WiX Toolset v6.0.2
  ✓ Installs WiX globally via dotnet tool

Step 7: Determine version
  ✓ Extracts version from tag (v1.0.0.0 → 1.0.0.0)

Step 8: Build Solution
  ✓ Compiles OpenAdhanForWindows.exe (Release mode)

Step 9: Build Installer  
  ✓ Creates OpenAdhanSetup.msi with the version

Step 10: Upload Artifact
  ✓ Saves MSI for download

Step 11: Find MSI file
  ✓ Locates the MSI in bin directory

Step 12: Create Release (if tag push)
  ✓ Creates GitHub Release
  ✓ Generates release notes
  ✓ Uploads MSI file
  ✓ Publishes release
```

**Total Time:** 5-10 minutes

---

## Updating Version Numbers

The version number appears in several places. Here's what gets updated:

### 1. MSI Package Version ✅ (Automatic)

When you run `build-installer.ps1 -Version "1.0.0.0"`, it updates:
```xml
<!-- Product.wxs -->
<Package Version="1.0.0.0" ... />
```

### 2. Assembly Version (Manual)

If you want the EXE file to show the version:

**Using Visual Studio:**
1. Right-click `OpenAdhanForWindowsX` project
2. Properties → Application → Assembly Information
3. Update:
   - Assembly version: `1.0.0.0`
   - File version: `1.0.0.0`

**Or edit directly:**
```csharp
// Properties\AssemblyInfo.cs
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
```

### 3. Default Version in Product.wxs (Optional)

Edit the default if you want:
```xml
<!-- OpenAdhanInstaller\Product.wxs -->
<Package Name="Open Adhan for Windows" 
         Version="1.0.0.0"    <!-- Change default here -->
         ... />
```

---

## Troubleshooting

### Issue: Tag Already Exists

```bash
# Error when trying to create duplicate tag
error: tag 'v1.0.0.0' already exists

# Solution: Delete the old tag
git tag -d v1.0.0.0                    # Delete locally
git push origin :refs/tags/v1.0.0.0    # Delete on GitHub

# Then create the new tag
git tag v1.0.0.0
git push origin v1.0.0.0
```

### Issue: Build Failed

**Symptoms:** Red X on the workflow run

**Steps to Debug:**
1. Go to the Actions tab
2. Click on the failed workflow run
3. Expand each step to find errors
4. Look for red text with error messages

**Common Causes:**
- Missing file in source
- Build errors in C# code
- WiX compilation errors
- File path issues

**Solution:**
1. Fix the issue locally
2. Commit and push the fix
3. Delete and recreate the tag
4. Push the tag again

### Issue: Release Not Created

**Symptoms:** Build succeeds but no release appears

**Checklist:**
- ☑ Does tag start with `v`? (e.g., `v1.0.0.0`)
- ☑ Was tag pushed to GitHub? (`git push origin v1.0.0.0`)
- ☑ Did workflow complete successfully?
- ☑ Are you looking at the right repository?

**Check Workflow:**
```
Step "Create Release" should show:
✓ Create Release
  Published release v1.0.0.0
```

### Issue: Wrong Version in MSI

**Problem:** MSI shows different version than expected

**Cause:** Version might be cached in Product.wxs

**Solution:**
```bash
# Clean the build
Remove-Item OpenAdhanInstaller\bin -Recurse -Force
Remove-Item OpenAdhanInstaller\obj -Recurse -Force

# Rebuild with specific version
.\build-installer.ps1 -Configuration Release -Version "1.0.0.0"
```

---

## Testing Before Official Release

### Local Testing

```bash
# 1. Build locally with test version
.\build-installer.ps1 -Configuration Release -Version "0.9.9.9"

# 2. Install and test
.\OpenAdhanInstaller\bin\x64\Release\OpenAdhanSetup.msi

# 3. Test all features:
- Installation to C:\Program Files\OpenAdhan
- Application launches
- Prayer times display correctly
- Audio files play
- Minimize button works
- Auto-start on boot
- Uninstallation removes all files

# 4. If everything works, create official release
git tag v1.0.0.0
git push origin v1.0.0.0
```

### Test Release Without Publishing

```bash
# Use manual workflow (Method 2)
# This builds but doesn't create a public release
# Download artifact and test thoroughly
# Then create official tag when ready
```

---

## Deleting a Release

If you need to delete a release:

### On GitHub Web Interface

1. Go to: `https://github.com/HICalSoft/OpenAdhan/releases`
2. Click on the release you want to delete
3. Click "Delete" button (top right)
4. Confirm deletion

### Delete the Git Tag

```bash
# Delete local tag
git tag -d v1.0.0.0

# Delete remote tag
git push origin :refs/tags/v1.0.0.0

# Or use this alternative syntax
git push origin --delete v1.0.0.0
```

**Note:** Deleting a release doesn't delete the tag automatically. You must delete both.

---

## Best Practices

### 1. Always Test Locally First

```bash
# Build and test the installer before tagging
.\build-installer.bat
# Install the MSI and verify everything works
```

### 2. Use Meaningful Commit Messages

```bash
# Good
git commit -m "Add minimize button with Win+Down shortcut"
git commit -m "Fix: Bismillah.wav not removed on uninstall"
git commit -m "Update to WiX Toolset v6.0.2"

# Bad
git commit -m "Fixed stuff"
git commit -m "Update"
git commit -m "Changes"
```

### 3. Follow Semantic Versioning

```
v1.0.0.0 → v1.0.1.0  (bug fix)
v1.0.0.0 → v1.1.0.0  (new feature)
v1.0.0.0 → v2.0.0.0  (breaking change)
```

### 4. Always Tag from Main Branch

```bash
# Ensure you're on main
git checkout main
git pull origin main

# Then create tag
git tag v1.0.0.0
git push origin v1.0.0.0
```

### 5. One Release Per Tag

Don't reuse tags. Each tag should be a permanent marker of a specific release.

### 6. Write Release Notes

After GitHub creates the release, edit it to add:
- Summary of changes
- New features
- Bug fixes
- Known issues
- Installation instructions

---

## Quick Command Reference

### Create New Release
```bash
git tag v1.0.0.0 && git push origin v1.0.0.0
```

### List All Tags
```bash
git tag -l
```

### Delete Tag Locally and Remotely
```bash
git tag -d v1.0.0.0
git push origin :refs/tags/v1.0.0.0
```

### View Latest Tag
```bash
git describe --tags --abbrev=0
```

### Create Tag from Specific Commit
```bash
git tag v1.0.0.0 abc1234
git push origin v1.0.0.0
```

---

## Useful Links

### Your Repository Links
```
Releases:
https://github.com/HICalSoft/OpenAdhan/releases

Actions (Build Status):
https://github.com/HICalSoft/OpenAdhan/actions

Tags:
https://github.com/HICalSoft/OpenAdhan/tags

Latest Release:
https://github.com/HICalSoft/OpenAdhan/releases/latest
```

### Download Links for Users
```
Latest Installer:
https://github.com/HICalSoft/OpenAdhan/releases/latest/download/OpenAdhanSetup.msi

Specific Version:
https://github.com/HICalSoft/OpenAdhan/releases/download/v1.0.0.0/OpenAdhanSetup.msi
```

---

## After Creating a Release

### 1. Announce the Release

- Update README.md with download link
- Post on social media
- Email notification list
- Update project website

### 2. Monitor for Issues

- Check GitHub Issues
- Watch for user feedback
- Monitor crash reports

### 3. Plan Next Release

- Gather feature requests
- Create GitHub Issues for bugs
- Set milestones for next version

---

## Summary

### To Create an Official Release:

```bash
# 1. Commit your changes
git add .
git commit -m "Prepare release"
git push origin main

# 2. Create and push tag
git tag v1.0.0.0
git push origin v1.0.0.0

# 3. Wait for GitHub Actions (5-10 minutes)

# 4. Download from releases page
# https://github.com/HICalSoft/OpenAdhan/releases
```

**That's it!** The entire process is automated. 🎉

---

## Example: Real Release Workflow

```bash
# Monday: Add new feature
git checkout -b feature/dark-mode
# ... code changes ...
git commit -m "Add dark mode support"
git push origin feature/dark-mode

# Create pull request, review, merge to main

# Tuesday: Fix bug found in testing  
git checkout -b fix/audio-bug
# ... fix code ...
git commit -m "Fix: Audio not playing on some systems"
git push origin fix/audio-bug

# Create pull request, review, merge to main

# Wednesday: Prepare release
git checkout main
git pull origin main

# Update version in docs if needed
# Test locally
.\build-installer.bat
# Install and test MSI

# Thursday: Create official release
git tag v1.1.0.0
git push origin v1.1.0.0

# GitHub Actions builds and publishes
# Monitor the build
# Test the release download
# Announce to users

# Friday: Monitor for issues
# Check GitHub Issues
# Respond to user feedback
```

Done! 🚀
