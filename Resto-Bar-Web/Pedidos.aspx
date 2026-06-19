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
                                <div class="card border-start border-warning border-4 shadow-sm">
                                    <div class="card-body d-flex justify-content-between align-items-center flex-wrap">
                                        
                                        <div>
                                            <h5 class="fw-bold mb-1 text-dark">
                                                🛎️ Pedido Nro #<%# Eval("IdPedido") %> — 
                                                <span class="text-primary">Mesa <%# Eval("NroMesa") %></span>
                                            </h5>
                                            <p class="mb-0 text-muted small">
                                                🕒 Hora: <strong><%# Eval("FechayHoraPedido", "{0:HH:mm}") %> hs</strong> | 
                                                📅 Fecha: <%# Eval("FechayHoraPedido", "{0:dd/MM/yyyy}") %>
                                            </p>
                                        </div>

                                        <div class="px-3">
                                            <span class="text-muted d-block small">Monto Total</span>
                                            <h4 class="fw-bold text-success mb-0"><%# string.Format("{0:C}", Eval("PrecioTotal")) %></h4>
                                        </div>

                                        <div>
                                            <asp:Button ID="btnFinalizar" runat="server" Text="✔ Marcar Entregado" 
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
</asp:Content>