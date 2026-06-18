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
    public partial class Insumos : System.Web.UI.Page
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
                    if (!IsPostBack)
                    {
                        CargarProductos();
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Permisos insuficientes');", true);
                    ///redirigir a pagina de error, el mensaje es temporal
                }
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
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error al cargar productos: {ex.Message}');", true);
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
                ////validaciones a desarrollar
                throw ex; 
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
    }
}