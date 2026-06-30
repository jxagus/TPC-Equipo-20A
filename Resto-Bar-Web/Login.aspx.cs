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
            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Text;

            LoginNegocio negocio = new LoginNegocio();

            try
            {
                Usuario usuarioLogueado = negocio.ValidarLogin(usuario, contrasena);

                if (usuarioLogueado != null)
                {
                    //ESTADO
                    if (usuarioLogueado.Estado == false)
                    {
                        lblError.Text = "⚠️ Esta cuenta se encuentra inactiva.";
                        lblError.Visible = true;
                        return;
                    }

                    Session.Add("idUsuario", usuarioLogueado.IdUsuario);
                    Session.Add("idRol", usuarioLogueado.IdRol);

                    Response.Redirect("Dashboard.aspx", false);
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
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            Response.Redirect("Login.aspx", false);
        }

        protected void btnIrDashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("Dashboard.aspx", false);
        }
    }
}