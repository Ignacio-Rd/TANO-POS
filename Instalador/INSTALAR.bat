@echo off
rem  TANO POS - Lanzador del instalador.
rem  (Solo ASCII a proposito: cmd.exe no maneja bien tildes en archivos .bat.)
setlocal
cd /d "%~dp0"
title TANO POS - Instalador

rem --- Necesita permisos de administrador: si no los tiene, se vuelve a abrir pidiendolos ---
net session >nul 2>&1
if %errorlevel% neq 0 (
    echo Pidiendo permisos de administrador...
    powershell -NoProfile -Command "Start-Process -FilePath '%~f0' -ArgumentList '%*' -Verb RunAs"
    exit /b
)

powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0herramientas\Instalar-TanoPos.ps1" %*
set RESULTADO=%errorlevel%

echo.
if %RESULTADO% neq 0 (
    echo La instalacion NO termino bien. Lee el mensaje de arriba.
) else (
    echo Listo.
)
pause
exit /b %RESULTADO%
