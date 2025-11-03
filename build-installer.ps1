# Build Open Adhan Installer
# This script builds the application and creates the MSI installer using WiX Toolset v6

param(
    [string]$Configuration = "Release",
    [string]$Version = "1.0.0.0"
)

$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Building Open Adhan Installer" -ForegroundColor Cyan
Write-Host "Configuration: $Configuration" -ForegroundColor Cyan
Write-Host "Version: $Version" -ForegroundColor Cyan
Write-Host "WiX Toolset: v6.0.2" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# Find MSBuild
$msbuildPath = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" `
    -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe `
    -prerelease | Select-Object -First 1

if (-not $msbuildPath) {
    Write-Error "MSBuild not found. Please install Visual Studio 2022 or Build Tools."
    exit 1
}

Write-Host "`nFound MSBuild: $msbuildPath" -ForegroundColor Green

# Check if .NET SDK is installed
$dotnetVersion = dotnet --version 2>$null
if ($LASTEXITCODE -ne 0) {
    Write-Error ".NET SDK not found. Please install .NET 6.0 SDK or later from https://dotnet.microsoft.com/download"
    exit 1
}

Write-Host "Found .NET SDK: $dotnetVersion" -ForegroundColor Green

# Check if WiX is installed as dotnet tool
Write-Host "`nChecking WiX Toolset installation..." -ForegroundColor Yellow
$wixInstalled = dotnet tool list -g | Select-String "wix"

if (-not $wixInstalled) {
    Write-Host "WiX Toolset not found. Installing WiX Toolset v6.0.2..." -ForegroundColor Yellow
    dotnet tool install --global wix --version 6.0.2
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to install WiX Toolset. Please install manually: dotnet tool install --global wix --version 6.0.2"
        exit 1
    }
    
    Write-Host "WiX Toolset installed successfully!" -ForegroundColor Green
} else {
    Write-Host "WiX Toolset is already installed" -ForegroundColor Green
}

# Build the main application
Write-Host "`n[1/3] Building OpenAdhanForWindowsX..." -ForegroundColor Yellow

# Backup original AssemblyInfo.cs
$assemblyInfoPath = "OpenAdhanForWindowsX\Properties\AssemblyInfo.cs"
$assemblyInfoBackup = "OpenAdhanForWindowsX\Properties\AssemblyInfo.cs.bak"
Copy-Item $assemblyInfoPath $assemblyInfoBackup -Force

try {
    # Update version in AssemblyInfo.cs
    $assemblyInfo = Get-Content $assemblyInfoPath -Raw
    $assemblyInfo = $assemblyInfo -replace '\[assembly: AssemblyVersion\(".*?"\)\]', "[assembly: AssemblyVersion(`"$Version`")]"
    $assemblyInfo = $assemblyInfo -replace '\[assembly: AssemblyFileVersion\(".*?"\)\]', "[assembly: AssemblyFileVersion(`"$Version`")]"
    Set-Content $assemblyInfoPath -Value $assemblyInfo -NoNewline

    & $msbuildPath "OpenAdhanForWindowsX\OpenAdhanForWindowsX.csproj" `
        /p:Configuration=$Configuration `
        /p:Platform=AnyCPU `
        /t:Rebuild `
        /v:minimal `
        /nologo

    if ($LASTEXITCODE -ne 0) {
        throw "Failed to build OpenAdhanForWindowsX"
    }
}
finally {
    # Restore original AssemblyInfo.cs
    Move-Item $assemblyInfoBackup $assemblyInfoPath -Force
}

# Build the registry setup app
Write-Host "`n[2/3] Building OpenAdhanRegistrySetupApp..." -ForegroundColor Yellow
& $msbuildPath "OpenAdhanRegistrySetupApp\OpenAdhanRegistrySetupApp.csproj" `
    /p:Configuration=$Configuration `
    /p:Platform=AnyCPU `
    /t:Rebuild `
    /v:minimal `
    /nologo

if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to build OpenAdhanRegistrySetupApp"
    exit 1
}

# Build the installer using WiX v6
Write-Host "`n[3/3] Building MSI Installer with WiX v6..." -ForegroundColor Yellow

# Build using dotnet build (WiX SDK project)
& dotnet build "OpenAdhanInstaller\OpenAdhanInstaller.wixproj" `
    -c $Configuration `
    -p:ProductVersion=$Version `
    -v:minimal

if ($LASTEXITCODE -ne 0) {
    Write-Error "WiX build failed"
    exit 1
}

Write-Host "`n========================================" -ForegroundColor Green
Write-Host "Build completed successfully!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host "`nInstaller location:" -ForegroundColor Cyan

# WiX v6 outputs to bin\x64\Release\ or bin\x86\Release\
$msiPaths = @(
    "OpenAdhanInstaller\bin\x64\$Configuration\OpenAdhanSetup.msi",
    "OpenAdhanInstaller\bin\x86\$Configuration\OpenAdhanSetup.msi",
    "OpenAdhanInstaller\bin\$Configuration\OpenAdhanSetup.msi"
)

$found = $false
foreach ($msiPath in $msiPaths) {
    if (Test-Path $msiPath) {
        Write-Host "  $(Resolve-Path $msiPath)" -ForegroundColor White
        $found = $true
        break
    }
}

if (-not $found) {
    # Search for it
    $msiFile = Get-ChildItem -Path "OpenAdhanInstaller\bin" -Filter "*.msi" -Recurse | Select-Object -First 1
    if ($msiFile) {
        Write-Host "  $($msiFile.FullName)" -ForegroundColor White
    } else {
        Write-Host "  MSI file not found in OpenAdhanInstaller\bin\" -ForegroundColor Yellow
    }
}
