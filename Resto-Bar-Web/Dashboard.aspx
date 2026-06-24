<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Resto_Bar_Web.Dashboard" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Content/dashboard.css" rel="stylesheet" type="text/css" />

    <div class="row g-3 mb-4">
        <div class="col-md-3">
            <div class="card bg-gradient-diario text-white shadow-sm card-dashboard-hover">
                <div class="card-body">
                    <h6 class="text-uppercase fw-bold text-white small mb-2">💵 Caja del Día</h6>
                    <h2 class="fw-bold mb-0 text-monto-exito">
                        <asp:Label ID="lblCajaDia" runat="server" Text="$0.00"></asp:Label>
                    </h2>
                </div>
            </div>
        </div>

        <div class="col-md-3">
            <div class="card bg-gradient-mensual text-white shadow-sm card-dashboard-hover">
                <div class="card-body">
                    <h6 class="text-uppercase fw-bold text-white small mb-2">📈 Mes Actual</h6>
                    <h2 class="fw-bold mb-0 text-monto-exito">
                        <asp:Label ID="lblCajaMes" runat="server" Text="$0.00"></asp:Label>
                    </h2>
                </div>
            </div>
        </div>

        <div class="col-md-3">
            <div class="card bg-gradient-anual text-white shadow-sm card-dashboard-hover">
                <div class="card-body">
                    <h6 class="text-uppercase fw-bold text-white small mb-2">📊 Acumulado Anual</h6>
                    <h2 class="fw-bold mb-0 text-monto-exito">
                        <asp:Label ID="lblCajaAnio" runat="server" Text="$0.00"></asp:Label>
                    </h2>
                </div>
            </div>
        </div>

        <div class="col-md-3">
            <div class="card bg-gradient-pedidos text-white shadow-sm card-dashboard-hover">
                <div class="card-body">
                    <h6 class="text-uppercase fw-bold text-white small mb-2">🔥 Pedidos Hoy</h6>
                    <h2 class="fw-bold mb-0 text-warning">
                        <asp:Label ID="lblPedidosHoy" runat="server" Text="0"></asp:Label>
                    </h2>
                </div>
            </div>
        </div>
    </div>

    <div class="row g-3">
        <div class="col-md-6">
            <div class="card shadow-sm h-100">
                <div class="card-header bg-dark text-white fw-bold">🔥 Platos Más Solicitados (Top 5)</div>
                <div class="card-body">
                    
                    <asp:Repeater ID="repPlatosTop" runat="server">
                        <ItemTemplate>
                            <div class="mb-3">
                                <div class="d-flex justify-content-between align-items-center mb-1">
                                    <span class="fw-semibold text-secondary"><%# Eval("NombreProducto") %></span>
                                    <span class="badge bg-primary rounded-pill"><%# Eval("CantidadVendida") %> porciones</span>
                                </div>
                                <div class="progress" style="height: 8px;">
                                    <div class="progress-bar bg-success" role="progressbar" 
                                         style='width: <%# Eval("Porcentaje") %>%;' 
                                         aria-valuenow='<%# Eval("Porcentaje") %>' aria-valuemin="0" aria-valuemax="100"></div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    
                    <asp:Label ID="lblSinPlatos" runat="server" Text="No hay datos de ventas disponibles hoy." 
                               CssClass="text-muted fst-italic d-block text-center my-3" Visible="false" />
                </div>
            </div>
        </div>

        <div class="col-md-6">
            <div class="card shadow-sm h-100">
                <div class="card-header bg-dark text-white fw-bold">📊 Estado del Salón</div>
                <div class="card-body d-flex flex-column justify-content-around">
                    
                    <div class="p-3 bg-light rounded mb-2 border-start border-warning border-4">
                        <p class="mb-0 text-muted small fw-bold text-uppercase">Cocina</p>
                        <h5 class="fw-bold mb-0 mt-1">
                            ⌛ Comandas esperando: <asp:Label ID="lblPendientes" runat="server" CssClass="text-danger" Text="0"></asp:Label>
                        </h5>
                    </div>
                    <div class="p-3 bg-light rounded border-start border-primary border-4">
                        <p class="mb-0 text-muted small fw-bold text-uppercase">Rendimiento</p>
                        <h5 class="fw-bold mb-0 mt-1">
                            📍 Mayor rotación: <span class="text-primary">Mesa Nro <asp:Label ID="lblMesaTop" runat="server" Text="-"></asp:Label></span>
                        </h5>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>