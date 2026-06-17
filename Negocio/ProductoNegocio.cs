using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
