using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Negocio;

namespace Negocio
{
    public class MesasNegocio
    {
        public List<Mesa> listarTodas()
        {
            List<Mesa> lista = new List<Mesa>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT NroMesa, ISNULL(IdUsuario, 0) AS IdUsuario, MesaUrlImagen, Estado FROM Mesas");
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Mesa aux = new Mesa();
                    aux.IdMesa = (int)datos.Lector["NroMesa"];
                    aux.IdUsuario = (int)datos.Lector["IdUsuario"];
                    aux.MesaUrlImagen = (string)datos.Lector["MesaUrlImagen"];
                    bool estadobdd = (bool)datos.Lector["Estado"];
                    aux.EstadoMesa = estadobdd ? EstadoMesa.Habilitada : EstadoMesa.Inhabilitada;

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

        public void inhabilitarHabilitarMesa(int estado, int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {

                datos.setearConsulta("update Mesas SET Estado = @Estado WHERE NroMesa = @idMesa");
                datos.setearParametros("@Estado", estado);
                datos.setearParametros("@idMesa", id);
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

        public bool obtenerEstadoPorId(int idMesa)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT Estado FROM Mesas WHERE NroMesa = @idMesa");
                datos.setearParametros("@idMesa", idMesa);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    return (bool)datos.Lector["Estado"];
                }
                return false;
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
        public void asignarMozo(int nroMesa, int idUsuarioMozo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Impactamos el mozo directo en la fila de la mesa
                datos.setearConsulta("UPDATE Mesas SET IdUsuario = @idUsuario WHERE NroMesa = @idMesa");
                datos.setearParametros("@idUsuario", idUsuarioMozo);
                datos.setearParametros("@idMesa", nroMesa);

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
        public void finalizarAsignacion(int nroMesa)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE mesas SET IdUsuario = NULL WHERE NroMesa = @nroMesa");
                datos.setearParametros("@nroMesa", nroMesa);

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

        public int obtenerProximoNumeroMesa()
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT CAST(IDENT_CURRENT('Mesas') + 1 AS INT) AS Proximo");
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    return (int)datos.Lector["Proximo"];
                }
                return 1;
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

        public void agregarNuevaMesaAutomatica()
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO Mesas (IdUsuario, Estado, MesaUrlImagen) VALUES (NULL, 1, '')");
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
