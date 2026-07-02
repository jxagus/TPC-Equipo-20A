using Dominio;
using Negocio; 
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Resto_Bar_Web
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarMetricasDashboard();
                cargarDDL();
                cargarReporteMetodosdePago(0);
            }
        }

        private void CargarMetricasDashboard()
        {
            try
            {
                DashboardNegocio negocio = new DashboardNegocio();

                decimal cajaDia = negocio.ObtenerCajaDelDia();
                decimal cajaMes = negocio.ObtenerCajaDelMes();
                decimal cajaAnio = negocio.ObtenerCajaAnual();
                int pedidosHoy = negocio.ObtenerCantidadPedidosHoy();

                lblCajaDia.Text = string.Format("{0:C}", cajaDia);
                lblCajaMes.Text = string.Format("{0:C}", cajaMes);
                lblCajaAnio.Text = string.Format("{0:C}", cajaAnio);
                lblPedidosHoy.Text = pedidosHoy.ToString();

                lblPendientes.Text = negocio.ObtenerPedidosPendientesCocina().ToString();

                int mesaTop = negocio.ObtenerMesaMayorRotacion();
                lblMesaTop.Text = mesaTop > 0 ? mesaTop.ToString() : "-";

                List<Productos> listaPlatos = negocio.ObtenerTopPlatos();

                if (listaPlatos != null && listaPlatos.Count > 0)
                {
                    repPlatosTop.DataSource = listaPlatos;
                    repPlatosTop.DataBind();
                }
                else
                {
                    lblSinPlatos.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }

        protected void ddlPeriodoReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            int periodo = Convert.ToInt32(ddlPeriodoReporte.SelectedValue);
            cargarReporteMetodosdePago(periodo);
        }

        protected void cargarDDL()
        {

            ddlPeriodoReporte.Items.Insert(0, new ListItem("Completo", "0"));
            ddlPeriodoReporte.Items.Insert(1, new ListItem("Hoy", "1"));
            ddlPeriodoReporte.Items.Insert(2, new ListItem("Ayer", "2"));
            ddlPeriodoReporte.Items.Insert(3, new ListItem("Ultima Semana", "3"));
            ddlPeriodoReporte.Items.Insert(4, new ListItem("Ultimo Mes", "4"));
            ddlPeriodoReporte.Items.Insert(4, new ListItem("Ultimo Trimestre", "5"));
        }

        protected void cargarReporteMetodosdePago(int periodo)
        {
            DashboardNegocio negocio = new DashboardNegocio();

            rpReportesMetodoPago.DataSource = negocio.ObtenerReporteMetodosDePago(periodo);
            rpReportesMetodoPago.DataBind();
            if(negocio.ObtenerReporteMetodosDePago(periodo).Count == 0)
            {
                lblVacio.Visible = true;
            }
            else { lblVacio.Visible=false; }
        }
    }
}