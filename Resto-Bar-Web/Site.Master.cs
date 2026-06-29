using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace Resto_Bar_Web
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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

                if (rol == 0)
                    TipoUsuario.InnerText = "👤 Admin";

                else if (rol == 1)
                    TipoUsuario.InnerText = "👤 Gerente";

                else if (rol == 2)
                    TipoUsuario.InnerText = "👤 Mesero";
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