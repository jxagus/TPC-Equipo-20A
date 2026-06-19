using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace Resto_Bar_Web
{
    public partial class Pedidos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUsuario"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarColaPedidos();
            }
        }

        private void CargarColaPedidos()
        {
            try
            {
                PedidoNegocio negocio = new PedidoNegocio();
                List<Pedido> listaActivos = negocio.listarPedidosActivos();

                if (listaActivos.Count == 0)
                {
                    pnlVacio.CssClass = "text-center py-5";
                    repPedidos.DataSource = null;
                }
                else
                {
                    pnlVacio.CssClass = "text-center py-5 d-none";
                    repPedidos.DataSource = listaActivos;
                }

                repPedidos.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al cargar la cola de pedidos: " + ex.Message + "');", true);
            }
        }

        protected void repPedidos_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "FinalizarPedido")
            {
                try
                {
                    int idPedido = Convert.ToInt32(e.CommandArgument);
                    PedidoNegocio negocio = new PedidoNegocio();
                    negocio.finalizarPedido(idPedido);

                    CargarColaPedidos();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('🛎️ Pedido #" + idPedido + " marcado como entregado con éxito.');", true);
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al finalizar el pedido: " + ex.Message + "');", true);
                }
            }
        }
    }
}