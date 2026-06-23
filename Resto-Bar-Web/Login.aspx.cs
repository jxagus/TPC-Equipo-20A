using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace Resto_Bar_Web
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["idUsuario"] != null)
                {
                    pnlLogin.Visible = false;
                    pnlPerfil.Visible = true;
                }
                else
                {
                    pnlLogin.Visible = true;
                    pnlPerfil.Visible = false;
                }
            }
        }

        protected void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;
            lblError.Text = "";

            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Text;

            LoginNegocio negocio = new LoginNegocio();

            try
            {
                int idUsuario = negocio.existeUsuario(usuario, contrasena);

                if (idUsuario != 0)
                {
                    Session.Add("idUsuario", idUsuario);
                    Session.Add("idRol", negocio.traerRol(idUsuario));

                    Response.Redirect("~/");
                }
                else
                {
                    lblError.Text = "⚠️ Usuario y/o Contraseña incorrectos.";
                    lblError.Visible = true;
                    txtContrasena.Text = null;
                }
            }
            catch (Exception ex)
            {
                //por si la db tira error
                lblError.Text = "Error de conexión con el servidor.";
                lblError.Visible = true;
            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
        protected void btnIrDashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/");
        }
    }
}