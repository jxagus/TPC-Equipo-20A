using System;
using System.Collections.Generic;
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
                string script = "alert('Permisos insuficientes'); window.location.href = 'Dashboard.aspx';";
                ClientScript.RegisterStartupScript(this.GetType(), "alertPermisos", script, true);
            }
            if (!IsPostBack)
            {
                CargarGrillas();
            }
        }
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            lblMensajeError.Text = "";

            ///  campos vacíos
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtNombreUsuario.Text) || string.IsNullOrWhiteSpace(txtContrasena.Text) ||
                string.IsNullOrWhiteSpace(txtRepetirContrasena.Text))
            {
                lblMensajeError.Text = "Todos los campos son obligatorios.";
                return;
            }

            ///contraseñas 
            if (txtContrasena.Text != txtRepetirContrasena.Text)
            {
                lblMensajeError.Text = "Las contraseñas no coinciden.";
                return;
            }

            ///usuario duplicado
            UsuariosNegocio negocio = new UsuariosNegocio();
            if (negocio.ExisteNombreUsuario(txtNombreUsuario.Text))
            {
                lblMensajeError.Text = "El nombre de usuario ya existe.";
                return;
            }

            ///Registro
            try
            {
                Usuario nuevoUsuario = new Usuario();
                nuevoUsuario.Nombre = txtNombre.Text;
                nuevoUsuario.Apellido = txtApellido.Text;
                nuevoUsuario.NombreUsuario = txtNombreUsuario.Text;
                nuevoUsuario.Contrasena = txtContrasena.Text;
                nuevoUsuario.IdRol = Convert.ToInt32(ddlRol.SelectedValue);
                nuevoUsuario.Estado = true;

                negocio.agregarNuevo(nuevoUsuario);

                Response.Redirect("CrearPersonal.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Session["error"] = ex.Message;
                Response.Redirect("error.aspx", false);
            }
        }
        private void CargarGrillas()
        {
            UsuariosNegocio negocio = new UsuariosNegocio();
            List<Usuario> listaCompleta = negocio.listar();

            //activos
            dgvUsuarios.DataSource = listaCompleta.FindAll(x => x.Estado == true);
            dgvUsuarios.DataBind();

            //eliminados
            dgvEliminados.DataSource = listaCompleta.FindAll(x => x.Estado == false);
            dgvEliminados.DataBind();
        }
        protected void dgvUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvUsuarios.PageIndex = e.NewPageIndex;
            CargarGrillas();
        }
        protected void dgvEliminados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvEliminados.PageIndex = e.NewPageIndex;
            CargarGrillas();
        }
        protected void dgvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                int idUsuario = Convert.ToInt32(e.CommandArgument);
                UsuariosNegocio negocio = new UsuariosNegocio();
                negocio.eliminar(idUsuario); 
                CargarGrillas();
            }
        }
        protected string ObtenerNombreRol(int idRol)
        {
            switch (idRol)
            {
                case 0: return "Admin";
                case 1: return "Gerente";
                case 2: return "Mozo";
                default: return "Desconocido";
            }
        }
        protected void dgvEliminados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Reactivar")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                UsuariosNegocio negocio = new UsuariosNegocio();

                negocio.reactivar(id);

                CargarGrillas(); 
            }
        }
    }
    
    
}