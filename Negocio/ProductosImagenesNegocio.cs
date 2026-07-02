using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class ProductosImagenesNegocio
    {
        public void agregarImagen(ProductosImagenes imagen)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO ProductosImagenes (IdProducto, UrlImagen) VALUES (@idProducto, @urlImagen)");
                datos.setearParametros("@idProducto", imagen.IdProducto);
                datos.setearParametros("@urlImagen", imagen.UrlImagen);
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

        public void eliminarImagen(int idImagen)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("DELETE FROM ProductosImagenes WHERE IdImagen = @idImagen");
                datos.setearParametros("@idImagen", idImagen);

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
