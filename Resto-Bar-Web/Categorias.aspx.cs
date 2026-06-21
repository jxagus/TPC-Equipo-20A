using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Resto_Bar_Web
{
    public partial class Categorias : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUsuario"] == null)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error: no esta iniciado sesion');", true);
                Response.Redirect("Usuarios.aspx");

                ///redirigir a una pagian de error, el mensaje es temporal
            }
            else
            {
                LoginNegocio negocio = new LoginNegocio();
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);
                int rol = negocio.traerRol(idUsuario);
                if (rol == 0 || rol == 1)
                {
                    if (!IsPostBack)
                    {
                        cargarSubcategorias();
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Permisos insuficientes');", true);
                    ///redirigir a pagina de error, el mensaje es temporal
                }
            }
        }

        protected void btnAgregarCategoria_Click(object sender, EventArgs e)
        {
            CategoriaNegocio negocio = new CategoriaNegocio();

            if(!chkSubcategoria.Checked) {
                negocio.agregarCategoria(txtNombreCategoria.Text);
            }
            else
            {
                int idPadre = Convert.ToInt32(ddlCategoriaPadre.SelectedValue);
                negocio.agregarSubcategoria(txtNombreCategoria.Text, idPadre);
            }

            txtNombreCategoria.Text = string.Empty;
            cargarSubcategorias();
            chkSubcategoria.Checked = false;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ///Para cuando este la opcion de editar categoria
        }

        private void cargarSubcategorias()
        {
            CategoriaNegocio negocio = new CategoriaNegocio();
            ddlCategoriaPadre.DataSource = negocio.CargarCategorias();
            ddlCategoriaPadre.DataValueField = "IdCategoria";
            ddlCategoriaPadre.DataTextField = "NombreCategoria";
            ddlCategoriaPadre.DataBind();
        }

        protected void chkSubcategoria_CheckedChanged(object sender, EventArgs e)
        {
            divSubcategorias.Visible = chkSubcategoria.Checked;
        }
    }
}