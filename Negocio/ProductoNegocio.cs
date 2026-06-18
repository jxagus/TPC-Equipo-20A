using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class ProductoNegocio
    {
        public void agregarProducto(string nombre, string desc, decimal precio, int stock)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO Productos (NombreProducto, DescripcionProducto, Precio, Stock) VALUES (@nombre, @descripcion, @precio, @stock)");
                datos.setearParametros("@nombre", nombre);
                datos.setearParametros("@descripcion", desc);
                datos.setearParametros("@precio", precio);
                datos.setearParametros("@stock", stock);
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
        public List<Productos> listar()
        {
            List<Productos> lista = new List<Productos>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT IdProducto, NombreProducto, DescripcionProducto, Precio, Stock FROM Productos");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Productos aux = new Productos();

                    aux.IdProducto = (int)datos.Lector["IdProducto"];
                    aux.NombreProducto = (string)datos.Lector["NombreProducto"];

                     if (!(datos.Lector["DescripcionProducto"] is DBNull))
                    {
                        aux.DescripcionProducto = (string)datos.Lector["DescripcionProducto"];
                    }
                    else
                    {
                        aux.DescripcionProducto = "";
                    }

                    aux.Precio = (decimal)datos.Lector["Precio"];
                    aux.Stock = (int)datos.Lector["Stock"];

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
