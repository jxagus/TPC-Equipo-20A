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
            
    
        <asp:Repeater id="repeaterMesas" runat="server" OnItemCommand="repeaterMesas_ItemCommand">
            <ItemTemplate>
                <div class="col">   
                    <div class="card shadow <%#  VerificarEstadoMesa(Eval("EstadoMesa")) %>" style="width: 16rem;">
                      <img src="https://www.shutterstock.com/image-vector/business-meeting-icon-three-people-600nw-2765189057.jpg" class="card-img-top rounded-circle w-75 mx-auto mt-3 shadow-lg user-select-none" alt="img generica">
                      <div class="card-body">
                        <h5 class="card-title text-center user-select-none">Mesa Numero: <%# Eval("IdMesa") %></h5>
                        <h5 class="card-title text-center user-select-none">Mesero asignado: <%# Eval("Idusuario") %></h5>
                          <div class="d-grid gap-3 col-auto ">
                         <asp:Button ID="btnAdministrarMesa" CssClass="btn btn-primary" runat="server" Text="Administrar mesa"  CommandName="AbrirModal" CommandArgument='<%# Eval("IdMesa") %>' CausesValidation="false"/>
                          </div>
                      </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>


                

                
                
         <% } %>
    </div>



 <div class="modal fade" id="modalAdministrarMesa" tabindex="-1" aria-hidden="true">
  <div class="modal-dialog modal-dialog-centered">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="tituloModalMesa" runat="server">titulo aca</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body text-center">
        
        <div class="form-check form-switch d-flex align-items-center justify-content-between w-100">
            <label class="form-check-label fs-5 fw-semibold" id="lblEstadoMesa">Habilitar Mesa </label>
            <input class="form-check-input m-5" type="checkbox" runat="server" role="switch" id="chkEstadoMesa" onchange="mostrarAdvertenciaMesa(this)" checked style="cursor: pointer;">
        </div>

      </div>

        <div id="alertaDeshabilitarMesa" class="mt-3 w-100 user-select-none" style="  display: none">
            <div class="alert alert-danger d-flex align-items-center justify-content-center text-center shadow-sm m-0 alertaFade" role="alert">
                <span class="fs-4 me-2">⚠️</span>
                <div>
                La mesa se inhabilitará...
              </div>
            </div>
        </div>

        <div class="d-flex flex-column align-items-center my-3 p-3 bg-light rounded-3">
    <div class="modal-footer">
        <asp:Button ID="btnVerPedido" runat="server" Text="Ver Pedido" CssClass="btn btn-success btnZoom" OnClick="btnVerPedido_Click" />

        <asp:Button ID="btnGuardarEstado" runat="server" Text="Guardar Cambios" CssClass="btn btn-dark btnZoom" OnClick="btnGuardarEstado_Click" />
    </div>


</div>

    </div>
  </div>
</div>



    <script>
        function mostrarAdvertenciaMesa(checkbox) {
            var label = document.getElementById('lblTextoAdvertencia');
            var alerta = document.getElementById('alertaDeshabilitarMesa');

            if (checkbox.checked) {
                alerta.style.display = "none";
            } else {
                alerta.style.display = "block";
            }
        }

    </script>
</asp:Content>