using System;
using System.Collections.Generic;
using System.Web.UI;
using Dominio;
using Negocio; 
namespace Resto_Bar_Web
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarMetricasDashboard();
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
                System.Diagnostics.Debug.WriteLine("Error en Dashboard: " + ex.Message);
            }
        }
    }
}