using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
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
            if (Session["idUsuario"] != null)
            {
                //// a desarrollar
            }
        }

        protected void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text;
            string contrasena = txtContrasena.Text;

            LoginNegocio negocio = new LoginNegocio();

            int idUsuario = negocio.existeUsuario(usuario, contrasena);
            if (idUsuario != 0)
            {
                Session.Add("idUsuario", idUsuario);
                Session.Add("idRol", negocio.traerRol(idUsuario));
                Response.Redirect("~/Mesas.aspx");

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Usuario y/o Contrasena Incorrectos');", true);
            }

            txtContrasena.Text = null;
            txtUsuario.Text = null;
        }
    }
}