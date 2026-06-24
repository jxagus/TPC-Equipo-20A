using Dominio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Deployment.Internal;
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

        public void modificarCategoria(string nombre, int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE Categorias SET NombreCategoria = @nombre WHERE IdCategoria = @id");
                datos.setearParametros("@nombre", nombre);
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

        public void modificarSubategoria(string nombre, int idCP, int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE Categorias SET NombreCategoria = @nombre, IdCategoriaPadre = @idCP WHERE IdCategoria = @id");
                datos.setearParametros("@nombre", nombre);
                datos.setearParametros("@idCP", idCP);
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

        public List<Categorias> listarTODAS()
        {
            AccesoDatos ad = new AccesoDatos();
            List<Categorias> lista = new List<Categorias>();
            try
            {
                ad.setearConsulta("SELECT IdCategoria, NombreCategoria, IdCategoriaPadre FROM Categorias");
                ad.ejecutarLectura();

                while (ad.Lector.Read())
                {
                    Categorias aux = new Categorias();
                    aux.IdCategoria = Convert.ToInt32(ad.Lector["IdCategoria"]);
                    aux.NombreCategoria = ad.Lector["NombreCategoria"].ToString();
                    if (ad.Lector["IdCategoriaPadre"] != DBNull.Value)
                    {
                    aux.IdCategoriaPadre = Convert.ToInt32(ad.Lector["IdCategoriaPadre"]);
                    }
                    else
                    {
                        aux.IdCategoriaPadre = 0;
                    }

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
                ad.cerrarConexion();
            }
        }
        public Categorias cargarCategoriaPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            Categorias categoria = new Categorias();
            try
            {
                datos.setearConsulta("SELECT NombreCategoria, IdCategoriaPadre FROM Categorias WHERE IdCategoria = @id");
                datos.setearParametros("@id", id);
                datos.ejecutarLectura();


                if (datos.Lector.Read())
                {
                    categoria.IdCategoria = id;

                    categoria.NombreCategoria = (string)datos.Lector["NombreCategoria"];

                    if (datos.Lector["IdCategoriaPadre"] != DBNull.Value)
                    {
                        categoria.IdCategoriaPadre = Convert.ToInt32(datos.Lector["IdCategoriaPadre"]);
                    }
                    else
                    {
                        categoria.IdCategoriaPadre = 0;
                    }
                }
                return categoria;
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
