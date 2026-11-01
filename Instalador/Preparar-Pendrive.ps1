<#
.SYNOPSIS
    Arma la carpeta del instalador (para copiar al pendrive) a partir del código del repositorio.

.DESCRIPTION
    Se ejecuta en TU computadora de desarrollo, con internet. Compila el programa en modo Release y deja todo
    ordenado: app\, sql\, herramientas\, prerequisitos\ (vacía: ahí van las dos descargas de Microsoft),
    VERSION.txt y CONTROL.sha256 (para comprobar que nada se corrompió al copiar).

.EXAMPLE
    .\Preparar-Pendrive.ps1 -Salida 'E:\TANO-POS-Instalador'
    .\Preparar-Pendrive.ps1 -Salida "$env:TEMP\kit" -SinCompilar     # usa lo que ya está compilado
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)][string]$Salida,
    [switch]$SinCompilar
)

$ErrorActionPreference = 'Stop'
$Instalador = $PSScriptRoot
$Repo = Split-Path -Parent $Instalador
$ProyectoExe = Join-Path $Repo 'TPFinalNivel2_RuizDiaz\TPFinalNivel2_RuizDiaz\TPFinalNivel2_RuizDiaz.csproj'
$CarpetaRelease = Join-Path $Repo 'TPFinalNivel2_RuizDiaz\TPFinalNivel2_RuizDiaz\bin\Release'
$ScriptSql = Join-Path $Repo 'BaseDeDatos\CATALOGO_DB_RECUPERADA.sql'

function Paso([string]$t) { Write-Host ''; Write-Host ">> $t" -ForegroundColor Cyan }

if (-not (Test-Path $ProyectoExe)) { throw "No encuentro el proyecto: $ProyectoExe" }
if (-not (Test-Path $ScriptSql)) { throw "No encuentro el script de la base: $ScriptSql" }

# ---------------------------------------------------------------- compilar
if (-not $SinCompilar) {
    Paso 'Compilando en modo Release'
    $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
    if (-not (Test-Path $vswhere)) { throw 'No encuentro Visual Studio (vswhere). Compilá en Release desde Visual Studio y usá -SinCompilar.' }
    $msbuild = & $vswhere -latest -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
    if (-not $msbuild) { throw 'No encuentro MSBuild.' }

    # SolutionDir hace falta para restaurar los paquetes (packages.config) al compilar el proyecto solo, sin la solución.
    $carpetaSolucion = (Split-Path -Parent $ProyectoExe) + '\'
    & $msbuild $ProyectoExe -restore -p:RestorePackagesConfig=true -p:Configuration=Release "-p:SolutionDir=$carpetaSolucion" -verbosity:minimal
    if ($LASTEXITCODE -ne 0) { throw 'La compilación falló.' }
}
if (-not (Test-Path (Join-Path $CarpetaRelease 'Punto De Venta.exe'))) { throw "No hay una compilación Release en $CarpetaRelease" }

# ---------------------------------------------------------------- armar
Paso "Armando el instalador en $Salida"
if (Test-Path $Salida) {
    # Se limpia lo generado por una corrida anterior, pero NO la carpeta prerequisitos\ (ahí están las descargas pesadas).
    foreach ($sub in 'app', 'sql', 'herramientas') { $r = Join-Path $Salida $sub; if (Test-Path $r) { Remove-Item $r -Recurse -Force } }
}
foreach ($d in 'app', 'sql', 'herramientas', 'prerequisitos') { [void](New-Item -ItemType Directory -Path (Join-Path $Salida $d) -Force) }

# Programa: todo lo compilado menos archivos de depuración y de publicación ClickOnce
& robocopy $CarpetaRelease (Join-Path $Salida 'app') /E /XF *.pdb *.application *.manifest /XD app.publish /NFL /NDL /NJH /NJS /NP | Out-Null
if ($LASTEXITCODE -ge 8) { throw "No se pudo copiar el programa (robocopy $LASTEXITCODE)." }

Copy-Item $ScriptSql (Join-Path $Salida 'sql') -Force
Copy-Item (Join-Path $Instalador 'INSTALAR.bat') $Salida -Force
Copy-Item (Join-Path $Instalador 'LEEME.md') $Salida -Force -ErrorAction SilentlyContinue
foreach ($s in 'Instalar-TanoPos.ps1', 'Respaldar-TanoPos.ps1') { Copy-Item (Join-Path $Instalador "herramientas\$s") (Join-Path $Salida 'herramientas') -Force }

@'
EN ESTA CARPETA VAN LAS DOS DESCARGAS DE MICROSOFT (se bajan UNA vez, con internet, y quedan en el pendrive):

  1) ndp48-x86-x64-allos-enu.exe
     .NET Framework 4.8, instalador sin conexión ("offline installer").
     Buscar en Microsoft: "Microsoft .NET Framework 4.8 offline installer for Windows" (KB 4503548).
     Windows 10 versión 1903 o más nueva y Windows 11 ya lo traen: en esas computadoras no hace falta.

  2) SQLEXPR_x64_ENU.exe
     SQL Server Express (motor de base de datos), instalador sin conexión.
     En la página de descarga de SQL Server Express elegir "Download Media" (o "Descargar medios") y
     tipo "Express Core". Guardar el archivo SQLEXPR_x64_ENU.exe acá.
     Si la computadora del cliente YA tiene SQL Server Express (instancia SQLEXPRESS) no hace falta.

Los nombres exactos y las versiones pueden cambiar: antes de usarlos conviene probarlos una vez en una
computadora limpia (o una máquina virtual). Ver LEEME.md.
'@ | Set-Content -Path (Join-Path $Salida 'prerequisitos\LEEME-DESCARGAS.txt') -Encoding UTF8

# ---------------------------------------------------------------- versión y control
Paso 'Versión y control de integridad'
$commit = 'sin-git'; $rama = ''
try { $commit = (& git -C $Repo rev-parse --short HEAD).Trim(); $rama = (& git -C $Repo rev-parse --abbrev-ref HEAD).Trim() } catch { }
$version = (Get-Item (Join-Path $Salida 'app\Punto De Venta.exe')).VersionInfo.FileVersion
@("TANO POS", "Versión del programa : $version", "Código (git)         : $commit  $rama", ("Armado el            : {0:yyyy-MM-dd HH:mm}" -f (Get-Date))) |
    Set-Content -Path (Join-Path $Salida 'VERSION.txt') -Encoding UTF8

$lineas = Get-ChildItem -Path $Salida -Recurse -File |
    Where-Object { $_.FullName -notmatch '\\prerequisitos\\' -and $_.Name -ne 'CONTROL.sha256' } |
    ForEach-Object { '{0}  {1}' -f (Get-FileHash $_.FullName -Algorithm SHA256).Hash, $_.FullName.Substring($Salida.TrimEnd('\').Length + 1) }
$lineas | Set-Content -Path (Join-Path $Salida 'CONTROL.sha256') -Encoding UTF8

Write-Host ''
Write-Host "Listo: $Salida" -ForegroundColor Green
Write-Host ("  Programa : {0} archivos" -f (Get-ChildItem (Join-Path $Salida 'app') -Recurse -File).Count)
Write-Host "  Versión  : $version  (código $commit)"
$pre = Get-ChildItem (Join-Path $Salida 'prerequisitos') -File | Where-Object { $_.Extension -eq '.exe' }
if (-not $pre) { Write-Host '  FALTA poner en prerequisitos\ las dos descargas de Microsoft (ver LEEME-DESCARGAS.txt)' -ForegroundColor Yellow }
else { $pre | ForEach-Object { Write-Host ("  Prerrequisito: {0} ({1:N0} MB)" -f $_.Name, ($_.Length / 1MB)) } }
