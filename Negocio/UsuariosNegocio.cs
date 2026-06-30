using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class UsuariosNegocio
    {
        public void agregarNuevo(Usuario nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("INSERT INTO Usuarios (NombreUsuario, Contrasena, IdRol, NombreCompleto) VALUES (@nombreUsuario, @contrasena, @idRol, @nombreCompleto)");

                datos.setearParametros("@nombreUsuario", nuevo.NombreUsuario);
                datos.setearParametros("@contrasena", nuevo.Contrasena);
                datos.setearParametros("@idRol", nuevo.IdRol);
                datos.setearParametros("@nombreCompleto", nuevo.NombreCompleto);

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

        public List<Usuario> listarMozos()
        {
            List<Usuario> lista = new List<Usuario>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(
                    "SELECT IdUsuario, NombreUsuario " +
                    "FROM Usuarios " +
                    "WHERE IdRol = @rol");

                datos.setearParametros("@rol", 2);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Usuario aux = new Usuario();
                    aux.IdUsuario = Convert.ToInt32(datos.Lector["IdUsuario"]);
                    aux.NombreUsuario = datos.Lector["NombreUsuario"].ToString();

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