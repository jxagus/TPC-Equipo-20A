using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace Resto_Bar_Web
{
    public partial class Mesas : System.Web.UI.Page
    {
        public List<Mesa> ListaMesas {  get; set; }
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
            if(e.CommandName == "AbrirModal")
            {
                
                string idMesa = e.CommandArgument.ToString();
                Session["MesaSeleccionada"] = idMesa;
                tituloModalMesa.InnerText = "Mesa Numero " + idMesa;

                string script = "var miModal = new bootstrap.Modal(document.getElementById('modalAdministrarMesa')); miModal.show();";
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
                        lblCerradosHoy.Text = datos.Lector["PedidosCerradosHoy"].ToString();
                        lblPedidoActual.Text = datos.Lector["PedidoActual"].ToString();
                    }
                    else
                    {
                        lblCerradosHoy.Text = "0";
                        lblPedidoActual.Text = "No";
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
                string script = "var miModalPedido = new bootstrap.Modal(document.getElementById('modalPedido')); miModalPedido.show();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PopPedido", script, true);
            }
        }

        protected void btnVerPedido_Click(object sender, EventArgs e)
        { 
            //falta logica para ver carrito al pedido actual con el nro de mesa seleccionado
        }

        protected void btnAgregarPedido_Click(object sender, EventArgs e)
        {
            Response.Redirect("cargarpedido.aspx", false); //falta logica para setear mesa en el pedido y poder avanzar de paso
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
    }
}