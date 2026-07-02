using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Resto_Bar_Web
{
    public partial class AsignacionMesa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUsuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            else
            {
                LoginNegocio negocio = new LoginNegocio();
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);
                int rol = negocio.traerRol(idUsuario);

                if (rol != 0 && rol != 1)
                {
                    string script = "alert('Permisos insuficientes'); window.location.href = 'Dashboard.aspx';";
                    ClientScript.RegisterStartupScript(this.GetType(), "alertPermisos", script, true);
                }
            }

            if (!IsPostBack)
            {
                CargarMozos();
                CargarMesas();
                CargarMesasPost();
                CargarAccion();
                ActualizarModuloMesas();
            }
        }

        private void CargarMozos()
        {
            try
            {
                UsuariosNegocio negocio = new UsuariosNegocio();
                List<Usuario> mozos = negocio.listarMozos();

                ddlMozo.Items.Clear();
                ddlMozo.Items.Add(new ListItem("-- Seleccioná un mozo --", "0"));

                foreach (Usuario mozo in mozos)
                {
                    ddlMozo.Items.Add(new ListItem(mozo.NombreUsuario, mozo.IdUsuario.ToString()));
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false); throw ex;
            }
        }

        private void CargarMesas()
        {
            try
            {
                MesasNegocio negocio = new MesasNegocio();

                ddlMesa.Items.Clear();
                ddlMesa.Items.Add(new ListItem("-- Seleccioná una mesa --", "0"));


                List<Mesa> mesas = negocio.listarTodas();

                foreach (Mesa mesa in mesas)
                {
                    if (mesa.EstadoMesa == EstadoMesa.Habilitada && mesa.IdUsuario == 0)
                    {
                        ddlMesa.Items.Add(
                            new ListItem("Mesa " + mesa.IdMesa, mesa.IdMesa.ToString())
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false); throw ex;
            }
        }
        public void asignarMozoAMesa(int nroMesa, int idUsuarioMozo)
        {
            try
            {
                MesasNegocio negocio = new MesasNegocio();
                negocio.asignarMozo(nroMesa, idUsuarioMozo);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false); throw ex;
            }
        }

        protected void btnAsignar_Click(object sender, EventArgs e)
        {
            if (ddlMozo.SelectedValue == "0" || ddlMesa.SelectedValue == "0") return;

            try
            {
                int idUsuarioMozo = int.Parse(ddlMozo.SelectedValue);
                int nroMesa = int.Parse(ddlMesa.SelectedValue);

                MesasNegocio negocio = new MesasNegocio();
                negocio.asignarMozo(nroMesa, idUsuarioMozo);

                CargarMesas();
                ddlMozo.SelectedValue = "0";

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('¡Mesa asignada con éxito!');", true);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }
        protected void listarMozos()
        {
            UsuariosNegocio negocio = new UsuariosNegocio();
            try
            {
                negocio.listarMozos();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
            MesasNegocio negocio = new MesasNegocio();

            if (ddlAccion.SelectedValue == "0" && negocio.mesaTienePedido(int.Parse(ddlMesas.SelectedValue)))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('No se puede inhabilitar: la mesa tiene un pedido activo.');", true);
                return;
            }

            negocio.inhabilitarHabilitarMesa(int.Parse(ddlAccion.SelectedValue), int.Parse(ddlMesas.SelectedValue));
            CargarMesas();
            CargarMesasPost();

            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }
        private void CargarAccion()
        {
            ddlAccion.Items.Clear();
            ddlAccion.Items.Add(new ListItem("Habilitar", "1"));
            ddlAccion.Items.Add(new ListItem("Inhabilitar", "0"));
        }
        public void CargarMesasPost()
        {
            try
            {
                ddlMesas.Items.Clear();
                ddlMesas.Items.Add(new ListItem("-- Seleccioná una mesa --", "0"));
                MesasNegocio negocio = new MesasNegocio();
                List<Mesa> mesas = negocio.listarTodas();

                string seleccion = ddlAccion.SelectedValue;

                foreach (Mesa mesa in mesas)
                {
                    if (seleccion == "0" && mesa.EstadoMesa == EstadoMesa.Habilitada && !negocio.mesaTienePedido(mesa.IdMesa) && mesa.IdUsuario == 0)
                    {
                        ddlMesas.Items.Add(
                            new ListItem("Mesa " + mesa.IdMesa, mesa.IdMesa.ToString())
                        );
                    }
                    else if (seleccion == "1" && mesa.EstadoMesa == EstadoMesa.Inhabilitada)
                    {
                        ddlMesas.Items.Add(
                            new ListItem("Mesa " + mesa.IdMesa, mesa.IdMesa.ToString())
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }
        protected void ddlAccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarMesasPost();
        }
        private void ActualizarModuloMesas()
        {
            try
            {
                MesasNegocio negocio = new MesasNegocio();

                int proximoNumero = negocio.obtenerProximoNumeroMesa();
                litProximaMesa.Text = proximoNumero.ToString();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }
        protected void btnCrearMesa_Click(object sender, EventArgs e)
        {
            lblMensaje.Visible = false;

            try
            {
                MesasNegocio negocio = new MesasNegocio();


                negocio.agregarNuevaMesaAutomatica();

                lblMensaje.Text = "✔️ Mesa creada correctamente desde la base de datos.";
                lblMensaje.CssClass = "alert alert-success d-block mb-3";
                lblMensaje.Visible = true;

                ActualizarModuloMesas();
                CargarMesas();     
                CargarMesasPost();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }
        
    }
}