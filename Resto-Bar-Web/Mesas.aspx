<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Mesas.aspx.cs" Inherits="Resto_Bar_Web.Mesas" %>
<asp:Content ID="ContentStyles" ContentPlaceHolderID="Stylesheets" runat="server">
    <link href="~/Content/Mesas.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Mesas Asignadas</h1>

    <div id="listado_mesas">
        <%
            if (ListaMesas != null && ListaMesas.Count > 0)
            {
                
                foreach (Dominio.Mesa mesa in ListaMesas)
                {
            %>
                <div class="tarjeta-mesa">
                    <img class="tarjeta mesa" src="https://w7.pngwing.com/pngs/678/430/png-transparent-table-furniture-chair-dining-room-matbord-bed-top-view-round-white-5-pc-wooden-dining-set-angle-coffee-tables-couch-thumbnail.png" alt="Mesa generica" />
                </div>
                <div class="tarjeta-cuerpo">
                    <h5 class="tarjeta-titulo"> Mesa Nro. <%= mesa.IdMesa %> Mesero <%= mesa.IdUsuario %></h5>
                    <button type="button" class="tarjeta-boton">
                        Administrar Mesa
                    </button>
                </div> <% 
                }
            } %>
    </div>

</asp:Content>