using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace Resto_Bar_Web
{
    public partial class CrearPersonal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUsuario"] == null)
            {
                Response.Redirect("Login.aspx"); 
                return;
            }

            int idRolLogueado = Convert.ToInt32(Session["idRol"]);

            if (idRolLogueado == 2)
            {
                Response.Redirect("Mesas.aspx");
            }
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            Usuario nuevoUsuario = new Usuario();

            nuevoUsuario.NombreCompleto = txtNombre.Text;
            nuevoUsuario.NombreUsuario = txtNombreUsuario.Text;
            nuevoUsuario.Contrasena = txtContrasena.Text;
            nuevoUsuario.IdRol = Convert.ToInt32(ddlRol.SelectedValue);

            UsuariosNegocio negocio = new UsuariosNegocio();

            try
            {
                negocio.agregarNuevo(nuevoUsuario);
                Response.Redirect("Mesas.aspx");
            }
            catch (Exception ex)
            {
                // Si hay un error de conexión o sintaxis SQL, rompemos la pantalla para verlo al toque
                Response.Write("<h2 style='color:red;'>Error al guardar personal:</h2>");
                Response.Write("<pre>" + ex.Message + "</pre>");
                Response.End();
            }
        }
    }
    
    
}