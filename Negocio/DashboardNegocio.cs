using System;
using System.Collections.Generic;
using System.Data;
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
                    "SELECT ISNULL(SUM(PrecioTotal), 0) FROM Pedidos " +
                    "WHERE CAST(FechayHoraPedido AS DATE) = CAST(GETDATE() AS DATE) AND IdEstadoPedido = 2");

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
                    "SELECT COUNT(*) FROM Pedidos WHERE CAST(FechayHoraPedido AS DATE) = CAST(GETDATE() AS DATE)");

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
                    "SELECT TOP 1 NroMesa FROM Pedidos " +
                    "WHERE CAST(FechayHoraPedido AS DATE) = CAST(GETDATE() AS DATE) " +
                    "GROUP BY NroMesa ORDER BY COUNT(*) DESC");

                datos.ejecutarLectura();
                return datos.Lector.Read() ? Convert.ToInt32(datos.Lector["NroMesa"]) : 0;
            }
            catch (Exception ex) { throw ex; }
            finally { datos.cerrarConexion(); }
        }

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
                    //el plato mas vendido (es el 100%) divide la multiplicacion de la cantidad de otros platos * 100
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

        
        

        public List<object> ObtenerReporteMetodosDePago(int periodo)
        {
            List<object> lista = new List<object>();
            AccesoDatos datos = new AccesoDatos();
            string filtroFecha = "1900-01-01";
                if(periodo == 1)filtroFecha = DateTime.Today.ToString("yyyy-MM-dd");
                if (periodo == 2) filtroFecha = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                if (periodo == 3) filtroFecha = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
                if (periodo == 4) filtroFecha = DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
                if (periodo == 4) filtroFecha = DateTime.Today.AddMonths(-3).ToString("yyyy-MM-dd");

            string consulta = @"
                SELECT 
                    MP.NombreMetodo,
                    SUM(P.PrecioTotal) AS MontoTotal
                FROM dbo.MetodosDePago MP
                INNER JOIN dbo.Pedidos P ON MP.IdMetodo = P.IdMetodo
                    AND (P.FechayHoraPedido >= @fechaFiltro OR @PeriodoOriginal = 0)
                GROUP BY Mp.NombreMetodo
                ORDER BY MontoTotal DESC;";
            try
            {
                datos.setearConsulta(consulta);
                datos.setearParametros("@fechaFiltro", filtroFecha);
                datos.setearParametros("@PeriodoOriginal", periodo);
                datos.ejecutarLectura();


                while (datos.Lector.Read())
                {
                    lista.Add(new
                    {
                        NombreMetodo = datos.Lector["NombreMetodo"].ToString(),
                        MontoTotal = Convert.ToDecimal(datos.Lector["MontoTotal"])
                    });


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