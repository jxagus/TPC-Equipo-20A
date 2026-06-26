using Dominio;
using Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Resto_Bar_Web
{
    public partial class Insumos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["idUsuario"] == null)
                {
                    Response.Redirect("Login.aspx", false);
                    return;
                }

                LoginNegocio negocio = new LoginNegocio();
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);
                int rol = negocio.traerRol(idUsuario);

                if (rol == 0 || rol == 1)
                {
                    if (!IsPostBack)
                    {
                        CargarProductos();
                    }
                }
                else
                {

                    throw new Exception("Permisos insuficientes: Su nivel de usuario no está autorizado para acceder a la gestión de productos.");
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }
        private void CargarProductos()
        {
            try
            {
                ProductoNegocio negocio = new ProductoNegocio();

                dgvProductos.DataSource = negocio.listar();

                dgvProductos.DataBind();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }
        protected void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                
                string nombre = txtNombreProducto.Text;
                string descripcion = txtDescripcion.Text;
                decimal precio = Convert.ToDecimal(txtPrecio.Text);
                int stock = Convert.ToInt32(txtStock.Text);

                ProductoNegocio negocio = new ProductoNegocio();
                if (btnAgregarProducto.Text == "Modificar")
                {
                    int id = Convert.ToInt32(hfIdProducto.Value);
                    negocio.modificarProducto(id, nombre, descripcion, precio, stock);
                    hfIdProducto.Value = "";
                }
                else
                {
                    negocio.agregarProducto(nombre, descripcion, precio, stock);
                }

                txtDescripcion.Text = null;
                txtNombreProducto.Text = null;
                txtPrecio.Text = null;
                txtStock.Text = null;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Producto agregado exitosamente!');", true);

                CargarProductos();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }

        protected void lbtnEditar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int idSeleccionado = Convert.ToInt32(btn.CommandArgument);

            txtId.Text = idSeleccionado.ToString();
            hfIdProducto.Value = idSeleccionado.ToString();
            IdOculto.Visible = true;
            btnCancelar.Visible = true;
            btnAgregarProducto.Text = "Modificar";
            cargarFormulario(idSeleccionado);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            txtNombreProducto.Text = null;
            txtDescripcion.Text = null;
            txtPrecio.Text = null;
            txtStock.Text = null;
            IdOculto.Visible = false;
            btnCancelar.Visible = false;
            btnAgregarProducto.Text = "Agregar Producto";
        }
        
        void cargarFormulario(int id)
        {
            Productos producto = new Productos();
            ProductoNegocio negocio = new ProductoNegocio();
            producto = negocio.cargarProductoPorId(id);
            
            txtNombreProducto.Text = producto.NombreProducto;
            txtDescripcion.Text = producto.DescripcionProducto;
            txtPrecio.Text = producto.Precio.ToString();
            txtStock.Text = producto.Stock.ToString();
        
        }

        protected void lbtnEliminar_Click(object sender, EventArgs e)
        {

        }

        protected void btnAplicarEliminarProducto_Click(object sender, EventArgs e)
        {
            ProductoNegocio negocio = new ProductoNegocio();
            int id = Convert.ToInt32(hfIdProducto.Value);
            negocio.desactivarProducto(id);
            hfIdProducto.Value = "";
            CargarProductos();

        }

        protected void dgvProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AbrirModalProducto")
            {

                string idProducto = e.CommandArgument.ToString();
                Session["IdProductoAEliminar"] = idProducto;
                hfIdProducto.Value = idProducto;

                ProductoNegocio negocio = new ProductoNegocio();
                Productos seleccionado = negocio.cargarProductoPorId(int.Parse(idProducto));
                
                lblEliminarId.Text = idProducto;
                lblEliminarNombre.Text = seleccionado.NombreProducto;
                lblEliminarDescripcion.Text = seleccionado.DescripcionProducto.ToString();
                lblEliminarPrecio.Text = "$" + seleccionado.Precio.ToString("0.00");
                lblEliminarStock.Text = seleccionado.Stock.ToString();



                string script = "var miModal = new bootstrap.Modal(document.getElementById('modalEliminarProducto')); miModal.show();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PopProducto", script, true);
            }
        }

        protected void btnAgregarCategoria_Click(object sender, EventArgs e)
        {

        }

        protected void btnCrearCategoria_Click(object sender, EventArgs e)
        {

        }
    }
}