using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using System.EnterpriseServices;

namespace Resto_Bar_Web
{
    public partial class Categorias : System.Web.UI.Page
    {
        private List<Dominio.Categorias> listaTodas;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["idUsuario"] == null)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error: no esta iniciado sesion');", true);
                Response.Redirect("Login.aspx");

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
                        cargarListadoCategorias();
                        
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
            if (!Page.IsValid)
            {
                return;
            }

            if (btnAgregarCategoria.Text == "Agregar Categoria")
            {
                if (!chkSubcategoria.Checked)
                {
                    negocio.agregarCategoria(txtNombreCategoria.Text);
                }
                else
                {
                    int idPadre = Convert.ToInt32(ddlCategoriaPadre.SelectedValue);
                    negocio.agregarSubcategoria(txtNombreCategoria.Text, idPadre);
                }
            }
            else if (btnAgregarCategoria.Text == "Modificar")
            {
                int idSeleccionado = Convert.ToInt32(ViewState["IdCategoriaAEditar"]);
                if (!chkSubcategoria.Checked)
                {
                    negocio.modificarCategoria(txtNombreCategoria.Text, idSeleccionado);
                }
                else
                {
                    if (negocio.tieneSubcategorias(idSeleccionado))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('No se puede convertir en Subcategoria una Categoria que contiene Subcategorias.');", true);
                        return;
                    }

                    int idPadre = Convert.ToInt32(ddlCategoriaPadre.SelectedValue);
                    negocio.modificarSubategoria(txtNombreCategoria.Text, idPadre, idSeleccionado);
                }
            }


            ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Categoria Agregada/Modificada con exito!.');", true);
            txtNombreCategoria.Text = string.Empty;
            cargarListadoCategorias();
            cargarSubcategorias();
            chkSubcategoria.Checked = false;
            divSubcategorias.Visible = false;
            btnAgregarCategoria.Text = "Agregar Categoria";
            ViewState["IdCategoriaAEditar"] = null;
            btnCancelar.Visible = false;

        }



        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            txtNombreCategoria.Text = string.Empty;
            cargarListadoCategorias();
            cargarSubcategorias();
            chkSubcategoria.Checked = false;
            divSubcategorias.Visible = false;
            btnCancelar.Visible = false;
            btnAgregarCategoria.Text = "Agregar Categoria";
            ViewState["IdCategoriaAEditar"] = null;
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

        protected void repCategorias_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Dominio.Categorias categoriaPadre = (Dominio.Categorias)e.Item.DataItem;
                Repeater repSubcategorias = (Repeater)e.Item.FindControl("repSubcategorias");
                if(listaTodas == null)
                {
                    CategoriaNegocio catNegocio = new CategoriaNegocio();
                    listaTodas = catNegocio.listarTODAS();
                }

                List<Dominio.Categorias> subcategorias = listaTodas.FindAll(c => c.IdCategoriaPadre == categoriaPadre.IdCategoria);
                if(subcategorias.Count > 0)
                {
                    repSubcategorias.DataSource = subcategorias;
                    repSubcategorias.DataBind();
                }
            }
        }

        private void cargarListadoCategorias()
        {
            CategoriaNegocio catNegocio = new CategoriaNegocio();

            listaTodas = catNegocio.listarTODAS();
            List<Dominio.Categorias> listaCategorias = listaTodas.FindAll(c => c.IdCategoriaPadre == 0);

            repCategorias.DataSource = listaCategorias;
            repCategorias.DataBind();
        }

        protected void repCategorias_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if(e.CommandName == "EditarCategoria")
            {
                int idCategoria = Convert.ToInt32(e.CommandArgument);
                ViewState["IdCategoriaAEditar"] = idCategoria;

                cargarFormulario(idCategoria);
                btnCancelar.Visible = true;
                btnAgregarCategoria.Text = "Modificar";
            }
        }
        void cargarFormulario(int id)
        {
            Dominio.Categorias categoria = new Dominio.Categorias();
            CategoriaNegocio negocio = new CategoriaNegocio();
            categoria = negocio.cargarCategoriaPorId(id);

            txtNombreCategoria.Text = categoria.NombreCategoria;
            if (categoria.IdCategoriaPadre != 0)
            {
                chkSubcategoria.Checked = true;
                divSubcategorias.Visible = chkSubcategoria.Checked;
                ddlCategoriaPadre.SelectedValue = categoria.IdCategoriaPadre.ToString();
            }
            else
            {
                chkSubcategoria.Checked = false;
            }

        }

        protected void repSubcategorias_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "EditarSubcategoria")
            {
                int idCategoria = Convert.ToInt32(e.CommandArgument);
                ViewState["IdCategoriaAEditar"] = idCategoria;

                cargarFormulario(idCategoria);
                btnCancelar.Visible = true;
                btnAgregarCategoria.Text = "Modificar";
            }
        }
    }
}