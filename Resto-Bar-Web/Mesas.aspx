<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Mesas.aspx.cs" Inherits="Resto_Bar_Web.Mesas" %>

<asp:Content ID="ContentStyles" ContentPlaceHolderID="Stylesheets" runat="server">
    <link href="/Content/Mesas.css" rel="stylesheet" />
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="mb-4">Mesas Asignadas</h1>


    <div class="d-flex flex-wrap gap-4 justify-content-center">
        <%
            if (ListaMesas != null && ListaMesas.Count > 0)
            {
                %>
            
            
            <%
                foreach (Dominio.Mesa mesa in ListaMesas)
                {
                    string cartaColor = "bg-success bg-opacity-50 text-white";
                    if(mesa.EstadoMesa == Dominio.EstadoMesa.Inhabilitada){
                        cartaColor = "bg-secondary bg-opacity-50 text-dark";
                    }
                    else {
                        bool tienePedidoActivo = false; /*TEMPORAL PARA PROBAR, DESPUES SE LE VA A AGREGAR FUNCIONALIDAD PARA VERIFICARLO REALMENTE*/
                        if(tienePedidoActivo){
                            cartaColor = "bg-warning bg-opacity-50 text-dark";
                        }
                    }

            %>
                


                <div class="col">   
                    <div class="card shadow <%= cartaColor %>" style="width: 16rem;">
                      <img src="https://www.shutterstock.com/image-vector/business-meeting-icon-three-people-600nw-2765189057.jpg" class="card-img-top rounded-circle w-75 mx-auto mt-3 shadow-lg user-select-none" alt="img generica">
                      <div class="card-body">
                        <h5 class="card-title text-center user-select-none">Mesa Numero: <%= mesa.IdMesa %></h5>
                        <h5 class="card-title text-center user-select-none">Mesero asignado: <%= mesa.IdUsuario %></h5>
                          <div class="d-grid gap-3 col-auto ">
                         <asp:Button ID="btnAdministrarMesa" OnClick="btnAdministrarMesa_Click" CssClass="btn btn-primary" runat="server" Text="Administrar mesa" />
                          </div>
                      </div>
                    </div>
                </div>

                
        
        
        <% 
                }%>

                 <%
            } %>
    </div>

</asp:Content>