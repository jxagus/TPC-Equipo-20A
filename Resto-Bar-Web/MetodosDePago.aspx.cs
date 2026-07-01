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
    public partial class MetodosDePago : System.Web.UI.Page
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
            if (!IsPostBack)
            {
                CargarGrillas();
            }
        }
       
        private void CargarGrillas()
        {
            MetodoPagoNegocio negocio = new MetodoPagoNegocio();
            List<MetodoPago> listaCompleta = negocio.listar();

            dgvMetodosDePago.DataSource = listaCompleta.FindAll(x => x.Activo == true);
            dgvMetodosDePago.DataBind();

            dgvEliminados.DataSource = listaCompleta.FindAll(x => x.Activo == false);
            dgvEliminados.DataBind();
        }

        protected void dgvEliminados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Reactivar")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                MetodoPagoNegocio negocio = new MetodoPagoNegocio();

                negocio.estadoDeMetodoPago(1, id);

                CargarGrillas();
            }
        }

        protected void dgvMetodosDePago_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            MetodoPagoNegocio negocio = new MetodoPagoNegocio();
            int id = Convert.ToInt32(e.CommandArgument);
            ViewState["IdModificando"] = id;
            if (e.CommandName == "Eliminar")
            {
                negocio.estadoDeMetodoPago(0, id);
                CargarGrillas();
            }else if(e.CommandName == "Modificar")
            {
                MetodoPago aux = negocio.buscarPorId(id);
                txtNombre.Text = aux.NombreMetodo;
                btnAgregarMetodoPago.Text = "Modificar";
                btnCancelar.Visible = true;
            }
            
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {

            limpiarBotonesyTxt();

        }

        protected void btnAgregarMetodoPago_Click(object sender, EventArgs e)
        {
            MetodoPagoNegocio negocio = new MetodoPagoNegocio();
            try
            {
                if (negocio.verificarExistencia(txtNombre.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Ya existe ese Metodo de Pago');", true);
                    return;
                }
                if (btnAgregarMetodoPago.Text == "Agregar Metodo")
                {
                    string nombreMetodo = txtNombre.Text;
                    negocio.agregarMetodoPago(nombreMetodo);
                    txtNombre.Text = string.Empty;
                }
                else
                {
                    int idModificado = Convert.ToInt32(ViewState["IdModificando"]);
                    string nombreMetodo = txtNombre.Text;
                    negocio.modificarMetodoPago(nombreMetodo, idModificado);

                }
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Categoria agregada/modificada exitosamente!');", true);
                limpiarBotonesyTxt();
                CargarGrillas();
            }
            catch (Exception ex )
            {

                throw ex;
            }
        }

        protected void limpiarBotonesyTxt()
        {
            ViewState["IdModificando"] = "";
            btnAgregarMetodoPago.Text = "Agregar Metodo";
            txtNombre.Text = string.Empty;
            btnCancelar.Visible = false;
        }

        protected void dgvEliminados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvEliminados.PageIndex = e.NewPageIndex;
            CargarGrillas();
        }

        protected void dgvMetodosDePago_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvMetodosDePago.PageIndex = e.NewPageIndex;
            CargarGrillas();
        }
    }
}
