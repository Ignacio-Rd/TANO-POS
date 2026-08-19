using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Negocios;
using Negocio_;

namespace Negocio_
{
    public class ProveedorNegocio
    {


        public List<Proveedor> listar()
        {
            List<Proveedor> lista = new List<Proveedor>();
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                datos.SetearConsulta("SELECT Id, Nombre, Cuit, Telefono, Estado FROM PROVEEDORES WHERE Estado = 1");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Proveedor aux = new Proveedor();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Nombre = (string)datos.Lector["Nombre"];

                    // Manejo de nulos por si Cuit o Telefono están vacíos
                    aux.Cuit = datos.Lector["Cuit"] is DBNull ? "" : (string)datos.Lector["Cuit"];
                    aux.Telefono = datos.Lector["Telefono"] is DBNull ? "" : (string)datos.Lector["Telefono"];
                    aux.Estado = (bool)datos.Lector["Estado"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }



        public void agregar(Proveedor nuevo)
        {
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                datos.SetearConsulta("INSERT INTO PROVEEDORES (Nombre, Cuit, Telefono, Estado) VALUES (@nombre, @cuit, @tel, 1)");
                datos.setearParametro("@nombre", nuevo.Nombre);

                // Esto evita el error de la captura 442
                datos.setearParametro("@cuit", (object)nuevo.Cuit ?? DBNull.Value);
                datos.setearParametro("@tel", (object)nuevo.Telefono ?? DBNull.Value);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void RegistrarEntrada(Compras nueva, List<DetalleCompra> detalles)
        {
            AccesoDATOS datos = new AccesoDATOS();
            try
            {
                // 1. Insertamos la compra y pedimos el ID generado con SCOPE_IDENTITY()
                string consulta = @"INSERT INTO COMPRAS (IdProveedor, Fecha, TotalCompra) 
                            VALUES (@idProv, @fecha, @total); 
                            SELECT SCOPE_IDENTITY();";

                datos.SetearConsulta(consulta);
                datos.setearParametro("@idProv", nueva.IdProveedor);
                datos.setearParametro("@fecha", DateTime.Today);




                // Ejecutamos y guardamos el ID
                int idGenerado = Convert.ToInt32(datos.EjecutarScalar());

                // 2. Ahora guardamos los productos vinculados a ese ID
                foreach (var item in detalles)
                {
                    datos.LimpiarParametros();
                    datos.SetearConsulta("INSERT INTO DETALLE_COMPRAS (IdCompra, IdProducto, Cantidad, PrecioCosto) VALUES (@idC, @idP, @cant, @precio)");
                    datos.setearParametro("@idC", idGenerado);
                    datos.setearParametro("@idP", item.IdProducto);
                    datos.EjecutarAccion();
                    // ... ejecutarAccion() ...
                }
            }
            finally
            {
                datos.CerrarConexion();
            }
        }


       

      


    }

}
