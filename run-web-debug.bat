@echo off
setlocal
pushd "%~dp0"

if not defined GODOT_WEB_EXPORTER set "GODOT_WEB_EXPORTER=%USERPROFILE%\Downloads\Godot_v4.6.3-stable_mono_web_export_win64\Godot_v4.6.3-stable_mono_web_export_win64_console.exe"
if not exist "%GODOT_WEB_EXPORTER%" (
    echo Godot Web export console not found:
    echo   %GODOT_WEB_EXPORTER%
    echo Set GODOT_WEB_EXPORTER to the path of your Godot Web export console and try again.
    popd
    pause
    exit /b 1
)

if not exist "builds\web" mkdir "builds\web"

:run
echo Building Survivors.csproj...
dotnet build Survivors.csproj
if errorlevel 1 goto build_failed

echo Exporting Web build...
"%GODOT_WEB_EXPORTER%" --headless --editor --path "%~dp0." --export-debug "Web" "%~dp0builds\web\index.html"
if errorlevel 1 goto export_failed

echo.
echo Build and export complete. Starting the Web server and browser console...
powershell.exe -NoLogo -ExecutionPolicy Bypass -File "%~dp0serve-web.ps1" -Root "%~dp0builds\web"
echo.
set "again="
set /p "again=Run the build, export, and server again? Enter y to continue: "
if /i "%again%"=="y" goto run
popd
exit /b %errorlevel%

:build_failed
echo.
echo Build failed. See the errors above.
popd
pause
exit /b 1

:export_failed
echo.
echo Web export failed. See the errors above.
popd
pause
exit /b 1
