using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class LoginNegocio
    {
        public int existeUsuario(string username, string password)
        {
            AccesoDatos datos = new AccesoDatos();
            Usuario usuario = new Usuario();
            int idUsuario = 0;
            try
            {
                datos.setearConsulta("SELECT IdUsuario FROM Usuarios WHERE NombreUsuario = @User AND Contrasena = @Pass");
                datos.setearParametros("@User", username);
                datos.setearParametros("@Pass", password);
                datos.ejecutarLectura();

                if(datos.Lector.Read()){
                    idUsuario = (int)datos.Lector["IdUsuario"];
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

            return idUsuario;
        }


        public int traerRol(int idUsuario)
        {
            AccesoDatos datos = new AccesoDatos();
            Usuario usuario = new Usuario();

            try
            {
                datos.setearConsulta("SELECT IdRol FROM Usuarios WHERE IdUsuario = @idUsuario");
                datos.setearParametros("@idUsuario", idUsuario);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    int suRol = (int)datos.Lector["IdRol"];
                    return suRol;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

            return -1;
        }

    }
}
