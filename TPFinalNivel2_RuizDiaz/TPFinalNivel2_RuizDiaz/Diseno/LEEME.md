# Diseño de la interfaz

Componentes compartidos para que todas las pantallas tengan el mismo aspecto (estilo ERP / Fluent / Stripe).

| Archivo | Qué es |
|---|---|
| `Tema.cs` | Paleta, fuentes (Segoe UI) y utilidades. `Tema.EstilizarGrilla(dgv)` deja cualquier DataGridView con el estilo moderno. |
| `TarjetaPanel.cs` | Panel de esquinas redondeadas, borde fino y título opcional (`Titulo`). |
| `BotonPlano.cs` | Botón plano con variantes `Primario`, `Exito` (cobrar), `Peligro` (quitar) y `Secundario`, tecla rápida en un chip (`Atajo`) y anillo de foco para teclado. Sigue siendo un `Button`. |
| `Estilizador.cs` | `Estilizador.Aplicar(this)`: lleva cualquier formulario existente al estilo moderno **sin mover ni renombrar controles** (colores, tipografía, botones con esquinas redondeadas, grillas). Una línea en el constructor. |
| `ItemMenu.cs` | Ítem del menú lateral (ícono + texto + tecla). ↑/↓ mueven el foco, Enter/Espacio abren. |

## Paleta

Menú `#0F172A` · fondo `#F8FAFC` · tarjetas blancas con borde `#E2E8F0` · acción principal `#2563EB` ·
cobrar `#16A34A` · quitar/cancelar `#DC2626`.

## Qué pantallas usan qué

- **Rediseño completo por código** (reubican los controles existentes): pantalla principal, `REGISTRADORA DE COMPRAS.Diseno.cs` (ventas) y `TOTAL A COBRAR.Diseno.cs` (cobro).
- **Estilo automático** (`Estilizador.Aplicar(this)` en el constructor): todas las demás pantallas.

## Cómo llevar otra pantalla al mismo estilo (rápido)

Si solo querés el estilo, alcanza con una línea después de `InitializeComponent();`:

```csharp
Diseno.Estilizador.Aplicar(this);
```

## Cómo hacer un rediseño completo (como ventas o cobro)

1. En el constructor, después de `InitializeComponent()`: `BackColor = Tema.Fondo; Font = Tema.Fuente(10f);`
2. Cada grilla: `Tema.EstilizarGrilla(miGrilla);`
3. Cada botón importante: cambiar `Button` por `Diseno.BotonPlano` en el `.Designer.cs` (tipo y `new`) y asignar `Variante` / `Atajo`.
4. Agrupar controles en `TarjetaPanel` (con `Titulo`) en vez de `GroupBox`.

La pantalla de ventas (`REGISTRADORA DE COMPRAS.Diseno.cs`) es el ejemplo completo: reubica los controles
existentes **sin cambiar sus nombres ni sus eventos**, por eso el teclado funciona igual que antes.

## Reglas para no romper la operatoria por teclado

- No cambiar el nombre de los controles que tienen manejadores de teclado (`codigo_Producto`, etc.).
- Todo control interactivo debe poder recibir foco (`TabStop`) y mostrar el foco de forma visible.
- Nada de ventanas de confirmación nuevas en el flujo de venta.

## Íconos

El menú usa la fuente `Segoe MDL2 Assets` (viene con Windows 10 y 11). Si no está instalada (Windows 7/8),
`Tema.IconosDisponibles` es `false` y cada ítem muestra un punto en lugar del ícono.
