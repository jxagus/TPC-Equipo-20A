using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class MetodoPagoNegocio
    {
        public List<MetodoPago> listar()
        {
            List<MetodoPago> lista = new List<MetodoPago>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT IdMetodo, NombreMetodo, Activo FROM MetodosDePago");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    MetodoPago aux = new MetodoPago();
                    aux.IdMetodo = (int)datos.Lector["IdMetodo"];

                    aux.NombreMetodo = (string)datos.Lector["NombreMetodo"];

                    aux.Activo = (bool)datos.Lector["Activo"];

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

        public void estadoDeMetodoPago(int activo, int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {

                datos.setearConsulta("update MetodosDePago SET Activo = @Activo WHERE IdMetodo = @idMetodo");
                datos.setearParametros("@Activo", activo);
                datos.setearParametros("@idMetodo", id);
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

        public void agregarMetodoPago(string nombreMetodo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {

                datos.setearConsulta("insert into MetodosDePago (NombreMetodo) VALUES (@nombre)");
                datos.setearParametros("@nombre", nombreMetodo);
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

        public void modificarMetodoPago(string nombreMetodo, int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {

                datos.setearConsulta("UPDATE MetodosDePago SET NombreMetodo = @nombre WHERE IdMetodo = @idMetodo");
                datos.setearParametros("@idMetodo", id);
                datos.setearParametros("@nombre", nombreMetodo);
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


        public MetodoPago buscarPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            MetodoPago aux = null;
            try
            {
                datos.setearConsulta("SELECT IdMetodo, NombreMetodo, Activo FROM MetodosDePago WHERE IdMetodo = @id");
                datos.setearParametros("@id", id);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    aux = new MetodoPago();
                    aux.IdMetodo = (int)datos.Lector["IdMetodo"];

                    aux.NombreMetodo = (string)datos.Lector["NombreMetodo"];

                    aux.Activo = (bool)datos.Lector["Activo"];

                    return aux;
                }

                return aux;
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

        public bool verificarExistencia(string nombre)
        {
            AccesoDatos datos = new AccesoDatos();
            string nombreSinespacios = nombre.Trim();
            try
            {
                datos.setearConsulta("SELECT COUNT(*) FROM MetodosDePago WHERE UPPER(LTRIM(RTRIM(NombreMetodo))) = UPPER(@nombre)");
                datos.setearParametros("@nombre", nombreSinespacios);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    int cantidad = Convert.ToInt32(datos.Lector[0]);
                    return cantidad > 0;
                }

                return false;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
        
            }
        }
    }
}
