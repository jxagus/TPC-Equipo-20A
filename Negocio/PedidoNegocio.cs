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
        public List<Pedido> listarPedidosActivos()
        {
            List<Pedido> lista = new List<Pedido>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT IdPedido, NroMesa, IdUsuario, FechayHoraPedido, PrecioTotal, IdMetodo, IdEstadoPedido FROM Pedidos WHERE IdEstadoPedido = 1 ORDER BY FechayHoraPedido ASC");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Pedido aux = new Pedido();

                    aux.IdPedido = (int)datos.Lector["IdPedido"];
                    aux.NroMesa = (int)datos.Lector["NroMesa"];
                    aux.IdUsuario = (int)datos.Lector["IdUsuario"];
                    aux.FechayHoraPedido = (DateTime)datos.Lector["FechayHoraPedido"];
                    aux.PrecioTotal = (decimal)datos.Lector["PrecioTotal"];

                    if (!(datos.Lector["IdMetodo"] is DBNull))
                    {
                        aux.IdMetodo = (int)datos.Lector["IdMetodo"];
                    }

                    aux.IdEstadoPedido = (int)datos.Lector["IdEstadoPedido"];

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
                datos.cerrarConexion();
            }
        }
        public int agregarPedido(Pedido nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                //Registramos el pedido. Usamos SCOPE_IDENTITY() para que SQL nos devuelva el ID que se acaba de crear.
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
        public List<string> listarNumeroMesas()
        {
            List<string> lista = new List<string>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT NroMesa FROM Mesas");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    //Extraemos el numero de la mesa y lo añadimos a la lista
                    lista.Add(datos.Lector["NroMesa"].ToString());
                }

                return lista;
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
        public void finalizarPedido(int idPedido)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                //Cambiamos el estado al ID 2 (Finalizado)
                datos.setearConsulta("UPDATE Pedidos SET IdEstadoPedido = 2 WHERE IdPedido = @idPedido");
                datos.setearParametros("@idPedido", idPedido);

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
        public List<DetallePedido> listarDetallesPorId(int idPedido)
        {
            List<DetallePedido> lista = new List<DetallePedido>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT D.IdPedido, D.IdProducto, P.NombreProducto, D.Cantidad, D.PrecioUnitario " +
                                     "FROM DetallePedido D " +
                                     "INNER JOIN Productos P ON D.IdProducto = P.IdProducto " +
                                     "WHERE D.IdPedido = @idPedido");

                datos.setearParametros("@idPedido", idPedido);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    DetallePedido aux = new DetallePedido();

                    aux.IdPedido = (int)datos.Lector["IdPedido"];
                    aux.IdProducto = (int)datos.Lector["IdProducto"];
                    aux.Cantidad = (int)datos.Lector["Cantidad"];
                    aux.PrecioUnitario = (decimal)datos.Lector["PrecioUnitario"];

                    aux.NombreProducto = datos.Lector["NombreProducto"].ToString();

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
                datos.cerrarConexion();
            }
        }
    }
}