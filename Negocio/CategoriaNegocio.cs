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
    public class CategoriaNegocio
    {
        public void agregarCategoria(string nombre)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO Categorias (NombreCategoria) VALUES (@nombre)");
                datos.setearParametros("@nombre", nombre);
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
        public void agregarSubcategoria(string nombre, int catpadre)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO Categorias (NombreCategoria, IdCategoriaPadre) VALUES (@nombre, @id)");
                datos.setearParametros("@nombre", nombre);
                datos.setearParametros("@id", catpadre);
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
        public List<Categorias> CargarCategorias()
        {
            AccesoDatos ad = new AccesoDatos();
            List<Categorias> lista = new List<Categorias>();
            try
            {
                ad.setearConsulta("SELECT IdCategoria, NombreCategoria FROM Categorias WHERE IdCategoriaPadre IS NULL");
                ad.ejecutarLectura();

                while (ad.Lector.Read())
                {
                    Categorias aux = new Categorias();
                    aux.IdCategoria = Convert.ToInt32(ad.Lector["IdCategoria"]);
                    aux.NombreCategoria = ad.Lector["NombreCategoria"].ToString();

                    lista.Add(aux);
                }
                return lista;
            }
            catch(Exception ex) 
            {
                throw ex;
            }
            finally
            {
                ad.cerrarConexion();
            }
        }
}
}
