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
                datos.setearConsulta("SELECT NroMesa, IdUsuario, MesaUrlImagen, Estado FROM Mesas");
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Mesa aux = new Mesa();
                    aux.IdMesa = (int)datos.Lector["NroMesa"];
                    aux.IdUsuario = (int)datos.Lector["IdUsuario"];
                    aux.MesaUrlImagen = (string)datos.Lector["MesaUrlImagen"];
                    bool estadobdd = (bool)datos.Lector["Estado"];
                    aux.EstadoMesa = estadobdd ? EstadoMesa.Habilitada : EstadoMesa.Inhabilitada ;

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
            finally { 
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
    }
}
