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
            if (!IsPostBack)
            {
                MesasNegocio negocio = new MesasNegocio();
                ListaMesas = negocio.listarTodas();
            }

        }
    }
}