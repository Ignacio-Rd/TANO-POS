<#
.SYNOPSIS
    Instala TANO POS desde el pendrive, SIN internet: prerrequisitos, base de datos, programa, accesos directos y respaldo diario.

.DESCRIPTION
    Se ejecuta desde INSTALAR.bat (que pide permisos de administrador). Se puede volver a correr sin riesgo:
    actualiza el programa y NO toca los datos de la base si ya existe.

.EXAMPLE
    .\Instalar-TanoPos.ps1 -SoloVerificar        # solo muestra qué falta, no cambia nada
    .\Instalar-TanoPos.ps1 -AutoInicio            # además arranca el sistema al iniciar sesión en Windows
#>
[CmdletBinding()]
param(
    [string]$Destino = 'C:\TANO POS',
    [string]$Instancia = 'SQLEXPRESS',
    [string]$BaseDeDatos = 'CATALOGO_DB_RECUPERADA',
    # Cuenta de Windows que usa el sistema para conectarse a la base (permisos mínimos: leer y escribir datos).
    # Vacío = el grupo local de Usuarios, con el nombre que tenga en el idioma de Windows (en español: BUILTIN\Usuarios).
    [string]$CuentaApp = '',
    [string]$CarpetaEscritorio = '',
    [string]$HoraRespaldo = '04:00',
    [int]$DiasRespaldos = 30,
    [string]$CopiaExterna = '',
    [switch]$AutoInicio,
    [switch]$SinPrerequisitos,
    [switch]$SinTareaRespaldo,
    [switch]$SoloVerificar,
    [switch]$NoRequerirAdmin   # solo para pruebas del instalador
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version 2

$Raiz = Split-Path -Parent $PSScriptRoot          # carpeta del pendrive (la que tiene app\, sql\, prerequisitos\)
$Servidor = if ($Instancia -eq 'MSSQLSERVER') { '.' } else { ".\$Instancia" }
$NombreServicioSql = if ($Instancia -eq 'MSSQLSERVER') { 'MSSQLSERVER' } else { "MSSQL`$$Instancia" }
# Los grupos de Windows se llaman distinto según el idioma (Users / Usuarios, Administrators / Administradores):
# se buscan por su identificador universal (SID), nunca por el nombre en inglés.
function Nombre-Windows([string]$sid) { return (New-Object Security.Principal.SecurityIdentifier($sid)).Translate([Security.Principal.NTAccount]).Value }
if (-not $CuentaApp) { $CuentaApp = Nombre-Windows 'S-1-5-32-545' }
$GrupoAdmins = Nombre-Windows 'S-1-5-32-544'
$CuentaSistema = Nombre-Windows 'S-1-5-18'
$Exe = 'Punto De Venta.exe'
$NombreAcceso = 'TANO POS'
$ScriptSql = Join-Path $Raiz 'sql\CATALOGO_DB_RECUPERADA.sql'

# ------------------------------------------------------------------ utilidades

function Paso([string]$texto) { Write-Host ''; Write-Host ">> $texto" -ForegroundColor Cyan }
function Ok([string]$texto) { Write-Host "   [OK] $texto" -ForegroundColor Green }
function Aviso([string]$texto) { Write-Host "   [!]  $texto" -ForegroundColor Yellow }
function Fallar([string]$texto) { throw $texto }

function Validar-Nombre([string]$valor, [string]$que) {
    # Los nombres se usan dentro de sentencias SQL entre corchetes: solo se aceptan caracteres normales.
    if ($valor -notmatch '^[\w\\\$\. -]+$') { Fallar "El valor de $que contiene caracteres no permitidos: '$valor'" }
}

function Nueva-Conexion([string]$bd = 'master') {
    $cn = New-Object System.Data.SqlClient.SqlConnection("Server=$Servidor;Database=$bd;Integrated Security=true;Connect Timeout=15")
    $cn.Open()
    return $cn
}

# Ejecuta un script con varias tandas separadas por GO (como hace sqlcmd), en UNA sola conexión.
function Ejecutar-Sql([string]$sql) {
    $cn = Nueva-Conexion
    try {
        foreach ($tanda in ($sql -split '(?im)^\s*GO\s*$')) {
            if ($tanda.Trim().Length -eq 0) { continue }
            $cmd = $cn.CreateCommand()
            $cmd.CommandText = $tanda
            $cmd.CommandTimeout = 180
            [void]$cmd.ExecuteNonQuery()
        }
    }
    finally { $cn.Dispose() }
}

function Consultar-Escalar([string]$sql, [string]$bd = 'master') {
    $cn = Nueva-Conexion $bd
    try { $cmd = $cn.CreateCommand(); $cmd.CommandText = $sql; return $cmd.ExecuteScalar() }
    finally { $cn.Dispose() }
}

function Es-Administrador {
    $id = [Security.Principal.WindowsIdentity]::GetCurrent()
    return (New-Object Security.Principal.WindowsPrincipal($id)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
}

function Release-NetFramework {
    $clave = 'HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full'
    $valor = Get-ItemProperty -Path $clave -ErrorAction SilentlyContinue
    if ($valor -and $valor.PSObject.Properties['Release']) { return [int]$valor.Release }
    return 0
}

function Servicio-Sql { return Get-Service -Name $NombreServicioSql -ErrorAction SilentlyContinue }

# ------------------------------------------------------------------ verificación

function Mostrar-Estado {
    Paso 'Estado de esta computadora'
    $os = [Environment]::OSVersion.Version
    Write-Host ("   Windows {0}.{1} compilación {2} ({3} bits)" -f $os.Major, $os.Minor, $os.Build, $(if ([Environment]::Is64BitOperatingSystem) { 64 } else { 32 }))
    Write-Host ("   Permisos de administrador : {0}" -f $(if (Es-Administrador) { 'sí' } else { 'NO' }))
    $rel = Release-NetFramework
    Write-Host ("   .NET Framework 4.8        : {0}" -f $(if ($rel -ge 528040) { 'instalado' } else { 'FALTA' }))
    $svc = Servicio-Sql
    Write-Host ("   SQL Server ($Instancia)    : {0}" -f $(if ($svc) { "instalado ($($svc.Status))" } else { 'FALTA' }))
    Write-Host ("   Programa en $Destino : {0}" -f $(if (Test-Path (Join-Path $Destino "App\$Exe")) { 'ya instalado (se actualizará)' } else { 'no instalado' }))
    Write-Host ("   Archivos del pendrive     : app={0}  sql={1}" -f (Test-Path (Join-Path $Raiz "app\$Exe")), (Test-Path $ScriptSql))
}

# ------------------------------------------------------------------ pasos

function Comprobar-Requisitos {
    Paso '1/9  Comprobando la computadora y el pendrive'
    if (-not $NoRequerirAdmin -and -not (Es-Administrador)) { Fallar 'Hace falta ejecutar como administrador. Usá INSTALAR.bat (te lo pide solo).' }
    if ([Environment]::OSVersion.Version.Build -lt 14393) { Fallar 'Se necesita Windows 10 (versión 1607 o más nueva) u 11.' }
    if (-not [Environment]::Is64BitOperatingSystem) { Fallar 'Se necesita Windows de 64 bits.' }
    if (-not (Test-Path (Join-Path $Raiz "app\$Exe"))) { Fallar "No encuentro app\$Exe en el pendrive. ¿Copiaste la carpeta completa?" }
    if (-not (Test-Path $ScriptSql)) { Fallar 'No encuentro sql\CATALOGO_DB_RECUPERADA.sql en el pendrive.' }
    Validar-Nombre $BaseDeDatos 'BaseDeDatos'; Validar-Nombre $Instancia 'Instancia'; Validar-Nombre $CuentaApp 'CuentaApp'
    Ok 'Windows y archivos del pendrive correctos'
}

function Instalar-NetFramework {
    Paso '2/9  .NET Framework 4.8'
    if ((Release-NetFramework) -ge 528040) { Ok 'Ya está instalado'; return }
    if ($SinPrerequisitos) { Fallar 'Falta .NET Framework 4.8 (y se pidió no instalar prerrequisitos).' }

    $instalador = Get-ChildItem -Path (Join-Path $Raiz 'prerequisitos') -Filter 'ndp48*.exe' -ErrorAction SilentlyContinue | Select-Object -First 1
    if (-not $instalador) { Fallar "Falta .NET Framework 4.8 y no está el instalador en prerequisitos\ (ndp48-x86-x64-allos-enu.exe). Ver LEEME." }

    Write-Host "   Instalando $($instalador.Name) (puede tardar unos minutos)..."
    $p = Start-Process -FilePath $instalador.FullName -ArgumentList '/q /norestart' -Wait -PassThru
    if ($p.ExitCode -eq 3010 -or $p.ExitCode -eq 1641) {
        Aviso 'Se instaló, pero Windows necesita REINICIAR. Reiniciá la computadora y volvé a ejecutar INSTALAR.bat.'
        throw 'REINICIO_NECESARIO'
    }
    if ($p.ExitCode -ne 0) { Fallar "El instalador de .NET Framework terminó con el código $($p.ExitCode)." }
    Ok '.NET Framework 4.8 instalado'
}

function Instalar-SqlServer {
    Paso "3/9  SQL Server Express (instancia $Instancia)"
    $svc = Servicio-Sql
    if (-not $svc) {
        if ($SinPrerequisitos) { Fallar "No existe la instancia $Instancia de SQL Server (y se pidió no instalar prerrequisitos)." }

        $carpetaPre = Join-Path $Raiz 'prerequisitos'
        $setup = Get-ChildItem -Path $carpetaPre -Filter 'setup.exe' -Recurse -ErrorAction SilentlyContinue | Select-Object -First 1
        $autoextraible = Get-ChildItem -Path $carpetaPre -Filter 'SQLEXPR*.exe' -ErrorAction SilentlyContinue | Select-Object -First 1
        if (-not $setup -and -not $autoextraible) { Fallar "No hay instalador de SQL Server Express en prerequisitos\ (SQLEXPR_x64_ENU.exe). Ver LEEME." }

        $parametros = @(
            '/Q', '/ACTION=Install', '/IACCEPTSQLSERVERLICENSETERMS',
            '/FEATURES=SQLENGINE', "/INSTANCENAME=$Instancia",
            "/SQLSYSADMINACCOUNTS=`"$GrupoAdmins`"",   # los administradores de Windows administran la base
            '/TCPENABLED=0', '/NPENABLED=0',                    # solo memoria compartida: la base NO queda expuesta en la red
            '/UPDATEENABLED=0', '/SKIPRULES=RebootRequiredCheck'
        ) -join ' '

        $ejecutable = if ($setup) { $setup.FullName } else { $autoextraible.FullName }
        Write-Host "   Instalando SQL Server Express desde $(Split-Path $ejecutable -Leaf) (10 a 20 minutos, no cierres la ventana)..."
        $p = Start-Process -FilePath $ejecutable -ArgumentList $parametros -Wait -PassThru
        if ($p.ExitCode -eq 3010) { Aviso 'SQL Server pide reiniciar Windows al terminar; si algo falla más adelante, reiniciá y volvé a ejecutar el instalador.' }
        elseif ($p.ExitCode -ne 0) { Fallar "La instalación de SQL Server terminó con el código $($p.ExitCode). Detalle en: C:\Program Files\Microsoft SQL Server\*\Setup Bootstrap\Log\Summary.txt" }

        $svc = Servicio-Sql
        if (-not $svc) { Fallar "SQL Server dijo que terminó pero no aparece el servicio $NombreServicioSql." }
    }

    if ($svc.Status -ne 'Running') { Start-Service -Name $NombreServicioSql; $svc.WaitForStatus('Running', [TimeSpan]::FromSeconds(60)) }

    # Esperar a que acepte conexiones
    $listo = $false
    for ($i = 0; $i -lt 30 -and -not $listo; $i++) {
        try { [void](Consultar-Escalar 'SELECT 1'); $listo = $true } catch { Start-Sleep -Seconds 2 }
    }
    if (-not $listo) { Fallar "SQL Server ($Servidor) no responde. Revisá que el servicio $NombreServicioSql esté iniciado." }

    # Que arranque solo al prender la computadora (importante después de un corte de luz)
    try { Set-Service -Name $NombreServicioSql -StartupType Automatic }
    catch { Aviso 'No se pudo dejar SQL Server en inicio automático (hace falta administrador). Hacelo desde Servicios de Windows.' }
    Ok "SQL Server $Instancia funcionando"
}

function Crear-BaseDeDatos {
    Paso "4/9  Base de datos '$BaseDeDatos'"
    $idBase = Consultar-Escalar "SELECT DB_ID(N'$BaseDeDatos')"
    $existia = ($null -ne $idBase) -and ($idBase -isnot [DBNull])

    $sql = [IO.File]::ReadAllText($ScriptSql, [Text.Encoding]::UTF8)
    if ($BaseDeDatos -ne 'CATALOGO_DB_RECUPERADA') { $sql = $sql.Replace('CATALOGO_DB_RECUPERADA', $BaseDeDatos) }
    Ejecutar-Sql $sql      # el script no toca lo que ya existe: es seguro en reinstalaciones y actualizaciones

    if ($existia) { Ok 'La base ya existía: se verificó la estructura y NO se tocaron los datos' }
    else { Ok 'Base creada con sus tablas y las formas de pago iniciales' }
}

function Configurar-Permisos {
    Paso "5/9  Permisos de la base (cuenta: $CuentaApp)"
    # La aplicación solo lee y escribe datos (no crea ni borra tablas): se le dan permisos mínimos.
    $sql = @"
IF SUSER_ID(N'$CuentaApp') IS NULL CREATE LOGIN [$CuentaApp] FROM WINDOWS;
GO
USE [$BaseDeDatos];
DECLARE @usuario sysname = (SELECT name FROM sys.database_principals WHERE sid = SUSER_SID(N'$CuentaApp'));
IF @usuario IS NULL BEGIN CREATE USER [$CuentaApp] FOR LOGIN [$CuentaApp]; SET @usuario = N'$CuentaApp'; END
IF @usuario <> N'dbo'
BEGIN
    DECLARE @orden nvarchar(max) = N'ALTER ROLE db_datareader ADD MEMBER ' + QUOTENAME(@usuario) + N'; ALTER ROLE db_datawriter ADD MEMBER ' + QUOTENAME(@usuario) + N';';
    EXEC (@orden);
END
GO
"@
    Ejecutar-Sql $sql
    Ok "$CuentaApp puede leer y escribir datos (sin permisos de administración)"
}

function Copiar-Programa {
    Paso "6/9  Copiando el programa a $Destino"
    $proceso = Get-Process -Name ([IO.Path]::GetFileNameWithoutExtension($Exe)) -ErrorAction SilentlyContinue
    if ($proceso) { Fallar 'TANO POS está abierto. Cerralo y volvé a ejecutar el instalador.' }

    $carpetaApp = Join-Path $Destino 'App'
    foreach ($d in @($Destino, $carpetaApp, (Join-Path $Destino 'Herramientas'), (Join-Path $Destino 'Respaldos'), (Join-Path $Destino 'Logs'))) {
        if (-not (Test-Path $d)) { [void](New-Item -ItemType Directory -Path $d -Force) }
    }

    # /XF conserva el log de errores que ya exista; no se copian archivos de depuración
    $salida = & robocopy (Join-Path $Raiz 'app') $carpetaApp /MIR /XF log_errores.txt *.pdb /R:2 /W:2 /NFL /NDL /NJH /NJS /NP
    if ($LASTEXITCODE -ge 8) { Fallar "No se pudo copiar el programa (robocopy código $LASTEXITCODE)." }

    # El sistema escribe su log de errores junto al programa: los usuarios comunes necesitan poder escribir ahí.
    # *S-1-5-32-545 = grupo "Usuarios" en cualquier idioma de Windows.
    & icacls $carpetaApp /grant '*S-1-5-32-545:(OI)(CI)M' /T /C /Q | Out-Null
    & icacls (Join-Path $Destino 'Logs') /grant '*S-1-5-32-545:(OI)(CI)M' /T /C /Q | Out-Null

    # Cadena de conexión del programa -> esta instancia y esta base
    $rutaConfig = Join-Path $carpetaApp "$Exe.config"
    [xml]$xml = Get-Content -Path $rutaConfig -Raw -Encoding UTF8
    $conexion = $xml.configuration.connectionStrings.add | Where-Object { $_.name -eq 'CatalogoDB' } | Select-Object -First 1
    if (-not $conexion) { Fallar "El archivo $Exe.config no tiene la conexión 'CatalogoDB'." }
    $conexion.connectionString = "server=$Servidor; database=$BaseDeDatos; integrated security=true"
    $xml.Save($rutaConfig)

    Ok "Programa copiado y configurado para $Servidor / $BaseDeDatos"
}

function Copiar-Herramientas {
    Paso '7/9  Herramientas de mantenimiento'
    Copy-Item -Path (Join-Path $PSScriptRoot 'Respaldar-TanoPos.ps1') -Destination (Join-Path $Destino 'Herramientas') -Force
    Ok 'Respaldar-TanoPos.ps1 copiado'
}

function Crear-AccesosDirectos {
    Paso '8/9  Accesos directos'
    $escritorio = if ($CarpetaEscritorio) { $CarpetaEscritorio } else { [Environment]::GetFolderPath('CommonDesktopDirectory') }
    if (-not (Test-Path $escritorio)) { [void](New-Item -ItemType Directory -Path $escritorio -Force) }
    $shell = New-Object -ComObject WScript.Shell

    function Nuevo-Acceso([string]$ruta) {
        $a = $shell.CreateShortcut($ruta)
        $a.TargetPath = Join-Path $Destino "App\$Exe"
        $a.WorkingDirectory = Join-Path $Destino 'App'
        $a.IconLocation = (Join-Path $Destino "App\$Exe") + ',0'
        $a.Description = 'TANO POS - Punto de venta'
        $a.Save()
    }

    Nuevo-Acceso (Join-Path $escritorio "$NombreAcceso.lnk")
    Ok "Acceso directo en el escritorio: $escritorio"

    if ($AutoInicio) {
        $inicio = Join-Path ([Environment]::GetFolderPath('CommonStartup')) "$NombreAcceso.lnk"
        Nuevo-Acceso $inicio
        Ok 'El sistema se abrirá solo cuando inicie sesión en Windows'
    }
}

function Configurar-Respaldo {
    Paso '9/9  Respaldo diario automático'
    if ($SinTareaRespaldo) { Aviso 'Omitido (-SinTareaRespaldo). RECORDÁ configurar respaldos de otra forma.'; return }

    # La tarea corre como SYSTEM: necesita ser administradora de la instancia para poder hacer el BACKUP.
    Ejecutar-Sql @"
IF SUSER_ID(N'$CuentaSistema') IS NULL CREATE LOGIN [$CuentaSistema] FROM WINDOWS;
GO
ALTER SERVER ROLE sysadmin ADD MEMBER [$CuentaSistema];
GO
"@

    # El servicio de SQL Server (no el usuario) es quien escribe el archivo .bak: necesita permiso en la carpeta.
    & icacls (Join-Path $Destino 'Respaldos') /grant ("NT SERVICE\$NombreServicioSql" + ':(OI)(CI)M') /T /C /Q | Out-Null

    $argumentos = "-NoProfile -ExecutionPolicy Bypass -File `"$(Join-Path $Destino 'Herramientas\Respaldar-TanoPos.ps1')`" -Instancia `"$Instancia`" -BaseDeDatos `"$BaseDeDatos`" -Carpeta `"$(Join-Path $Destino 'Respaldos')`" -DiasConservar $DiasRespaldos"
    if ($CopiaExterna) { $argumentos += " -CopiarA `"$CopiaExterna`"" }

    $accion = New-ScheduledTaskAction -Execute 'powershell.exe' -Argument $argumentos
    $disparador = New-ScheduledTaskTrigger -Daily -At $HoraRespaldo
    $ajustes = New-ScheduledTaskSettingsSet -StartWhenAvailable -ExecutionTimeLimit (New-TimeSpan -Hours 1) -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries
    $principal = New-ScheduledTaskPrincipal -UserId 'SYSTEM' -LogonType ServiceAccount -RunLevel Highest
    [void](Register-ScheduledTask -TaskName 'TANO POS - Respaldo diario' -Action $accion -Trigger $disparador -Settings $ajustes -Principal $principal -Force -Description 'Respalda la base de datos de TANO POS todos los días.')
    Ok "Respaldo todos los días a las $HoraRespaldo en $(Join-Path $Destino 'Respaldos') (se conservan $DiasRespaldos días)"

    # Primer respaldo ahora mismo: comprueba que todo funciona y deja una copia inicial
    & powershell.exe -NoProfile -ExecutionPolicy Bypass -File (Join-Path $Destino 'Herramientas\Respaldar-TanoPos.ps1') -Instancia $Instancia -BaseDeDatos $BaseDeDatos -Carpeta (Join-Path $Destino 'Respaldos') -DiasConservar $DiasRespaldos | Out-Host
    if ($LASTEXITCODE -ne 0) { Aviso 'El primer respaldo falló: revisá Logs\respaldos.log.' } else { Ok 'Primer respaldo hecho y verificado' }
}

# ------------------------------------------------------------------ programa principal

Add-Type -AssemblyName System.Data
Write-Host ''
Write-Host '=========================================' -ForegroundColor White
Write-Host '   TANO POS - Instalador' -ForegroundColor White
Write-Host '=========================================' -ForegroundColor White

if ($SoloVerificar) { Mostrar-Estado; return }

try {
    if (-not (Test-Path $Destino)) { [void](New-Item -ItemType Directory -Path $Destino -Force) }
    $carpetaLogs = Join-Path $Destino 'Logs'
    if (-not (Test-Path $carpetaLogs)) { [void](New-Item -ItemType Directory -Path $carpetaLogs -Force) }
    Start-Transcript -Path (Join-Path $carpetaLogs ("instalacion_{0:yyyyMMdd_HHmm}.log" -f (Get-Date))) | Out-Null

    Comprobar-Requisitos
    Instalar-NetFramework
    Instalar-SqlServer
    Crear-BaseDeDatos
    Configurar-Permisos
    Copiar-Programa
    Copiar-Herramientas
    Crear-AccesosDirectos
    Configurar-Respaldo

    Write-Host ''
    Write-Host '=========================================' -ForegroundColor Green
    Write-Host '   INSTALACIÓN TERMINADA' -ForegroundColor Green
    Write-Host '=========================================' -ForegroundColor Green
    Write-Host ''
    Write-Host 'Pasos que quedan (los hace una persona):'
    Write-Host '  1. Abrir TANO POS desde el escritorio.'
    Write-Host '  2. Elegir la impresora: Ctrl+P (o el ícono de la impresora abajo a la izquierda) y probar un ticket.'
    Write-Host '  3. Revisar las formas de pago y sus recargos (Caja > Métodos de pago).'
    Write-Host '  4. Cargar los artículos y hacer una venta de prueba con impresión.'
    Write-Host ''
}
catch {
    if ($_.Exception.Message -eq 'REINICIO_NECESARIO') { Write-Host 'Reiniciá Windows y ejecutá INSTALAR.bat otra vez.' -ForegroundColor Yellow; exit 3010 }
    Write-Host ''
    Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host 'La instalación se detuvo. Podés corregir el problema y volver a ejecutar INSTALAR.bat: no rompe nada de lo ya instalado.' -ForegroundColor Yellow
    exit 1
}
finally {
    try { Stop-Transcript | Out-Null } catch { }
}
