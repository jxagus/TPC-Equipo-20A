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

                MesasNegocio negocio = new MesasNegocio();
                chkEstadoMesa.Checked = negocio.obtenerEstadoPorId(int.Parse(idMesa));


                string script = "var miModal = new bootstrap.Modal(document.getElementById('modalAdministrarMesa')); miModal.show();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", script, true);
            }
        }

        protected void btnVerPedido_Click(object sender, EventArgs e)
        {

        }

        protected void btnGuardarEstado_Click(object sender, EventArgs e)
        {
            if (Session["MesaSeleccionada"] != null)
            {
                MesasNegocio negocio = new MesasNegocio();

                int idMesa = Convert.ToInt32(Session["MesaSeleccionada"]);
                int nuevoEstado = chkEstadoMesa.Checked ? 1 : 0;
                negocio.inhabilitarHabilitarMesa(nuevoEstado, idMesa);
                Response.Redirect("Mesas.aspx");
            }
        }
    }
}