using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Resto_Bar_Web
{
    public partial class AsignacionMesa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUsuario"] == null)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error: no esta iniciado sesion');", true);

                ///redirigir a una pagian de error, el mensaje es temporal
            }
            else
            {
                LoginNegocio negocio = new LoginNegocio();
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);
                int rol = negocio.traerRol(idUsuario);
                if (rol == 0 || rol == 1)
                {

                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Permisos insuficientes');", true);

                    ///redirigir a pagina de error, el mensaje es temporal
                }
            }

            if (!IsPostBack)
            {
                CargarMozos();
                CargarMesas();
            }
        }

        //Usamos el acceso a datos, ademas marcamos que el rol 2 es para lo mozos(a definir)
        private void CargarMozos()
        {
            AccesoDatos ad = new AccesoDatos();
            try
            {
                ad.setearConsulta(
                    "SELECT IdUsuario, NombreUsuario " +
                    "FROM Usuarios " +
                    "WHERE IdRol = @rol");

                ad.setearParametros("@rol", 2);
                ad.ejecutarLectura();

                ddlMozo.Items.Add(new ListItem("-- Seleccioná un mozo --", "0"));

                while (ad.Lector.Read())
                {
                    int idUsuario = Convert.ToInt32(ad.Lector["IdUsuario"]);
                    string nombreUsuario = ad.Lector["NombreUsuario"].ToString();

                    ddlMozo.Items.Add(new ListItem(nombreUsuario, idUsuario.ToString()));
                }
            }
            finally
            {
                ad.cerrarConexion();
            }
        }

        private void CargarMesas()
        {
            MesasNegocio negocio = new MesasNegocio();

            ddlMesa.Items.Clear();
            ddlMesa.Items.Add(new ListItem("-- Seleccioná una mesa --", "0"));

            List<Mesa> mesas = negocio.listarTodas();

            foreach (Mesa mesa in mesas)
            {
                if (mesa.EstadoMesa == EstadoMesa.Habilitada && mesa.IdUsuario == 0)
                {
                    ddlMesa.Items.Add(
                        new ListItem(
                            "Mesa " + mesa.IdMesa,
                            mesa.IdMesa.ToString()
                        )
                    );
                }
            }
        }
        public void asignarMozoAMesa(int idMesa, int idUsuarioMozo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE Mesas SET IdUsuario = @idUsuario WHERE IdMesa = @idMesa");

                datos.setearParametros("@idUsuario", idUsuarioMozo);
                datos.setearParametros("@idMesa", idMesa);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // metodo funcionalidad del evento click en el btnAsignar
        protected void btnAsignar_Click(object sender, EventArgs e)
        {
            if (ddlMozo.SelectedValue == "0" || ddlMesa.SelectedValue == "0") return;

            try
            {
                int idUsuarioMozo = int.Parse(ddlMozo.SelectedValue);
                int nroMesa = int.Parse(ddlMesa.SelectedValue);

                MesasNegocio negocio = new MesasNegocio();
                negocio.asignarMozo(nroMesa, idUsuarioMozo);

                CargarMesas();
                ddlMozo.SelectedValue = "0";

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('¡Mesa asignada con éxito!');", true);
            }
            catch (Exception ex)
            {
                // Muestra un cartelito nativo con el error si algo falla
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error: {ex.Message}');", true);
            }
        }
    }
}
