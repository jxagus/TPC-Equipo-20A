using System;
using System.Collections.Generic;
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
    }
}