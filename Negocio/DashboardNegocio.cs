using System;
using System.Collections.Generic;
using Dominio;
using Negocio; 

namespace Negocio
{
    public class DashboardNegocio
    {
        public decimal ObtenerCajaDelDia()
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(
                    "DECLARE @FechaFiltro DATE = ISNULL((SELECT MAX(CAST(FechayHoraPedido AS DATE)) FROM Pedidos), CAST(GETDATE() AS DATE)); " +
                    "SELECT ISNULL(SUM(PrecioTotal), 0) FROM Pedidos " +
                    "WHERE CAST(FechayHoraPedido AS DATE) = @FechaFiltro AND IdEstadoPedido = 2"); // 2 = Finalizado

                datos.ejecutarLectura();
                return datos.Lector.Read() ? Convert.ToDecimal(datos.Lector[0]) : 0;
            }
            catch (Exception ex) { throw ex; }
            finally { datos.cerrarConexion(); }
        }

        public decimal ObtenerCajaDelMes()
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(
                    "DECLARE @Año INT = ISNULL((SELECT YEAR(MAX(FechayHoraPedido)) FROM Pedidos), YEAR(GETDATE())); " +
                    "DECLARE @Mes INT = ISNULL((SELECT MONTH(MAX(FechayHoraPedido)) FROM Pedidos), MONTH(GETDATE())); " +
                    "SELECT ISNULL(SUM(PrecioTotal), 0) FROM Pedidos " +
                    "WHERE MONTH(FechayHoraPedido) = @Mes AND YEAR(FechayHoraPedido) = @Año AND IdEstadoPedido = 2");

                datos.ejecutarLectura();
                return datos.Lector.Read() ? Convert.ToDecimal(datos.Lector[0]) : 0;
            }
            catch (Exception ex) { throw ex; }
            finally { datos.cerrarConexion(); }
        }

        public decimal ObtenerCajaAnual()
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(
                    "DECLARE @Año INT = ISNULL((SELECT YEAR(MAX(FechayHoraPedido)) FROM Pedidos), YEAR(GETDATE())); " +
                    "SELECT ISNULL(SUM(PrecioTotal), 0) FROM Pedidos WHERE YEAR(FechayHoraPedido) = @Año AND IdEstadoPedido = 2");

                datos.ejecutarLectura();
                return datos.Lector.Read() ? Convert.ToDecimal(datos.Lector[0]) : 0;
            }
            catch (Exception ex) { throw ex; }
            finally { datos.cerrarConexion(); }
        }

        public int ObtenerCantidadPedidosHoy()
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(
                    "DECLARE @FechaFiltro DATE = ISNULL((SELECT MAX(CAST(FechayHoraPedido AS DATE)) FROM Pedidos), CAST(GETDATE() AS DATE)); " +
                    "SELECT COUNT(*) FROM Pedidos WHERE CAST(FechayHoraPedido AS DATE) = @FechaFiltro");

                datos.ejecutarLectura();
                return datos.Lector.Read() ? Convert.ToInt32(datos.Lector[0]) : 0;
            }
            catch (Exception ex) { throw ex; }
            finally { datos.cerrarConexion(); }
        }

        public int ObtenerPedidosPendientesCocina()
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT COUNT(*) FROM Pedidos WHERE IdEstadoPedido = 1");
                datos.ejecutarLectura();
                return datos.Lector.Read() ? Convert.ToInt32(datos.Lector[0]) : 0;
            }
            catch (Exception ex) { throw ex; }
            finally { datos.cerrarConexion(); }
        }

        public int ObtenerMesaMayorRotacion()
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(
                    "DECLARE @FechaFiltro DATE = ISNULL((SELECT MAX(CAST(FechayHoraPedido AS DATE)) FROM Pedidos), CAST(GETDATE() AS DATE)); " +
                    "SELECT TOP 1 NroMesa FROM Pedidos " +
                    "WHERE CAST(FechayHoraPedido AS DATE) = @FechaFiltro " +
                    "GROUP BY NroMesa ORDER BY COUNT(*) DESC");

                datos.ejecutarLectura();
                return datos.Lector.Read() ? Convert.ToInt32(datos.Lector["NroMesa"]) : 0;
            }
            catch (Exception ex) { throw ex; }
            finally { datos.cerrarConexion(); }
        }

        //TOP 5 PLATOS MAS SOLICITADO
        public List<Productos> ObtenerTopPlatos()
        {
            List<Productos> lista = new List<Productos>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(
                    "SELECT TOP 5 P.IdProducto, P.NombreProducto, SUM(DP.Cantidad) AS Cantidad " +
                    "FROM DetallePedido DP " +
                    "INNER JOIN Productos P ON DP.IdProducto = P.IdProducto " +
                    "GROUP BY P.IdProducto, P.NombreProducto " +
                    "ORDER BY Cantidad DESC");

                datos.ejecutarLectura();

                int maxCantidad = 0;

                while (datos.Lector.Read())
                {
                    Productos aux = new Productos();
                    aux.IdProducto = Convert.ToInt32(datos.Lector["IdProducto"]);
                    aux.NombreProducto = datos.Lector["NombreProducto"].ToString();
                    aux.CantidadVendida = Convert.ToInt32(datos.Lector["Cantidad"]);

                    if (maxCantidad == 0)
                    {
                        maxCantidad = aux.CantidadVendida;
                    }

                    aux.Porcentaje = maxCantidad > 0 ? (aux.CantidadVendida * 100) / maxCantidad : 0;

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