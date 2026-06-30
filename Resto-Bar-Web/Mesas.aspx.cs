using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Dominio;

        

namespace Resto_Bar_Web
{
    public partial class Mesas : System.Web.UI.Page
    {
        public List<Mesa> ListaMesas { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //si no hay usuario logueado, al Login directo
                if (Session["idUsuario"] == null)
                {
                    Session.Add("error", "Debes iniciar sesión para acceder a esta sección.");
                    Response.Redirect("Login.aspx", false);
                    return;
                }
                MesasNegocio negocio = new MesasNegocio();
                List<Mesa> todasLasMesas = negocio.listarTodas();

                int idUsuario = Convert.ToInt32(Session["idUsuario"]);
                int idRol = (Session["idRol"] != null) ? Convert.ToInt32(Session["idRol"]) : -1;

                //todo
                if (idRol == 0 || idRol == 1)
                {
                    ListaMesas = todasLasMesas;
                }
                //solo las mesas que tiene asignadas
                else
                {
                    //Protegemos la consulta con un operador condicional por si listarTodas() vino null
                    ListaMesas = todasLasMesas != null ? todasLasMesas.FindAll(m => m.IdUsuario == idUsuario) : new List<Mesa>();
                }

                if (!IsPostBack)
                {
                    if (ListaMesas != null && ListaMesas.Count > 0)
                    {
                        repeaterMesas.DataSource = ListaMesas;
                        repeaterMesas.DataBind();
                    }
                    else
                    {
                        repeaterMesas.DataSource = null;
                        repeaterMesas.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }

        public string VerificarEstadoMesa(object mesaItem)
        {
            if (mesaItem == null) return "bg-white text-dark";

            Mesa mesa = (Mesa)mesaItem;

            if (mesa.EstadoMesa == Dominio.EstadoMesa.Inhabilitada)
            {
                return "bg-secondary bg-opacity-50 text-dark opacity-50";
            }

            PedidoNegocio pedidoNegocio = new PedidoNegocio();
            List<Pedido> activos = pedidoNegocio.listarPedidosActivos();

            bool tienePedidoActivo = activos.Exists(x => x.NroMesa == mesa.IdMesa);

            if (tienePedidoActivo)
            {
                return "bg-warning bg-opacity-50 text-dark fw-bold border border-warning";
            }

            return "bg-success bg-opacity-25 text-dark";
        }

        protected void repeaterMesas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int idMesa = Convert.ToInt32(e.CommandArgument);
            Session["MesaSeleccionada"] = idMesa;

            if (e.CommandName == "AbrirModal")
            {
                tituloModalMesa.InnerText = "Mesa Número " + idMesa;

                string script = "var elModal = document.getElementById('modalAdministrarMesa'); var modalBootstrap = bootstrap.Modal.getInstance(elModal) || new bootstrap.Modal(elModal); modalBootstrap.show();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", script, true);
            }

            if (e.CommandName == "AbrirPedido")
            {
                try
                {
                    tituloModalPedido.InnerText = "Mesa " + idMesa;

                    // 1. VALIDACIÓN REQUERIDA: Verificar si tiene un mesero activo asignado
                    MesasNegocio mesasNegocio = new MesasNegocio();
                    Mesa mesaActual = mesasNegocio.listarTodas().Find(m => m.IdMesa == idMesa);

                    if (mesaActual != null && mesaActual.IdUsuario == 0)
                    {
                        //Si no hay mesero (IdUsuario == 0), bloqueamos la carga de pedidos
                        btnAgregarPedido.Enabled = false;
                        btnAgregarPedido.CssClass = "btn btn-secondary disabled";
                        pnlAlertaMesero.Visible = true; 
                    }
                    else
                    {
                        btnAgregarPedido.Enabled = true;
                        btnAgregarPedido.CssClass = "btn btn-dark btnZoom";
                        pnlAlertaMesero.Visible = false;
                    }

                    PedidoNegocio pedidoNegocio = new PedidoNegocio();
                    int cerradosHoy;
                    string estadoPedido;

                    pedidoNegocio.ObtenerResumenPedidoPorMesa(idMesa, out cerradosHoy, out estadoPedido);

                    lblCerradosHoy.Text = cerradosHoy.ToString();
                    lblPedidoActual.Text = estadoPedido;

                    if (estadoPedido == "Sí")
                    {
                        btnAgregarPedido.Visible = false;
                        btnVerPedido.Visible = true;
                    }
                    else
                    {
                        // Si está bloqueado por falta de mesero, igual se oculta el botón "Ver Pedido"
                        btnAgregarPedido.Visible = true;
                        btnVerPedido.Visible = false;
                    }

                    upModalesPedido.Update();

                    string script = "var elModal = document.getElementById('modalPedido'); var modalBootstrap = bootstrap.Modal.getInstance(elModal) || new bootstrap.Modal(elModal); modalBootstrap.show();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PopPedido", script, true);
                }
                catch (Exception ex)
                {
                    Session.Add("error", ex.ToString());
                    Response.Redirect("error.aspx", false);
                }
            }
        }

        protected void btnVerPedido_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["MesaSeleccionada"] != null)
                {
                    string mesaSesion = Session["MesaSeleccionada"].ToString();
                    H1.InnerText = "Detalle - Mesa " + mesaSesion;

                    int nroMesa = Convert.ToInt32(mesaSesion);

                    PedidoNegocio pedidoNegocio = new PedidoNegocio();
                    List<Pedido> activos = pedidoNegocio.listarPedidosActivos();
                    Pedido pedidoMesa = activos.Find(x => x.NroMesa == nroMesa);

                    if (pedidoMesa != null)
                    {
                        List<DetallePedido> detalles = pedidoNegocio.listarDetallesPorId(pedidoMesa.IdPedido);
                        dgvDetallePedido.DataSource = detalles;
                        dgvDetallePedido.DataBind();
                    }
                    UpmodalPedidoActual.Update();
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }

        protected void btnAgregarPedido_Click(object sender, EventArgs e)
        {
            CargarOModificarPedido();
        }

        protected void btnGuardarEstado_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["MesaSeleccionada"] != null)
                {
                    MesasNegocio negocio = new MesasNegocio();
                    int nroMesa = Convert.ToInt32(Session["MesaSeleccionada"]);
                    negocio.finalizarAsignacion(nroMesa);
                    Response.Redirect("Mesas.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }

        protected void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            CargarOModificarPedido();
        }

        private void CargarOModificarPedido()
        {
            try
            {
                if (Session["MesaSeleccionada"] != null)
                {
                    int nroMesa = Convert.ToInt32(Session["MesaSeleccionada"]);

                    PedidoNegocio pedidoNegocio = new PedidoNegocio();
                    List<Pedido> activos = pedidoNegocio.listarPedidosActivos();
                    Pedido pedidoMesa = activos.Find(x => x.NroMesa == nroMesa);

                    Session["IdPedido"] = (pedidoMesa != null) ? pedidoMesa.IdPedido : 0;
                }

                Response.Redirect("cargarpedido.aspx", false);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }
        protected void dgvDetallePedido_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                string[] arg = e.CommandArgument.ToString().Split(',');
                int idPedido = Convert.ToInt32(arg[0]);
                int idProducto = Convert.ToInt32(arg[1]);

                PedidoNegocio negocio = new PedidoNegocio();
                negocio.eliminarProductoDelPedido(idPedido, idProducto);

                cargarDetallePedido();
            }
        }
        private void cargarDetallePedido()
        {
            if (Session["MesaSeleccionada"] != null)
            {
                int nroMesa = Convert.ToInt32(Session["MesaSeleccionada"]);
                PedidoNegocio pedidoNegocio = new PedidoNegocio();
                List<Pedido> activos = pedidoNegocio.listarPedidosActivos();
                Pedido pedidoMesa = activos.Find(x => x.NroMesa == nroMesa);

                if (pedidoMesa != null)
                {
                    List<DetallePedido> detalles = pedidoNegocio.listarDetallesPorId(pedidoMesa.IdPedido);
                    dgvDetallePedido.DataSource = detalles;
                    dgvDetallePedido.DataBind();
                }
                else
                {
                    dgvDetallePedido.DataSource = null;
                    dgvDetallePedido.DataBind();
                }
                UpmodalPedidoActual.Update();
            }
        }
        public bool MesaTienePedido(object idMesa)
        {
            int nroMesa = Convert.ToInt32(idMesa);
            MesasNegocio negocio = new MesasNegocio();
            return negocio.mesaTienePedido(nroMesa);
        }

    }
}