using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocios;
using Negocio_;
using Dominio;
using System.Data;

namespace Negocio_
{
    public class PagosVariosNegocio
    {
        public List<CategoriaPagosVarios> lista() {

            List<CategoriaPagosVarios> lista = new List<CategoriaPagosVarios>();
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                datos.SetearConsulta("SELECT IdCategoria, Nombre FROM CATEGORIAS_PAGOS_VARIOS ORDER BY Nombre");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    CategoriaPagosVarios aux = new CategoriaPagosVarios();
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.IdCategoria = (int)datos.Lector["IdCategoria"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            finally { datos.CerrarConexion(); }
           
            
        }


        public List<PagosVarios> listaPagos()
        {

            List<PagosVarios> lista = new List<PagosVarios>();
            AccesoDATOS datos = new AccesoDATOS();

            try
            {
                datos.SetearConsulta(@"
    SELECT C.Nombre, M.metodo_pago, P.IdPago, P.IdCategoria, 
           P.IdMetodoPago, P.Monto, P.Fecha, P.Observacion  
    FROM PAGOS_VARIOS P
    INNER JOIN CATEGORIAS_PAGOS_VARIOS C ON P.IdCategoria  = C.IdCategoria
    INNER JOIN metodos_de_pago         M ON P.IdMetodoPago = M.id_metodo
    ORDER BY P.Fecha DESC");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    PagosVarios aux = new PagosVarios();
                    aux.IdPago = (int)datos.Lector["IdPago"];
                    aux.Categoria = (string)datos.Lector["Nombre"];  // ← directo al string
                    aux.IdCategoria = (int)datos.Lector["IdCategoria"];
                    aux.IdMetodoPago = (int)datos.Lector["IdMetodoPago"];
                    aux.MetodoPago = (string)datos.Lector["metodo_pago"];
                    aux.Monto = (decimal)datos.Lector["Monto"];
                    aux.Fecha = (DateTime)datos.Lector["Fecha"];
                    aux.Observacion = datos.Lector["Observacion"] == DBNull.Value
                                       ? "-"
                                       : (string)datos.Lector["Observacion"];
                    
                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            finally { datos.CerrarConexion(); }


        }

        public void RegistrarPago(PagosVarios pago)
        {
            if (pago.Monto <= 0)
                throw new Exception("El monto debe ser mayor a cero.");
            if (pago.IdCategoria <= 0)
                throw new Exception("Seleccione una categoría válida.");
            if (pago.IdMetodoPago <= 0)
                throw new Exception("Seleccione un método de pago válido.");

            AccesoDATOS datos = new AccesoDATOS();
            datos.SetearConsulta(@"INSERT INTO PAGOS_VARIOS 
                           (IdCategoria, IdMetodoPago, Monto, Fecha, Observacion) 
                           VALUES (@cat, @metodo, @monto, @fecha, @obs)");
            datos.setearParametro("@cat", pago.IdCategoria);
            datos.setearParametro("@metodo", pago.IdMetodoPago);
            datos.setearParametro("@monto", pago.Monto);
            datos.setearParametro("@fecha", pago.Fecha);
            datos.setearParametro("@obs", string.IsNullOrEmpty(pago.Observacion)
                                             ? (object)DBNull.Value
                                             : pago.Observacion);
            datos.EjecutarAccion();
            datos.CerrarConexion();          
        }

    }
}
