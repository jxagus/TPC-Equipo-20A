<%@ Page Title="Pedidos Activos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pedidos.aspx.cs" Inherits="Resto_Bar_Web.Pedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-3">

        <div class="d-flex justify-content-between align-items-center mb-4">
            <h2 class="mb-0">📋 Cola de Pedidos Activos</h2>
            <a href="CargarPedido.aspx" class="btn btn-primary fw-bold px-4 shadow-sm">➕ Nueva Orden</a>
        </div>

        <div class="card shadow-sm">
            <div class="card-header bg-degrade-pasos fw-bold">
                Pedidos en Espera (Prioridad de arriba hacia abajo)
            </div>
            <div class="card-body bg-light">

                <asp:Panel ID="pnlVacio" runat="server" CssClass="text-center py-5 d-none">
                    <h4 class="text-muted">No hay pedidos pendientes en este momento. ✨</h4>
                </asp:Panel>

                <div class="row g-3">
                    <asp:Repeater ID="repPedidos" runat="server" OnItemCommand="repPedidos_ItemCommand">
                        <ItemTemplate>
                            <div class="col-12">
                                <div class="card border-start border-warning border-4 shadow-sm mb-2">
                                    <div class="card-body d-flex justify-content-between align-items-center flex-wrap">

                                        <div>
                                            <h5 class="fw-bold mb-1 text-dark">🍽️ Pedido Nro #<%# Eval("IdPedido") %> — 
                                                <span class="text-primary">Mesa <%# Eval("NroMesa") %></span>
                                            </h5>
                                            <p class="mb-0 text-muted small">
                                                🕒 Hora: <strong><%# Eval("FechayHoraPedido", "{0:HH:mm}") %> hs</strong>
                                            </p>
                                        </div>

                                        <div class="px-3" runat="server" visible='<%# DeterminarVisibilidadMonto() %>'>
                                            <span class="text-muted d-block small">Monto Total</span>
                                            <h4 class="fw-bold text-success mb-0"><%# string.Format("{0:C}", Eval("PrecioTotal")) %></h4>
                                        </div>

                                        <div class="d-flex gap-2">
                                            <asp:LinkButton ID="btnVerDetalle" runat="server"
                                                CssClass="btn btn-outline-secondary fw-bold"
                                                CommandName="VerDetalle"
                                                CommandArgument='<%# Eval("IdPedido") %>'>
                                                🔍 Ver Detalle
                                            </asp:LinkButton>

                                            <asp:Button ID="btnFinalizar" runat="server"
                                                Text="✔ Marcar Entregado"
                                                CssClass="btn btn-outline-success fw-bold"
                                                CommandName="FinalizarPedido"
                                                CommandArgument='<%# Eval("IdPedido") %>' />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

            </div>
        </div>
    </div>
    <div class="modal fade" id="modalDetallePedido" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header bg-dark text-white">
                    <h5 class="modal-title fw-bold" id="exampleModalLabel">📋 Artículos del Pedido #<asp:Label ID="lblModalIdPedido" runat="server" Text=""></asp:Label>
                    </h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">

                    <asp:GridView ID="dgvDetallePedido" runat="server" AutoGenerateColumns="False"
                        CssClass="table table-hover table-striped border-0">
                        <Columns>
                            <asp:BoundField DataField="NombreProducto" HeaderText="Alimento / Bebida" HeaderStyle-CssClass="table-dark text-white" />
                            <asp:BoundField DataField="Cantidad" HeaderText="Cant." HeaderStyle-CssClass="table-dark text-white text-center" ItemStyle-CssClass="text-center fw-bold" />
                            <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio Unit." DataFormatString="{0:C}" HeaderStyle-CssClass="table-dark text-white" />
                            <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:C}" HeaderStyle-CssClass="table-dark text-white fw-bold text-success" />
                        </Columns>
                    </asp:GridView>

                </div>
                <div class="modal-footer bg-light">
                    <button type="button" class="btn btn-secondary fw-bold" data-bs-dismiss="modal">Cerrar Ventana</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
