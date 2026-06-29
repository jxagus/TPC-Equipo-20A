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
            int idPedido = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "FinalizarPedido")
            {
                try
                {
                    PedidoNegocio negocio = new PedidoNegocio();

                    //buscamos el pedido activo para sacar NroMesa antes de finalizarlo
                    List<Pedido> activos = negocio.listarPedidosActivos();
                    Pedido pedidoActual = activos.Find(p => p.IdPedido == idPedido);

                    List<DetallePedido> detalles = negocio.listarDetallesPorId(idPedido);

                    lblFacturaMesa.Text = (pedidoActual != null) ? pedidoActual.NroMesa.ToString() : "-";
                    lblFacturaIdPedido.Text = idPedido.ToString();

                    repFacturaItems.DataSource = detalles;
                    repFacturaItems.DataBind();

                    decimal totalFactura = 0;
                    foreach (var item in detalles)
                    {
                        totalFactura += Convert.ToDecimal(item.Subtotal);
                    }
                    lblFacturaTotal.Text = totalFactura.ToString("N2");

                    negocio.finalizarPedido(idPedido);
                    CargarColaPedidos();

                    string script = "var miModal = new bootstrap.Modal(document.getElementById('modalExitoPedido')); miModal.show();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PopFactura", script, true);
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: " + ex.Message + "');", true);
                }
            }
            else if (e.CommandName == "VerDetalle")
            {
                try
                {
                    lblModalIdPedido.Text = idPedido.ToString();

                    PedidoNegocio negocio = new PedidoNegocio();
                    List<DetallePedido> detalles = negocio.listarDetallesPorId(idPedido);

                    dgvDetallePedido.DataSource = detalles;
                    dgvDetallePedido.DataBind();

                    string script = "var miModal = new bootstrap.Modal(document.getElementById('modalDetallePedido')); miModal.show();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PopDetalle", script, true);
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al cargar el detalle: " + ex.Message + "');", true);
                }
            }
        }
        public bool DeterminarVisibilidadMonto()
        {
            if (Session["idRol"] != null)
            {
                int rolUsuario = Convert.ToInt32(Session["idRol"]);

                //Admin (0) o Gerente (1)
                if (rolUsuario == 0 || rolUsuario == 1)
                {
                    return true;
                }
            }
            return false;
        }
    }

}