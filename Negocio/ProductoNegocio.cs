using Dominio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

        public void modificarProducto (int id, string nombre, string desc, decimal precio, int stock)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE Productos SET NombreProducto = @nombre, DescripcionProducto = @descripcion, Precio = @precio, Stock = @stock WHERE IdProducto = @id");
                datos.setearParametros("@id", id);
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

        public Productos cargarProductoPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            Productos producto = new Productos();
            try
            {
                datos.setearConsulta("SELECT NombreProducto, DescripcionProducto, Precio, Stock FROM Productos WHERE IdProducto = @id");
                datos.setearParametros("@id", id);
                datos.ejecutarLectura();


                if (datos.Lector.Read())
                {
                    producto.NombreProducto = (string)datos.Lector["NombreProducto"];

                    if (!(datos.Lector["DescripcionProducto"] is DBNull))
                    {
                        producto.DescripcionProducto = (string)datos.Lector["DescripcionProducto"];
                    }
                    else
                    {
                        producto.DescripcionProducto = "";
                    }

                    producto.Precio = (decimal)datos.Lector["Precio"];
                    producto.Stock = (int)datos.Lector["Stock"];
                }
                return producto;
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
