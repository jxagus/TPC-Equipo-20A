using System;
using System.Collections.Generic;
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
            if (Session["idUsuario"] == null)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: no esta iniciado sesion');", true);
                return;
            }

            if (!IsPostBack)
            {
                if (Session["Carrito"] == null)
                {
                    Session["Carrito"] = new List<DetallePedido>();
                }

                CargarComboMesas();
                CargarCardsProductos();
                ActualizarInterfazCarrito(); 
            }
        }
        protected void btnConfirmarPedido_Click(object sender, EventArgs e)
        {
            //agarra el carrito desde la sesion
            List<DetallePedido> carrito = (List<DetallePedido>)Session["Carrito"];

            //Validacion basica por las dudas
            if (carrito == null || carrito.Count == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('El pedido esta vacio.');", true);
                return;
            }

            try
            {
                //Calculamos el total recorriendo el carrito
                decimal precioTotal = 0;
                foreach (DetallePedido item in carrito)
                {
                    precioTotal = precioTotal + item.Subtotal;
                }

                //Creamos el objeto Pedido para la base de datos
                Pedido nuevoPedido = new Pedido();
                nuevoPedido.NroMesa = Convert.ToInt32(ddlMesas.SelectedValue);
                nuevoPedido.IdUsuario = Convert.ToInt32(Session["idUsuario"]);
                nuevoPedido.FechayHoraPedido = DateTime.Now; //fechita actual
                nuevoPedido.PrecioTotal = precioTotal;
                nuevoPedido.IdEstadoPedido = 1; //1 Pendiente / 2 finalizado
                //Guardamos el pedido principal
                PedidoNegocio pedidoNegocio = new PedidoNegocio();
                int idPedidoGenerado = pedidoNegocio.agregarPedido(nuevoPedido);

                foreach (DetallePedido item in carrito)
                {
                    item.IdPedido = idPedidoGenerado; 

                    pedidoNegocio.agregarDetalle(item);
                    pedidoNegocio.restarStock(item.IdProducto, item.Cantidad);
                }
                //Vaciamos el carrito y actualizamos la interfaz
                Session["Carrito"] = new List<DetallePedido>();

                ActualizarInterfazCarrito();
                CargarCardsProductos(); //Volvemos a listar las cards para que reflejen el nuevo stock restado

                string script = "alert('¡Pedido enviado a cocina con exito!'); window.location='CargarPedido.aspx';";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Redirect", script, true);
            }
            catch (Exception ex)
            {
                //Response.Write("<h1 style='color:red; background:white; position:fixed; top:0; left:0; z-index:99999;'>ERROR REAL: " + ex.Message + "</h1>");
                //Response.End(); 
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }
        private void CargarComboMesas()
        {
            try
            {
                PedidoNegocio pedidoNegocio = new PedidoNegocio();
                List<string> listaMesas = pedidoNegocio.listarNumeroMesas();

                ddlMesas.Items.Clear();

                foreach (string nroMesa in listaMesas)
                {
                    ddlMesas.Items.Add(new ListItem("Mesa Nro " + nroMesa, nroMesa));
                }
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
                repProductos.DataSource = negocio.listar();
                repProductos.DataBind();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }
        //Se ejecuta al apretar "Añadir" en cualquier Card de producto
        protected void repProductos_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "AgregarProducto")
            {
                int idProducto = Convert.ToInt32(e.CommandArgument);

                ProductoNegocio prodNegocio = new ProductoNegocio();
                List<Productos> listaProductos = prodNegocio.listar();
                Productos seleccionado = null;

                foreach (Productos p in listaProductos)
                {
                    if (p.IdProducto == idProducto)
                    {
                        seleccionado = p;
                        break;
                    }
                }

                //Si encontramos el producto y hay stock, operamos con el carrito
                if (seleccionado != null)
                {
                    if (seleccionado.Stock <= 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('¡Sin Stock disponible!');", true);
                        return;
                    }

                    List<DetallePedido> carrito = (List<DetallePedido>)Session["Carrito"];
                    DetallePedido itemExistente = null;

                    //Buscamos si el artículo ya estaba en el carrito
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
                        //Si ya existia, validamos que no supere el stock disponible y aumentamos la cantidad
                        if (itemExistente.Cantidad >= seleccionado.Stock)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Límite de stock alcanzado para este producto.');", true);
                            return;
                        }
                        itemExistente.Cantidad++;
                    }
                    else
                    {
                        //Si es la primera vez que se agrega, creamos un nuevo renglon de detalle
                        DetallePedido nuevoItem = new DetallePedido();
                        nuevoItem.IdProducto = seleccionado.IdProducto;
                        nuevoItem.NombreProducto = seleccionado.NombreProducto;
                        nuevoItem.PrecioUnitario = seleccionado.Precio;
                        nuevoItem.Cantidad = 1;

                        carrito.Add(nuevoItem);
                    }

                    //Guardamos el estado actualizado en la sesion y refrescamos los componentes visuales
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

            //Script de Bootstrap para volver a abrir el modal automaticamente tras el PostBack
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "var myModal = new bootstrap.Modal(document.getElementById('modalRevision')); myModal.show();", true);
        }
        private void ActualizarInterfazCarrito()
        {
            List<DetallePedido> carrito = (List<DetallePedido>)Session["Carrito"];

            int totalUnidades = 0;
            decimal montoTotal = 0;

            //Recorremos el carrito acumulando cantidades y subtotales
            foreach (DetallePedido item in carrito)
            {
                totalUnidades += item.Cantidad;
                montoTotal += item.Subtotal;
            }

            //Asignamos a las etiquetas visuales
            lblCantidadItems.Text = totalUnidades.ToString();
            lblTotalPedido.Text = string.Format("{0:C}", montoTotal);

            dgvPedidoActual.DataSource = carrito;
            dgvPedidoActual.DataBind();
        }

    }
}