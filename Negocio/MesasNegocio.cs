using System;
using System.Collections.Generic;
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
    }
}
