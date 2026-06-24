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
                VerificarEstiloNavbarPorRol();
            }

            // Control del texto del Login según el Rol en sesión
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
            if (Session["idRol"] != null)
            {
                int rol = (int)Session["idRol"];

                //Si el rol es 2 (Mesero / Mozo)
                if (rol == 2)
                {
                    sidebarMenu.Attributes["class"] += " bg-gradient-mozo";
                }
            }
        }
    }
}