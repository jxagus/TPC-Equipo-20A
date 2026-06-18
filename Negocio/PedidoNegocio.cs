using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class PedidoNegocio
    {
        public int agregarPedido(Pedido nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Registramos el pedido. Usamos SCOPE_IDENTITY() para que SQL nos devuelva el ID que se acaba de crear.
                datos.setearConsulta(@"INSERT INTO Pedidos (NroMesa, IdUsuario, FechayHoraPedido, PrecioTotal, IdMetodo, IdEstadoPedido) 
                                      VALUES (@nroMesa, @idUsuario, @fecha, @precioTotal, NULL, @idEstadoPedido);
                                      SELECT SCOPE_IDENTITY();");

                datos.setearParametros("@nroMesa", nuevo.NroMesa);
                datos.setearParametros("@idUsuario", nuevo.IdUsuario);
                datos.setearParametros("@fecha", nuevo.FechayHoraPedido);
                datos.setearParametros("@precioTotal", nuevo.PrecioTotal);
                datos.setearParametros("@idEstadoPedido", nuevo.IdEstadoPedido);

                int idGenerado = datos.ejecutarAccionEscalar();
                return idGenerado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Metodo para guardar un renglon del detalle del pedido
        public void agregarDetalle(DetallePedido detalle)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO DetallePedido (IdPedido, IdProducto, Cantidad, PrecioUnitario) VALUES (@idPedido, @idProducto, @cantidad, @precioUnitario)");
                datos.setearParametros("@idPedido", detalle.IdPedido);
                datos.setearParametros("@idProducto", detalle.IdProducto);
                datos.setearParametros("@cantidad", detalle.Cantidad);
                datos.setearParametros("@precioUnitario", detalle.PrecioUnitario);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void restarStock(int idProducto, int cantidadRestar)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE Productos SET Stock = Stock - @cantidad WHERE IdProducto = @idProducto");

                datos.setearParametros("@cantidad", cantidadRestar);
                datos.setearParametros("@idProducto", idProducto);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}