<%@ Page Title="Pedidos Activos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pedidos.aspx.cs" Inherits="Resto_Bar_Web.Pedidos" %>

<asp:Content ID="ContentStyles" ContentPlaceHolderID="Stylesheets" runat="server">
    <link href="/Content/factura.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-3">

        <div class="d-flex justify-content-between align-items-center mb-4">
            <h2 class="mb-0">📋 Cola de Pedidos Activos</h2>
            <a href="Mesas.aspx" class="btn btn-primary fw-bold px-4 shadow-sm">➕ Nueva Orden</a>
        </div>

        <div class="card mb-4 shadow-sm border-0">
            <div class="card-body bg-light rounded shadow-sm">
                <div class="row align-items-end g-3">

                    <div class="col-12 col-sm-4 col-md-3">
                        <label class="form-label fw-bold text-secondary mb-1">🔍 Filtrar por Mesa:</label>
                        <asp:TextBox ID="txtFiltroMesa" runat="server" CssClass="form-control" placeholder="Ej: 4"></asp:TextBox>

                        <asp:Label ID="lblErrorFiltro" runat="server" CssClass="text-danger small d-block mt-1 fw-bold" Visible="false"></asp:Label>
                    </div>

                    <div class="col-12 col-sm-8 col-md-6 d-flex gap-2">
                        <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-primary px-4 fw-bold shadow-sm" OnClick="btnFiltrar_Click" />
                        <asp:Button ID="btnLimpiarFiltro" runat="server" Text="No Filtrar / Ver Todos" CssClass="btn btn-outline-secondary px-3" OnClick="btnLimpiarFiltro_Click" />
                    </div>

                </div>
            </div>
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

    <div class="modal fade" id="modalExitoPedido" tabindex="-1" aria-hidden="true" data-bs-backdrop="static">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content p-3 ticket-comprobante">
                <div class="modal-header border-0 pb-0">
                    <h5 class="modal-title ticket-titulo">*** COMPROBANTE DE PAGO ***</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row fw-bold">
                            <div class="col-md-6">
                                Mesa:
                                <asp:Label ID="lblFacturaMesa" runat="server" />
                            </div>
                            <div class="col-md-6 text-end">
                                Pedido: #<asp:Label ID="lblFacturaIdPedido" runat="server" />
                            </div>
                        </div>

                        <div class="ticket-separador"></div>

                        <div class="row ticket-cabecera-items">
                            <div class="col-6">Detalle</div>
                            <div class="col-2 text-center">Cant</div>
                            <div class="col-4 text-end">Total</div>
                        </div>

                        <asp:Repeater ID="repFacturaItems" runat="server">
                            <ItemTemplate>
                                <div class="row">
                                    <div class="col-6"><%# Eval("NombreProducto") %></div>
                                    <div class="col-2 text-center"><%# Eval("Cantidad") %></div>
                                    <div class="col-4 text-end">$<%# string.Format("{0:N2}", Eval("Subtotal")) %></div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>

                        <div class="ticket-separador"></div>

                        <div class="row ticket-total">
                            <div class="d-flex align-items-center justify-content-between my-3 text-success fw-bold">
                                <span class="text-nowrap me-2">Metodo de Pago:</span>
                                <asp:DropDownList ID="ddlMetodosDePago" CssClass="form-select form-select-sm w-51 fs-6 py-0 px-2 font-monospace" Style="background-image: none; padding-right: 0.5rem !important;" runat="server"></asp:DropDownList>
                            </div>
                            <div class="col-6 text-end ms-auto">TOTAL:</div>
                            <div class="col-6 text-end">
                                $<asp:Label ID="lblFacturaTotal" runat="server" />
                            </div>
                        </div>

                        <div class="row mt-3 text-center text-muted small">
                            <div class="col-12 mb-3">¡Gracias por su visita! </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer border-0 d-grid gap-2">
                    <asp:Button ID="btnFinalizarPedido" CssClass="btn btn-dark w-100" runat="server" OnClick="btnFinalizarPedido_Click" Text="Aceptar y Finalizar" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
