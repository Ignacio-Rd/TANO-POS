# TANO POS – Instalador por USB (sin internet)

## Qué hay en el pendrive

```
TANO-POS-Instalador\
├─ INSTALAR.bat              ← doble clic; pide permisos de administrador solo
├─ LEEME.md                  ← esta guía
├─ VERSION.txt               ← versión del programa y del código
├─ CONTROL.sha256            ← huellas para comprobar que nada se corrompió al copiar
├─ app\                      ← el programa compilado (Release)
├─ sql\CATALOGO_DB_RECUPERADA.sql   ← crea la base (se puede correr varias veces sin perder datos)
├─ herramientas\
│   ├─ Instalar-TanoPos.ps1
│   └─ Respaldar-TanoPos.ps1
└─ prerequisitos\            ← LAS DOS DESCARGAS DE MICROSOFT (ver abajo)
    ├─ ndp48-x86-x64-allos-enu.exe
    └─ SQLEXPR_x64_ENU.exe
```

## Preparar el pendrive (en tu PC, con internet, UNA vez)

1. Bajá de Microsoft y guardá en `prerequisitos\`:
   - **.NET Framework 4.8**, instalador sin conexión (`ndp48-x86-x64-allos-enu.exe`).
   - **SQL Server Express**, "Download Media" → "Express Core" (`SQLEXPR_x64_ENU.exe`).
2. Armá la carpeta:

   ```powershell
   .\Instalador\Preparar-Pendrive.ps1 -Salida 'E:\TANO-POS-Instalador'
   ```

   Compila en Release, copia todo y genera `VERSION.txt` y `CONTROL.sha256`. No toca lo que ya hay en `prerequisitos\`.

## Instalar en la computadora del cliente

1. Conectar el pendrive y hacer doble clic en `INSTALAR.bat` (acepta el aviso de administrador).
2. El instalador, en orden: comprueba requisitos → instala .NET 4.8 si falta → instala SQL Server Express si falta → crea la base y corre el script SQL → da permisos mínimos a los usuarios de Windows (solo leer y escribir datos) → copia el programa a `C:\TANO POS\App` → crea acceso directo en el escritorio → programa el respaldo diario.
3. Si pide reiniciar (pasa con .NET), reiniciar y volver a ejecutar `INSTALAR.bat`: retoma donde quedó.

Es seguro **reinstalar o actualizar**: la base de datos y sus datos no se tocan, solo se reemplaza el programa.

Opciones (se agregan al final de `INSTALAR.bat`):
- `-SoloVerificar` muestra qué falta sin instalar nada.
- `-AutoInicio` abre el sistema solo al iniciar sesión en Windows.
- `-CopiaExterna 'E:\Respaldos TANO'` deja configurada la copia del respaldo a otro disco.
- `-HoraRespaldo '04:00'` cambia la hora del respaldo diario (por defecto 04:00).

## Checklist después de instalar

- [ ] Abrir el programa desde el acceso directo.
- [ ] Configurar la impresora (Ctrl+P) y hacer una impresión de prueba.
- [ ] Revisar los métodos de pago y sus recargos.
- [ ] Hacer una venta de prueba y **borrarla**.
- [ ] Ejecutar `C:\TANO POS\Herramientas\Respaldar-TanoPos.ps1 -CopiarA 'E:\...'` una vez y comprobar que aparece el `.bak`.
- [ ] Anotar dónde queda el respaldo y quién lo lleva fuera del local.

## Respaldos

- Diario, automático, en `C:\TANO POS\Respaldos` (se conservan 30 días). Cada uno se **verifica** al hacerlo.
- Un respaldo que está solo en la misma computadora no sirve si se rompe el disco: usar `-CopiarA` con un pendrive/disco externo. Si no está conectado, el script lo avisa en `Logs\respaldos.log` y termina con código 2.
- Restaurar: SSMS → clic derecho en Bases de datos → Restore Database → Device → elegir el `.bak`. O por SQL: `RESTORE DATABASE [CATALOGO_DB_RECUPERADA] FROM DISK = N'ruta.bak' WITH REPLACE`.

## Qué está probado y qué no

Probado en la PC de desarrollo (con SQL Server ya instalado): armado del kit, instalación y reinstalación conservando datos, creación de base, permisos por SID (funciona con Windows en español), cadena de conexión, acceso directo, respaldo + verificación + limpieza + copia externa + aviso de unidad ausente.

**NO probado** (hay que probarlo una vez en una máquina virtual o PC limpia antes de usarlo con un cliente):
- La instalación silenciosa real de .NET 4.8 y SQL Server Express (los nombres de archivo y parámetros pueden cambiar con la versión).
- La creación de la tarea programada de respaldo (requiere administrador).
- El inicio automático con Windows.
