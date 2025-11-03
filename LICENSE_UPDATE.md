# License Update Summary

## Changes Made

The Open Adhan for Windows project license has been updated to use the **MIT License** consistently across all files.

## What Was Changed

### 1. Main LICENSE File
**Before:** GNU General Public License v3 (GPL v3) - 674 lines  
**After:** MIT License - 22 lines

**Location:** `LICENSE` (root directory)

### 2. Installer License
**Before:** MIT License (already correct, but no copyright info)  
**After:** MIT License with proper copyright notice and formatting

**Location:** `OpenAdhanInstaller\license.rtf`

### Changes to installer license:
- Added copyright year and holder: "Copyright (c) 2025 Open Adhan Contributors"
- Improved formatting with bold text for title and warranty disclaimer
- Larger font for title
- Better visual presentation
- **Added `WixUILicenseRtf` variable in Product.wxs to override default Lorem ipsum license**

## MIT License

The MIT License is one of the most permissive open-source licenses. It allows users to:

✅ **Use** - Use the software for any purpose  
✅ **Copy** - Make copies of the software  
✅ **Modify** - Change the software as needed  
✅ **Merge** - Combine with other software  
✅ **Publish** - Distribute the software  
✅ **Sublicense** - Grant these rights to others  
✅ **Sell** - Sell copies of the software  

### Requirements:
- Include the copyright notice and license text in copies
- That's it! Very simple.

### No Warranty:
The software is provided "AS IS" without any warranty. The authors are not liable for any damages.

## Why MIT?

1. **Simple & Clear** - Easy to understand, no complex legal language
2. **Very Permissive** - Allows commercial and private use
3. **No Copyleft** - Can be combined with proprietary software
4. **Industry Standard** - Widely used and accepted
5. **Business Friendly** - Companies can use and integrate the software

## Comparison

### GPL v3 (Old)
- ❌ Copyleft - Derivative works must also be GPL
- ❌ Complex - 674 lines of legal text
- ❌ Restrictive - Many conditions and requirements
- ❌ Less business-friendly

### MIT (New)
- ✅ Permissive - Few restrictions
- ✅ Simple - 22 lines total
- ✅ Flexible - Can be used in any project
- ✅ Business-friendly

## Verification

### Build Test ✅
- Clean build performed after license update
- Build succeeded with no errors or warnings
- MSI installer created successfully
- License agreement will show in installer

### Files Updated
1. ✅ `LICENSE` - Main project license (root)
2. ✅ `OpenAdhanInstaller\license.rtf` - Installer license dialog
3. ✅ `OpenAdhanInstaller\Product.wxs` - Added WixUILicenseRtf variable to use custom license

## License Text

```
MIT License

Copyright (c) 2025 Open Adhan Contributors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

## User Impact

### For End Users
- Software remains free to use
- No restrictions on usage
- Clear understanding of no warranty
- Can use for personal or commercial purposes

### For Developers
- Can fork and modify the code
- Can use in proprietary projects
- Can sell modified versions
- Must include copyright notice

### For Contributors
- Contributions will be under MIT License
- Clear legal framework for contributions
- No complex licensing requirements

## Documentation

The MIT License is also mentioned in:
- `README_INSTALLER.md` - Installation documentation
- `CHANGES_SUMMARY.md` - Project changes summary

All documentation is consistent with the MIT License.

## Summary

The Open Adhan for Windows project now uses the **MIT License** - a simple, permissive, and widely-accepted open-source license that allows maximum freedom while providing appropriate disclaimers of warranty and liability.

**No more GPL v3 complexity!** ✨  
**No Lorem ipsum placeholder text!** ✅  
**Clean, professional, permissive license!** 🎉
