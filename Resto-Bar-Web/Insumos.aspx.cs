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

                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Permisos insuficientes');", true);
                    ///redirigir a pagina de error, el mensaje es temporal
                }
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
                negocio.agregarProducto(nombre, descripcion, precio, stock);

                txtDescripcion.Text = null;
                txtNombreProducto.Text = null;
                txtPrecio.Text = null;
                txtStock.Text = null;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Producto agregado exitosamente!');", true);

            }
            catch (Exception ex)
            {
                ////validaciones a desarrollar
                throw ex; 
            }
        }
    }
}