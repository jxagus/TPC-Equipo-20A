using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace Resto_Bar_Web
{
    public partial class CargarPedido : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["idUsuario"] == null)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: no esta iniciado sesion');", true);
                    Response.Redirect("login.aspx", false);
                    return;
                }

                if (!IsPostBack)
                {
                    if (Session["Carrito"] == null)
                    {
                        Session["Carrito"] = new List<DetallePedido>();
                    }

                    EstablecerMesaSeleccionada();
                    CargarCardsProductos();
                    ActualizarInterfazCarrito();
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
                throw ex;
            }
        }

        private void EstablecerMesaSeleccionada()
        {
            try
            {
                string mesaEnCarrito = Session["MesaEnCarrito"]?.ToString(); //mesa en carrito la cual esta persistiendo si no se da con el evento del btn finalizar pedido
                string mesaActual = Session["MesaSeleccionada"]?.ToString();   // la session MesaSeleccionada para marcar la mesa actual, donde guardamos una modificacion dentro de la mesa seleccionada (en la page de mesas)

                if (!string.IsNullOrEmpty(Request.QueryString["idMesa"]))
                {
                    mesaActual = Request.QueryString["idMesa"];
                    Session["MesaSeleccionada"] = mesaActual; //guardamos en session la mesa actual
                }
                if (mesaActual != null)
                {
                    lblMesaSeleccionada.Text = "Mesa Nro " + mesaActual; //esto pq en la session ya se encuentra guardado la mesa actual
                }
                else
                {
                    lblMesaSeleccionada.Text = "No seleccionada";
                    lblMesaSeleccionada.CssClass = "badge bg-danger fs-5 px-3 py-2";
                }
                if (mesaEnCarrito != null && mesaActual != null && mesaEnCarrito != mesaActual) // comparamos mesa anterior con mesa actual (ademas marcamos si ambas son distintas a null), dentro de la session, para poder limpiar la interfaz del carrito si la misma no es confirmada y se procede con otro pedido con distinto idMesa
                {
                    Session["Carrito"] = new List<DetallePedido>();
                    Session["IdPedido"] = null; //marcamos idpedido como null
                }
                Session["MesaEnCarrito"] = mesaActual;
                ActualizarInterfazCarrito();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }

        protected void btnConfirmarPedido_Click(object sender, EventArgs e)
        {
            List<DetallePedido> carrito = (List<DetallePedido>)Session["Carrito"];

            if (carrito == null || carrito.Count == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('El pedido esta vacio.');", true);
                return;
            }

            try
            {
                int idPedidoActual = (Session["IdPedido"] != null) ? Convert.ToInt32(Session["IdPedido"]) : 0;
                PedidoNegocio pedidoNegocio = new PedidoNegocio();

                if (idPedidoActual > 0)
                {
                    foreach (DetallePedido item in carrito)
                    {
                        pedidoNegocio.modificarPedido(idPedidoActual, item.IdProducto, item.Cantidad, item.PrecioUnitario);
                        pedidoNegocio.restarStock(item.IdProducto, item.Cantidad);
                    }
                }
                else
                {
                    decimal precioTotal = 0;
                    foreach (DetallePedido item in carrito)
                    {
                        precioTotal = precioTotal + item.Subtotal;
                    }

                    int nroMesaSeleccionada = 0;
                    if (Session["MesaSeleccionada"] != null)
                    {
                        nroMesaSeleccionada = Convert.ToInt32(Session["MesaSeleccionada"]);
                    }

                    Pedido nuevoPedido = new Pedido();
                    nuevoPedido.NroMesa = nroMesaSeleccionada;
                    nuevoPedido.IdUsuario = Convert.ToInt32(Session["idUsuario"]);
                    nuevoPedido.FechayHoraPedido = DateTime.Now;
                    nuevoPedido.PrecioTotal = precioTotal;
                    nuevoPedido.IdEstadoPedido = 1; // 1 Pendiente

                    int idPedidoGenerado = pedidoNegocio.agregarPedido(nuevoPedido);

                    foreach (DetallePedido item in carrito)
                    {
                        item.IdPedido = idPedidoGenerado;
                        pedidoNegocio.agregarDetalle(item);
                        pedidoNegocio.restarStock(item.IdProducto, item.Cantidad);
                    }
                }

                Session["Carrito"] = new List<DetallePedido>();
                Session["IdPedido"] = null;

                ActualizarInterfazCarrito();
                CargarCardsProductos();

                string script = "alert('¡Pedido enviado a cocina con exito!'); window.location='Mesas.aspx';";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Redirect", script, true);
                Response.Redirect("Mesas.aspx", false);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }

        private void CargarCardsProductos()
        {
            try
            {
                ProductoNegocio negocio = new ProductoNegocio();
                repProductos.DataSource = negocio.listar(1);
                repProductos.DataBind();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }

        protected void repProductos_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "AgregarProducto")
            {
                int idProducto = Convert.ToInt32(e.CommandArgument);

                ProductoNegocio prodNegocio = new ProductoNegocio();
                List<Productos> listaProductos = prodNegocio.listar(1);
                Productos seleccionado = null;

                foreach (Productos p in listaProductos)
                {
                    if (p.IdProducto == idProducto)
                    {
                        seleccionado = p;
                        break;
                    }
                }

                if (seleccionado != null)
                {
                    if (seleccionado.Stock <= 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('¡Sin Stock disponible!');", true);
                        return;
                    }

                    List<DetallePedido> carrito = (List<DetallePedido>)Session["Carrito"];
                    DetallePedido itemExistente = null;

                    foreach (DetallePedido d in carrito)
                    {
                        if (d.IdProducto == idProducto)
                        {
                            itemExistente = d;
                            break;
                        }
                    }

                    if (itemExistente != null)
                    {
                        if (itemExistente.Cantidad >= seleccionado.Stock)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Límite de stock alcanzado para este producto.');", true);
                            return;
                        }
                        itemExistente.Cantidad++;

                    }
                    else
                    {
                        DetallePedido nuevoItem = new DetallePedido();
                        nuevoItem.IdProducto = seleccionado.IdProducto;
                        nuevoItem.NombreProducto = seleccionado.NombreProducto;
                        nuevoItem.PrecioUnitario = seleccionado.Precio;
                        nuevoItem.Cantidad = 1;

                        carrito.Add(nuevoItem);
                    }

                    Session["Carrito"] = carrito;
                    ActualizarInterfazCarrito();
                }
            }
        }

        protected void dgvPedidoActual_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            List<DetallePedido> carrito = (List<DetallePedido>)Session["Carrito"];

            carrito.RemoveAt(e.RowIndex);

            Session["Carrito"] = carrito;
            ActualizarInterfazCarrito();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "var myModal = new bootstrap.Modal(document.getElementById('modalRevision')); myModal.show();", true);
        }

        private void ActualizarInterfazCarrito()
        {
            List<DetallePedido> carrito = (List<DetallePedido>)Session["Carrito"];

            int totalUnidades = 0;
            decimal montoTotal = 0;

            foreach (DetallePedido item in carrito)
            {
                totalUnidades += item.Cantidad;
                montoTotal += item.Subtotal;
            }

            lblCantidadItems.Text = totalUnidades.ToString();
            lblTotalPedido.Text = string.Format("{0:C}", montoTotal);

            dgvPedidoActual.DataSource = carrito;
            dgvPedidoActual.DataBind();
        }
    }
}