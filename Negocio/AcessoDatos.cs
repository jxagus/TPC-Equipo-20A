using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
 
namespace Negocio
{
    public class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        public SqlDataReader Lector
        {
            get { return lector; }
        }
        public AccesoDatos()
        {
            conexion = new SqlConnection(ConfigurationManager.AppSettings["cadenaConexion"]); //desde webconfig
            comando = new SqlCommand();
            comando.Connection = conexion;

        }
        public void setearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }
        public void setearSP(string sp)
        {
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            comando.CommandText = sp;
        }
        public void ejecutarAccion()
        {
            comando.Connection = conexion; 

            try
            {
                conexion.Open();
                comando.ExecuteNonQuery(); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexion.Close();
            }
        }
        public void ejecutarLectura()
        {
            comando.Connection = conexion; 

            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void setearParametros(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }
        public void setearQuery(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }
        public int ejecutarAccionEscalar()
        {
            comando.Connection = conexion; 
            try
            {
                conexion.Open();
                return int.Parse(comando.ExecuteScalar().ToString()); // ejecutar acciones que devuelven un solo valor 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void agregarParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }
        public void cerrarConexion()
        {
            if (lector != null)
                lector.Close();

            conexion.Close();
        }
        public void limpiarComando()
        {
            comando.Parameters.Clear();
        }
        public void limpiarParametros()
        {
            if (this.comando.Parameters != null)
            {
                this.comando.Parameters.Clear();
            }
        }
        public void resetearAcceso()
        {
            if (this.comando != null)
            {
                this.comando.Parameters.Clear();
            }

            if (this.lector != null && !this.lector.IsClosed)
            {
                this.lector.Close();
            }

            if (this.conexion != null && this.conexion.State == System.Data.ConnectionState.Open)
            {
                this.conexion.Close();
            }
        }
    }
}
