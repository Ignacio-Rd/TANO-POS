# Pruebas automáticas del POS

Proyecto: `POS.Pruebas` (NUnit + Moq, .NET Framework 4.8). Está dentro de la solución `Punto_De_Venta.sln`.

## Qué hay

| Carpeta | Qué prueba | Necesita base de datos |
|---|---|---|
| `Unitarias/` | Subtotales, recargos por método de pago, conciliación y arqueo de caja. Usan Moq. | No (corren en ~1 segundo) |
| `Integracion/` | Guardado de ventas, descuento de stock, detalle del ticket, arqueo con SQL real, ajuste de precios. | Sí (SQL Server) |

Cada prueba de integración corre dentro de un `TransactionScope` que nunca se confirma: al terminar, SQL Server
deshace todo y la base queda como estaba.

## Cómo correrlas en tu PC

Desde Visual Studio: **Prueba → Explorador de pruebas → Ejecutar todo**.

Desde la terminal (en la carpeta de la solución):

```powershell
msbuild Punto_De_Venta.sln -restore -p:Configuration=Debug
dotnet test ..\Pruebas\POS.Pruebas\bin\Debug\net48\POS.Pruebas.dll --filter "TestCategory=Unitaria"
dotnet test ..\Pruebas\POS.Pruebas\bin\Debug\net48\POS.Pruebas.dll --filter "TestCategory=Integracion"
```

## Base de pruebas (una sola vez)

Las pruebas de integración usan una base **aparte** (`CATALOGO_DB_TEST`), nunca la del comercio. Para crearla:

```powershell
(Get-Content ..\..\BaseDeDatos\CATALOGO_DB_RECUPERADA.sql -Raw) -replace 'CATALOGO_DB_RECUPERADA','CATALOGO_DB_TEST' |
    Set-Content crear_base_test.sql -Encoding utf8
sqlcmd -S ".\SQLEXPRESS" -E -C -b -f 65001 -i crear_base_test.sql
```

La cadena de conexión está en `POS.Pruebas/app.config` (entrada `CatalogoDB`). Si la base no existe, las pruebas de
integración se **omiten** en tu PC; en GitHub Actions (`POS_REQUIRE_DB=1`) fallan.

## GitHub Actions

`.github/workflows/tests.yml` compila la solución y corre los dos grupos de pruebas en cada `git push` y en cada
pull request. El detalle de resultados (`.trx`) queda como artefacto de la corrida.
