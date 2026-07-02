using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Resto_Bar_Web
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sidebarMenu.Visible = false;
            bodyDelaPagina.Style["margin-left"] = "0px";
            if (!IsPostBack)
            {
                if (Session["idUsuario"] == null)
                {
                    //Response.Redirect("~/Login.aspx");
                }

            }
            VerificarEstiloNavbarPorRol();

            if (Session["idRol"] != null)
            {
                int rol = (int)Session["idRol"];
                bodyDelaPagina.Style["margin-left"] = "250px";

                sidebarMenu.Visible = true;
                if (rol == 0)
                {
                    TipoUsuario.InnerText = "👤 Admin";
                    phMenuGestion.Visible = true;
                    phCrearPersonal.Visible = true;
                }


                else if (rol == 1)
                {
                    TipoUsuario.InnerText = "👤 Gerente";
                    phMenuGestion.Visible = true;
                    phCrearPersonal.Visible = false;
                }

                else if (rol == 2)
                {
                    TipoUsuario.InnerText = "👤 Mesero";
                    phMenuGestion.Visible = false;
                    phCrearPersonal.Visible = false;

                }

            }
        }

        private void VerificarEstiloNavbarPorRol()
        {
            sidebarMenu.Attributes["class"] = "sidebar";

            var rolSesion = Session["idRol"];

            if (rolSesion == null)
            {
                sidebarMenu.Attributes["class"] += " bg-color-invitado";
                return;
            }

            int rol = (int)rolSesion;

            switch (rol)
            {
                case 0: // Admin
                    sidebarMenu.Attributes["class"] += " bg-gradient-admin";
                    break;
                case 1: // Gerente
                    sidebarMenu.Attributes["class"] += " bg-degrade-pasos";
                    break;
                case 2: // Mozo
                    sidebarMenu.Attributes["class"] += " bg-gradient-mozo";
                    break;
                default: // Cualquier otro caso
                    sidebarMenu.Attributes["class"] += " bg-color-invitado";
                    break;
            }
        }

    }
}