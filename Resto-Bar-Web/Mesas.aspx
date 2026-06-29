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


        <asp:Repeater ID="repeaterMesas" runat="server" OnItemCommand="repeaterMesas_ItemCommand">
            <ItemTemplate>
                <div class="col">
                    <div class="card shadow <%# VerificarEstadoMesa(Eval("EstadoMesa")) %>" style="width: 16rem;">
                        <img src="https://www.shutterstock.com/image-vector/business-meeting-icon-three-people-600nw-2765189057.jpg" class="card-img-top rounded-circle w-75 mx-auto mt-3 shadow-lg user-select-none" alt="img generica">
                        <div class="card-body">
                            <h5 class="card-title text-center user-select-none">Mesa Numero: <%# Eval("IdMesa") %></h5>
                            <h5 class="card-title text-center user-select-none"><%# Convert.ToInt32(Eval("Idusuario")) !=0 ? "Mesero asignado: " + Eval("Idusuario") : "Mesero sin Asignar"%></h5>
                            <div class="d-grid gap-3 col-auto ">
                                <asp:Button ID="btnPedido" CssClass="btn btn-primary" runat="server" Text="Pedido" CommandName="AbrirPedido" CommandArgument='<%# Eval("IdMesa") %>' CausesValidation="false" />
                                <asp:Button ID="btnAdministrarMesa" CssClass="btn btn-primary" runat="server" Text="Finalizar Asignacion" CommandName="AbrirModal" CommandArgument='<%# Eval("IdMesa") %>' CausesValidation="false" />
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
                    <h5 class="modal-title" id="tituloModalMesa" runat="server">Mesa Número X</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>

                <div class="modal-body text-center">
                    <div id="divFinalizarAsignacion" runat="server" class="mt-3 p-2 bg-light rounded-3 text-start">
                        <p class="fs-6 fw-semibold text-danger mb-1">⚠️ Finalizar Asignación de Turno</p>
                        <p class="text-muted small m-0">Al confirmar, la mesa se liberará para el siguiente turno.</p>
                    </div>
                </div>

                <div class="d-flex flex-column align-items-center my-3 p-3 bg-light rounded-3">
                    <div class="modal-footer w-100 d-flex justify-content-center gap-2">
                        <asp:Button ID="btnGuardarEstado" runat="server" Text="Finalizar" CssClass="btn btn-dark btnZoom" OnClick="btnGuardarEstado_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modalPedido" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="tituloModalPedido" runat="server">Mesa X</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>

                <asp:UpdatePanel ID="upModalesPedido" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="modal-body">
                            <ul class="list-group">
                                <li class="list-group-item d-flex justify-content-between align-items-center">Pedidos cerrados hoy           
                                   
                                    <span class="badge bg-primary rounded-pill">
                                        <asp:Label ID="lblCerradosHoy" runat="server" Text="0" />
                                    </span>
                                </li>
                                <li class="list-group-item d-flex justify-content-between align-items-center">Mesa ocupada
                                   
                                    <span class="badge bg-primary rounded-pill">
                                        <asp:Label ID="lblPedidoActual" runat="server" Text="No" />
                                    </span>
                                </li>
                            </ul>
                        </div>
                        <div class="modal-footer w-100 d-flex justify-content-center gap-2">
                            <asp:Button ID="btnAgregarPedido" runat="server" Text="Agregar Pedido" CssClass="btn btn-dark btnZoom" OnClick="btnAgregarPedido_Click" />

                            <asp:Button ID="btnVerPedido" runat="server" Text="Ver Pedido" CssClass="btn btn-dark btnZoom"
                                OnClick="btnVerPedido_Click"
                                data-bs-toggle="modal"
                                data-bs-target="#modalPedidoActual" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>


    <div class="modal fade" id="modalPedidoActual" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content shadow-lg">
                <div class="modal-header">
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>

                <asp:UpdatePanel ID="UpmodalPedidoActual" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                       <h5 class="modal-title" id="H1" runat="server">Mesa X</h5>
                        <div class="modal-body">
                            <asp:GridView ID="dgvDetallePedido" runat="server" AutoGenerateColumns="False"
                                CssClass="table table-hover table-striped border-0">
                                <Columns>
                                    <asp:BoundField DataField="NombreProducto" HeaderText="Alimento / Bebida" HeaderStyle-CssClass="table-dark text-white" />
                                    <asp:BoundField DataField="Cantidad" HeaderText="Cant." HeaderStyle-CssClass="table-dark text-white text-center" ItemStyle-CssClass="text-center fw-bold" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="modal-footer w-100 d-flex justify-content-center gap-2">
                            <asp:Button ID="btnAgregarProducto" runat="server" Text="Agregar" CssClass="btn btn-dark btnZoom" Onclick="btnAgregarProducto_Click"/>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

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
