@echo off
REM Build Open Adhan Installer
REM Usage: build-installer.bat [Configuration] [Version]
REM Example: build-installer.bat Release 1.0.0.0

setlocal

set CONFIG=%1
if "%CONFIG%"=="" set CONFIG=Release

set VERSION=%2
if "%VERSION%"=="" set VERSION=1.0.0.0

echo Building Open Adhan Installer...
echo Configuration: %CONFIG%
echo Version: %VERSION%

powershell.exe -ExecutionPolicy Bypass -File "%~dp0build-installer.ps1" -Configuration %CONFIG% -Version %VERSION%

if %ERRORLEVEL% neq 0 (
    echo.
    echo Build failed!
    exit /b 1
)

echo.
echo Build completed successfully!
exit /b 0
