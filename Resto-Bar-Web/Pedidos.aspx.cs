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
                cargarMetodosDePago();
            }
        }

        private void CargarColaPedidos()
        {
            try
            {
                PedidoNegocio negocio = new PedidoNegocio();
                List<Pedido> listaActivos = negocio.listarPedidosActivos();

                Session["ListaPedidosActivos"] = listaActivos;

                int mesaFiltrar; //para mantener el filtro
                if (!string.IsNullOrEmpty(txtFiltroMesa.Text.Trim()) && int.TryParse(txtFiltroMesa.Text.Trim(), out mesaFiltrar) && mesaFiltrar >= 1)
                {
                    listaActivos = listaActivos.FindAll(p => p.NroMesa == mesaFiltrar);
                }

                RenderizarLista(listaActivos);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }

        private void RenderizarLista(List<Pedido> lista)
        {
            if (lista == null || lista.Count == 0)
            {
                pnlVacio.CssClass = "text-center py-5";
                repPedidos.DataSource = null;
            }
            else
            {
                pnlVacio.CssClass = "text-center py-5 d-none";
                repPedidos.DataSource = lista;
            }
            repPedidos.DataBind();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                //por las dudas
                lblErrorFiltro.Visible = false;
                lblErrorFiltro.Text = "";

                if (string.IsNullOrEmpty(txtFiltroMesa.Text.Trim()))
                {
                    CargarColaPedidos();
                    return;
                }

                int mesaFiltrar;
                //Intentamos parsear para filtrar letras
                bool esNumeroValido = int.TryParse(txtFiltroMesa.Text.Trim(), out mesaFiltrar);

                //letras/simbolos/negativos
                if (!esNumeroValido || mesaFiltrar < 1)
                {
                    lblErrorFiltro.Text = "❌ ¡Solo números positivos!";
                    lblErrorFiltro.Visible = true;

                    //Renderizamos una lista vacia para denotar visualmente que fallo la operación
                    RenderizarLista(new List<Pedido>());
                    return;
                }

                PedidoNegocio negocio = new PedidoNegocio();
                List<Pedido> listaActivos = negocio.listarPedidosActivos();
                List<Pedido> listaFiltrada = listaActivos.FindAll(p => p.NroMesa == mesaFiltrar);

                RenderizarLista(listaFiltrada);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }

        protected void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                txtFiltroMesa.Text = "";
                lblErrorFiltro.Visible = false;
                lblErrorFiltro.Text = "";
                CargarColaPedidos();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }

        protected void repPedidos_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            int idPedido = Convert.ToInt32(e.CommandArgument);
            ViewState["idPedidoFinalizar"] = idPedido;

            if (e.CommandName == "FinalizarPedido")
            {
                try
                {
                    ddlMetodosDePago.SelectedIndex = 0;
                    PedidoNegocio negocio = new PedidoNegocio();

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

                    //negocio.finalizarPedido(idPedido);
                    CargarColaPedidos();

                    string script = "var miModal = new bootstrap.Modal(document.getElementById('modalExitoPedido')); miModal.show();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PopFactura", script, true);
                }
                catch (Exception ex)
                {
                    Session.Add("error", ex.ToString());
                    Response.Redirect("error.aspx", false);
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
                    Session.Add("error", ex.ToString());
                    Response.Redirect("error.aspx", false);
                }
            }
        }

        public bool DeterminarVisibilidadMonto()
        {
            if (Session["idRol"] != null)
            {
                int rolUsuario = Convert.ToInt32(Session["idRol"]);

                if (rolUsuario == 0 || rolUsuario == 1)
                {
                    return true;
                }
            }
            return false;
        }

        protected void btnFinalizarPedido_Click(object sender, EventArgs e)
        {
            if(ddlMetodosDePago.SelectedIndex == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error: Metodo de Pago Invlado. Intente Nuevamente');", true);
                return;
            }
            try
            {
                int idCategoria = Convert.ToInt32(ddlMetodosDePago.SelectedValue);
                int idPedido = Convert.ToInt32(ViewState["idPedidoFinalizar"]);
                PedidoNegocio negocio = new PedidoNegocio();
                negocio.finalizarPedido(idPedido, idCategoria);
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Pedido Finalizado Exitosamente!');", true);
                CargarColaPedidos();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);

                throw;
            }
        }

        private void cargarMetodosDePago()
        {
            MetodoPagoNegocio negocio = new MetodoPagoNegocio();
            List<MetodoPago> todos = negocio.listar();
            List<MetodoPago> metodosActivos = todos.FindAll(x => x.Activo == true);
            ddlMetodosDePago.DataSource = metodosActivos;
            ddlMetodosDePago.DataValueField = "IdMetodo";
            ddlMetodosDePago.DataTextField = "NombreMetodo";
            ddlMetodosDePago.DataBind();
            ddlMetodosDePago.Items.Insert(0, new ListItem("Seleccione...", "0"));
        }
    }
}