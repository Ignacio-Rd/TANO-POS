-- ***************************************************************
-- TANO POS - SCRIPT COMPLETO DE BASE DE DATOS
-- Base: CATALOGO_DB_RECUPERADA   Instancia: .\SQLEXPRESS01
--
-- Crea la base vacia con todas las tablas que usa el programa.
-- Basado en SCRIPT.sql (esquema del 3/4/2026) + lo que el codigo
-- agrego despues (DETALLE_COMPRAS_FIADO, metodo FIADO = 11).
-- Los metodos de pago son los reales de TANO_SCRIPT.sql.
--
-- Se puede ejecutar mas de una vez: lo que ya existe no se toca.
-- ***************************************************************

IF DB_ID('CATALOGO_DB_RECUPERADA') IS NULL
    CREATE DATABASE [CATALOGO_DB_RECUPERADA];
GO

USE [CATALOGO_DB_RECUPERADA];
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-----------------------------------------------------------------
-- 1. CATALOGO DE PRODUCTOS
-----------------------------------------------------------------

IF OBJECT_ID('dbo.MARCAS', 'U') IS NULL
CREATE TABLE dbo.MARCAS (
    Id          INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Descripcion VARCHAR(50) NULL
);
GO

IF OBJECT_ID('dbo.CATEGORIAS', 'U') IS NULL
CREATE TABLE dbo.CATEGORIAS (
    Id          INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Descripcion VARCHAR(50) NULL
);
GO

IF OBJECT_ID('dbo.ARTICULOS', 'U') IS NULL
CREATE TABLE dbo.ARTICULOS (
    Id          INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Codigo      VARCHAR(50)   NULL,          -- codigo de barras
    Nombre      VARCHAR(50)   NULL,
    Descripcion VARCHAR(150)  NULL,
    IdMarca     INT           NULL FOREIGN KEY REFERENCES dbo.MARCAS(Id),
    IdCategoria INT           NULL FOREIGN KEY REFERENCES dbo.CATEGORIAS(Id),
    ImagenUrl   VARCHAR(1000) NULL,
    Precio      MONEY         NULL,
    Stock       INT           NOT NULL DEFAULT 0,
    PrecioCosto MONEY         NOT NULL DEFAULT 0
);
GO

-----------------------------------------------------------------
-- 2. METODOS DE PAGO
-- IMPORTANTE: el programa tiene fijo que FIADO es id_metodo = 11
-- y busca los metodos por nombre 'Efectivo' y 'FIADO' (exactos).
-----------------------------------------------------------------

IF OBJECT_ID('dbo.metodos_de_pago', 'U') IS NULL
CREATE TABLE dbo.metodos_de_pago (
    id_metodo     INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    metodo_pago   NVARCHAR(100) NOT NULL,
    porcentaje    DECIMAL(5,2)  NOT NULL DEFAULT 0,  -- recargo %
    fondo_inicial DECIMAL(18,2) NOT NULL DEFAULT 0   -- fondo de caja
);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.metodos_de_pago)
BEGIN
    SET IDENTITY_INSERT dbo.metodos_de_pago ON;
    INSERT INTO dbo.metodos_de_pago (id_metodo, metodo_pago, porcentaje, fondo_inicial) VALUES
        (2,  N'Efectivo',       0.00, 0),
        (4,  N'Mercado Pago',  10.00, 0),
        (5,  N'Crédito',       10.00, 0),
        (6,  N'Débito',         5.00, 0),
        (7,  N'Transferencia',  3.00, 0),
        (10, N'Ualá',           7.00, 0),
        (11, N'FIADO',          0.00, 0);
    SET IDENTITY_INSERT dbo.metodos_de_pago OFF;
END
GO

-----------------------------------------------------------------
-- 3. CLIENTES (deudores / fiado)
-----------------------------------------------------------------

IF OBJECT_ID('dbo.Clientes', 'U') IS NULL
CREATE TABLE dbo.Clientes (
    Id       INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre   VARCHAR(100) NOT NULL,
    NumeroID VARCHAR(20)  NULL,
    Estado   BIT          NOT NULL DEFAULT 1
);
GO

-----------------------------------------------------------------
-- 4. VENTAS
-----------------------------------------------------------------

IF OBJECT_ID('dbo.Ventas', 'U') IS NULL
CREATE TABLE dbo.Ventas (
    Id        INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Fecha     DATETIME      NOT NULL,
    Total     DECIMAL(18,2) NOT NULL,
    id_metodo INT           NULL,
    IdCliente INT           NULL
);
GO

IF OBJECT_ID('dbo.Venta_Por_Producto', 'U') IS NULL
CREATE TABLE dbo.Venta_Por_Producto (
    Id             INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Fecha          DATETIME      NOT NULL,
    Producto       NVARCHAR(100) NOT NULL,
    Cantidad       INT           NOT NULL,
    IdVenta        INT           NULL CONSTRAINT FK_Detalle_Venta REFERENCES dbo.Ventas(Id),
    PrecioUnitario DECIMAL(18,2) NOT NULL DEFAULT 0,
    IdArticulo     INT           NULL CONSTRAINT FK_VentaPorProducto_Articulos REFERENCES dbo.ARTICULOS(Id),
    ImporteLinea   DECIMAL(10,2) NULL
);
GO

-- Detalle de lo que se llevo cada cliente fiado.
-- IdProducto guarda el CODIGO del articulo (no el Id).
IF OBJECT_ID('dbo.DETALLE_COMPRAS_FIADO', 'U') IS NULL
CREATE TABLE dbo.DETALLE_COMPRAS_FIADO (
    Id                  INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdVenta             INT           NOT NULL REFERENCES dbo.Ventas(Id) ON DELETE CASCADE,
    IdProducto          VARCHAR(50)   NOT NULL,
    Cantidad            INT           NOT NULL,
    PrecioCostoUnitario DECIMAL(18,2) NOT NULL DEFAULT 0,
    PrecioVentaUnitario DECIMAL(18,2) NOT NULL DEFAULT 0
);
GO

IF OBJECT_ID('dbo.PAGOS_DEUDORES', 'U') IS NULL
CREATE TABLE dbo.PAGOS_DEUDORES (
    Id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdCliente     INT           NOT NULL CONSTRAINT FK_Pagos_Clientes REFERENCES dbo.Clientes(Id),
    Fecha         DATETIME      NOT NULL,
    Monto         DECIMAL(18,2) NOT NULL,
    Observaciones VARCHAR(100)  NULL,
    IdMetodoPago  INT           NULL CONSTRAINT FK_PagoDeudor_Metodo REFERENCES dbo.metodos_de_pago(id_metodo)
);
GO

-- Tabla vieja de pagos de fiados (el programa hoy usa PAGOS_DEUDORES).
IF OBJECT_ID('dbo.Pagos_Fiados', 'U') IS NULL
CREATE TABLE dbo.Pagos_Fiados (
    Id          INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdCliente   INT           NOT NULL CONSTRAINT FK_Cliente_Pago REFERENCES dbo.Clientes(Id),
    Fecha       DATETIME      NULL DEFAULT GETDATE(),
    Monto       DECIMAL(18,2) NOT NULL,
    Observacion VARCHAR(150)  NULL
);
GO

IF OBJECT_ID('dbo.GananciasDiarias', 'U') IS NULL
CREATE TABLE dbo.GananciasDiarias (
    Fecha DATETIME      NOT NULL PRIMARY KEY,
    Total DECIMAL(18,2) NOT NULL
);
GO

-----------------------------------------------------------------
-- 5. PROVEEDORES Y COMPRAS DE MERCADERIA
-----------------------------------------------------------------

IF OBJECT_ID('dbo.PROVEEDORES', 'U') IS NULL
CREATE TABLE dbo.PROVEEDORES (
    Id       INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre   VARCHAR(100) NOT NULL,
    Cuit     VARCHAR(20)  NULL,
    Telefono VARCHAR(50)  NULL,
    Estado   BIT          NOT NULL DEFAULT 1
);
GO

IF OBJECT_ID('dbo.COMPRAS', 'U') IS NULL
CREATE TABLE dbo.COMPRAS (
    Id             INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdProveedor    INT           NOT NULL CONSTRAINT FK_Compra_Proveedor REFERENCES dbo.PROVEEDORES(Id),
    Fecha          DATETIME      NULL DEFAULT GETDATE(),
    TotalCompra    DECIMAL(18,2) NULL,
    NroComprobante VARCHAR(50)   NULL
);
GO

-- IdProducto guarda el CODIGO del articulo (no el Id).
IF OBJECT_ID('dbo.DETALLE_COMPRAS', 'U') IS NULL
CREATE TABLE dbo.DETALLE_COMPRAS (
    Id                  INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdCompra            INT           NOT NULL CONSTRAINT FK_Detalle_Compra REFERENCES dbo.COMPRAS(Id),
    IdProducto          NVARCHAR(50)  NULL,
    Cantidad            INT           NOT NULL,
    PrecioCostoUnitario DECIMAL(18,2) NULL
);
GO

-- IdCompra es NULL cuando se paga deuda sin compra asociada.
-- IdMetodoPago acepta NULL porque al guardar una compra el programa
-- registra el pago sin metodo.
IF OBJECT_ID('dbo.PAGOS_PROVEEDORES', 'U') IS NULL
CREATE TABLE dbo.PAGOS_PROVEEDORES (
    Id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdProveedor   INT           NOT NULL CONSTRAINT FK_Pago_Proveedor REFERENCES dbo.PROVEEDORES(Id),
    IdCompra      INT           NULL CONSTRAINT FK_Pago_Compra REFERENCES dbo.COMPRAS(Id),
    Monto         DECIMAL(18,2) NULL,
    Fecha         DATETIME      NULL DEFAULT GETDATE(),
    IdMetodoPago  INT           NULL CONSTRAINT FK_Pago_Metodo REFERENCES dbo.metodos_de_pago(id_metodo),
    Observaciones VARCHAR(200)  NULL,
    TotalCompra   DECIMAL(10,2) NULL
);
GO

-----------------------------------------------------------------
-- 6. PAGOS VARIOS (gastos)
-----------------------------------------------------------------

IF OBJECT_ID('dbo.CATEGORIAS_PAGOS_VARIOS', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.CATEGORIAS_PAGOS_VARIOS (
        IdCategoria INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Nombre      VARCHAR(50) NOT NULL
    );

    INSERT INTO dbo.CATEGORIAS_PAGOS_VARIOS (Nombre) VALUES
        ('Alquiler'),
        ('Sueldo'),
        ('Impuestos'),
        ('Servicios (luz/agua/gas)'),
        ('Insumos de limpieza'),
        ('Mantenimiento'),
        ('Publicidad'),
        ('Contador/Gestor'),
        ('Seguro'),
        ('Varios');
END
GO

IF OBJECT_ID('dbo.PAGOS_VARIOS', 'U') IS NULL
CREATE TABLE dbo.PAGOS_VARIOS (
    IdPago       INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdCategoria  INT           NOT NULL FOREIGN KEY REFERENCES dbo.CATEGORIAS_PAGOS_VARIOS(IdCategoria),
    IdMetodoPago INT           NOT NULL FOREIGN KEY REFERENCES dbo.metodos_de_pago(id_metodo),
    Monto        DECIMAL(18,2) NOT NULL,
    Fecha        DATETIME      NOT NULL DEFAULT GETDATE(),
    Observacion  VARCHAR(200)  NULL
);
GO

-----------------------------------------------------------------
-- 7. CONFIGURACION (impresora de tickets, una sola fila Id = 1)
-----------------------------------------------------------------

IF OBJECT_ID('dbo.CONFIGURACION', 'U') IS NULL
CREATE TABLE dbo.CONFIGURACION (
    Id              INT NOT NULL PRIMARY KEY CHECK (Id = 1),
    NombreImpresora VARCHAR(200) NOT NULL
);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.CONFIGURACION WHERE Id = 1)
    INSERT INTO dbo.CONFIGURACION (Id, NombreImpresora) VALUES (1, 'Microsoft Print to PDF');
GO

PRINT 'Base CATALOGO_DB_RECUPERADA lista.';
GO
