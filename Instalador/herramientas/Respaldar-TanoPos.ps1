<#
.SYNOPSIS
    Respalda la base de datos de TANO POS, verifica la copia, borra las viejas y (opcional) la copia a otro disco.

.DESCRIPTION
    La instalación lo programa todos los días. También se puede ejecutar a mano antes de actualizar el sistema o
    cambiar la computadora. Termina con código 0 si TODO salió bien y distinto de 0 si algo falló.

.EXAMPLE
    .\Respaldar-TanoPos.ps1 -CopiarA 'E:\Respaldos TANO'
#>
[CmdletBinding()]
param(
    [string]$Instancia = 'SQLEXPRESS',
    [string]$BaseDeDatos = 'CATALOGO_DB_RECUPERADA',
    [string]$Carpeta = 'C:\TANO POS\Respaldos',
    [int]$DiasConservar = 30,
    # Pendrive o disco externo: si está conectado se copia ahí también (un respaldo en la misma computadora no sirve si se rompe).
    [string]$CopiarA = ''
)

$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Data

$Servidor = if ($Instancia -eq 'MSSQLSERVER') { '.' } else { ".\$Instancia" }
$registro = Join-Path (Split-Path -Parent $Carpeta) 'Logs\respaldos.log'

function Anotar([string]$mensaje) {
    $linea = '{0:yyyy-MM-dd HH:mm:ss}  {1}' -f (Get-Date), $mensaje
    Write-Host $linea
    try {
        $dir = Split-Path -Parent $registro
        if (-not (Test-Path $dir)) { [void](New-Item -ItemType Directory -Path $dir -Force) }
        Add-Content -Path $registro -Value $linea -Encoding UTF8
    } catch { }
}

function Ejecutar([string]$sql, [int]$segundos = 600) {
    $cn = New-Object System.Data.SqlClient.SqlConnection("Server=$Servidor;Database=master;Integrated Security=true;Connect Timeout=15")
    $cn.Open()
    try { $cmd = $cn.CreateCommand(); $cmd.CommandText = $sql; $cmd.CommandTimeout = $segundos; [void]$cmd.ExecuteNonQuery() }
    finally { $cn.Dispose() }
}

try {
    if ($BaseDeDatos -notmatch '^[\w\. -]+$') { throw "Nombre de base no válido: $BaseDeDatos" }
    if (-not (Test-Path $Carpeta)) { [void](New-Item -ItemType Directory -Path $Carpeta -Force) }

    $archivo = Join-Path $Carpeta ('{0}_{1:yyyyMMdd_HHmm}.bak' -f $BaseDeDatos, (Get-Date))
    $archivoSql = $archivo.Replace("'", "''")

    Anotar "Respaldando [$BaseDeDatos] en $archivo"
    # CHECKSUM detecta páginas dañadas al copiar; INIT sobreescribe si el archivo existiera.
    Ejecutar "BACKUP DATABASE [$BaseDeDatos] TO DISK = N'$archivoSql' WITH INIT, CHECKSUM, NAME = N'TANO POS respaldo'"

    # Comprobar que la copia se puede leer. Un respaldo sin verificar no es un respaldo.
    Ejecutar "RESTORE VERIFYONLY FROM DISK = N'$archivoSql' WITH CHECKSUM"
    $tamano = [math]::Round((Get-Item $archivo).Length / 1MB, 1)
    Anotar "Respaldo verificado ($tamano MB)"

    # Limpiar los viejos
    $limite = (Get-Date).AddDays(-$DiasConservar)
    $viejos = Get-ChildItem -Path $Carpeta -Filter "$BaseDeDatos*.bak" | Where-Object { $_.LastWriteTime -lt $limite }
    foreach ($v in $viejos) { Remove-Item -Path $v.FullName -Force; Anotar "Borrado respaldo viejo: $($v.Name)" }

    # Copia a otro disco (si está conectado)
    if ($CopiarA) {
        if (Test-Path (Split-Path -Qualifier $CopiarA)) {
            if (-not (Test-Path $CopiarA)) { [void](New-Item -ItemType Directory -Path $CopiarA -Force) }
            Copy-Item -Path $archivo -Destination $CopiarA -Force
            Anotar "Copia externa guardada en $CopiarA"
        }
        else {
            Anotar "AVISO: no se pudo hacer la copia externa porque $(Split-Path -Qualifier $CopiarA) no está conectado."
            exit 2
        }
    }

    exit 0
}
catch {
    Anotar "ERROR: $($_.Exception.Message)"
    exit 1
}
