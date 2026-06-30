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
        public List<Usuario> listar()
        {
            List<Usuario> lista = new List<Usuario>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT IdUsuario, NombreUsuario, Contrasena, IdRol, Nombre, Apellido, Estado FROM Usuarios");
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Usuario aux = new Usuario();
                    aux.IdUsuario = (int)datos.Lector["IdUsuario"];
                    aux.NombreUsuario = (string)datos.Lector["NombreUsuario"];
                    aux.Contrasena = (string)datos.Lector["Contrasena"];
                    aux.IdRol = (int)datos.Lector["IdRol"];
                    aux.Estado = (bool)datos.Lector["Estado"];
                    //Nombre
                    if (!(datos.Lector["Nombre"] is DBNull))
                        aux.Nombre = (string)datos.Lector["Nombre"];
                    else
                        aux.Nombre = ""; 

                    //Apellido
                    if (!(datos.Lector["Apellido"] is DBNull))
                        aux.Apellido = (string)datos.Lector["Apellido"];
                    else
                        aux.Apellido = ""; //Si es NULL en la DB, le ponemos texto vacío

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
        public void agregarNuevo(Usuario nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("INSERT INTO Usuarios (NombreUsuario, Contrasena, IdRol, Nombre, Apellido) VALUES (@nombreUsuario, @contrasena, @idRol, @nombre, @apellido)");
                datos.setearParametros("@nombreUsuario", nuevo.NombreUsuario);
                datos.setearParametros("@contrasena", nuevo.Contrasena);
                datos.setearParametros("@idRol", nuevo.IdRol);
                datos.setearParametros("@nombre", nuevo.Nombre);
                datos.setearParametros("@apellido", nuevo.Apellido);
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
        public void eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE Usuarios SET Estado = 0 WHERE IdUsuario = @id");
                datos.setearParametros("@id", id);
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
        public bool ExisteNombreUsuario(string nombreUsuario)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @usuario");
                datos.setearParametros("@usuario", nombreUsuario);

                int cantidad = datos.ejecutarAccionEscalar();

                return cantidad > 0;
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
        public void reactivar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE Usuarios SET Estado = 1 WHERE IdUsuario = @id");
                datos.setearParametros("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex) { throw ex; }
            finally { datos.cerrarConexion(); }
        }
    }
}