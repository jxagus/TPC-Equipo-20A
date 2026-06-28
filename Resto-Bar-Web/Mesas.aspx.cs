using Antlr.Runtime.Misc;
using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Resto_Bar_Web
{
    public partial class Mesas : System.Web.UI.Page
    {
        public List<Mesa> ListaMesas { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            MesasNegocio negocio = new MesasNegocio();
            ListaMesas = negocio.listarTodas();
            if (!IsPostBack)
            {
                repeaterMesas.DataSource = ListaMesas;
                repeaterMesas.DataBind();
            }

        }

        public string VerificarEstadoMesa(object estado)
        {
            Dominio.EstadoMesa estadoMesa = (Dominio.EstadoMesa)estado;

            string cartaColor = "bg-success bg-opacity-50 text-white";

            if (estadoMesa == Dominio.EstadoMesa.Inhabilitada)
            {
                cartaColor = "bg-secondary bg-opacity-50 text-dark";
            }
            else
            {
                bool tienePedidoActivo = false; /*TEMPORAL PARA PROBAR, DESPUES SE LE VA A AGREGAR FUNCIONALIDAD PARA VERIFICARLO REALMENTE*/
                if (tienePedidoActivo)
                {
                    cartaColor = "bg-warning bg-opacity-50 text-dark";
                }
            }
            return cartaColor;
        }

        protected void repeaterMesas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "AbrirModal")
            {

                string idMesa = e.CommandArgument.ToString();
                Session["MesaSeleccionada"] = idMesa;
                tituloModalMesa.InnerText = "Mesa Numero " + idMesa;

                string script = "var elModal = document.getElementById('modalAdministrarMesa'); var modalBootstrap = bootstrap.Modal.getInstance(elModal) || new bootstrap.Modal(elModal); modalBootstrap.show();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", script, true);
            }
            if (e.CommandName == "AbrirPedido")
            {
                string idMesa = e.CommandArgument.ToString();
                Session["MesaSeleccionada"] = idMesa;
                AccesoDatos datos = new AccesoDatos();

                try
                {
                    datos.setearConsulta(@"SELECT 
                        COUNT(CASE WHEN CAST(FechayHoraPedido AS DATE) = CAST(GETDATE() AS DATE) AND IdEstadoPedido = 2 THEN 1 END) AS PedidosCerradosHoy,
                        CASE WHEN EXISTS (SELECT 1 FROM dbo.Pedidos 
                        WHERE NroMesa = @NroMesa AND IdEstadoPedido = 1) THEN 'Sí' ELSE 'No' END AS PedidoActual
                        FROM dbo.Pedidos
                        WHERE NroMesa = @NroMesa");
                    datos.setearParametros("@NroMesa", idMesa);
                    datos.ejecutarLectura();

                    if (datos.Lector.Read())
                    {
                        string estadoPedido = datos.Lector["PedidoActual"].ToString();
                        lblCerradosHoy.Text = datos.Lector["PedidosCerradosHoy"].ToString();
                        lblPedidoActual.Text = estadoPedido;
                        if (estadoPedido == "Sí")
                        {
                            btnAgregarPedido.Visible = false;
                            btnVerPedido.Visible = true; //se nos va mostrar el detalle del pedido actual
                        }
                        else
                        {
                            btnAgregarPedido.Visible = true;
                            btnVerPedido.Visible = false;
                        }
                    }
                    else
                    {
                        lblCerradosHoy.Text = "0";
                        lblPedidoActual.Text = "No";
                        btnVerPedido.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    datos.cerrarConexion();
                }
                tituloModalPedido.InnerText = "Mesa " + idMesa;

                upModalesPedido.Update();
                tituloModalPedido.InnerText = "Mesa " + idMesa;

                string script = "var elModal = document.getElementById('modalPedido'); var modalBootstrap = bootstrap.Modal.getInstance(elModal) || new bootstrap.Modal(elModal); modalBootstrap.show();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PopPedido", script, true);
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
                throw ex;
            }
        }

        protected void btnAgregarPedido_Click(object sender, EventArgs e)
        {
            CargarOModificarPedido(); //creacion de metodo debido a que tanto este evento como el de agregarProducto van a realizar practicamente lo mismo pero distinguido por un if 
        }

        protected void btnGuardarEstado_Click(object sender, EventArgs e)
        {
            if (Session["MesaSeleccionada"] != null)
            {
                MesasNegocio negocio = new MesasNegocio();

                int nroMesa = Convert.ToInt32(Session["MesaSeleccionada"]);
                negocio.finalizarAsignacion(nroMesa);
                Response.Redirect("Mesas.aspx");
            }
        }

        protected void btnCerrarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["MesaSeleccionada"] != null)
                {
                    string mesaSesion = Session["MesaSeleccionada"].ToString();
                    int nroMesa = Convert.ToInt32(mesaSesion);

                    PedidoNegocio pedidoNegocio = new PedidoNegocio();
                    List<Pedido> activos = pedidoNegocio.listarPedidosActivos();
                    Pedido pedidoMesa = activos.Find(x => x.NroMesa == nroMesa);

                    if (pedidoMesa != null)
                    {
                        List<DetallePedido> detalles = pedidoNegocio.listarDetallesPorId(pedidoMesa.IdPedido);


                        lblFacturaMesa.Text = nroMesa.ToString();
                        lblFacturaIdPedido.Text = pedidoMesa.IdPedido.ToString();

                        repFacturaItems.DataSource = detalles;
                        repFacturaItems.DataBind();

                        decimal total = 0;
                        foreach (var item in detalles)
                        {
                            total += Convert.ToDecimal(item.Subtotal);
                        }
                        lblFacturaTotal.Text = total.ToString("N2");
                        pedidoNegocio.finalizarPedido(pedidoMesa.IdPedido);
                        upModalFactura.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
            }
        }
    }
}