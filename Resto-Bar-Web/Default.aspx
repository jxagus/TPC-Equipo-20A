<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Resto_Bar_Web._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Content/dashboard.css" rel="stylesheet" type="text/css" />
    <div class="row g-3 mb-4">
        <div class="col-md-3">
            <div class="card bg-gradient-diario text-white shadow-sm card-dashboard-hover">
                <div class="card-body">
                    <h6 class="text-uppercase fw-bold text-white small mb-2">💵 Caja del Día</h6>
                    <h2 class="fw-bold mb-0 text-monto-exito">
                        <asp:Label ID="lblCajaDia" runat="server" Text="$0.00"></asp:Label></h2>
                </div>
            </div>
        </div>

        <div class="col-md-3">
            <div class="card bg-gradient-mensual text-white shadow-sm card-dashboard-hover">
                <div class="card-body">
                    <h6 class="text-uppercase fw-bold text-white small mb-2">📈 Mes Actual</h6>
                    <h2 class="fw-bold mb-0 text-monto-exito">
                        <asp:Label ID="lblCajaMes" runat="server" Text="$0.00"></asp:Label></h2>
                </div>
            </div>
        </div>

        <div class="col-md-3">
            <div class="card bg-gradient-anual text-white shadow-sm card-dashboard-hover">
                <div class="card-body">
                    <h6 class="text-uppercase fw-bold text-white small mb-2">📊 Acumulado Anual</h6>
                    <h2 class="fw-bold mb-0 text-monto-exito">
                        <asp:Label ID="lblCajaAnio" runat="server" Text="$0.00"></asp:Label></h2>
                </div>
            </div>
        </div>

        <div class="col-md-3">
            <div class="card bg-gradient-pedidos text-white shadow-sm card-dashboard-hover">
                <div class="card-body">
                    <h6 class="text-uppercase fw-bold text-white small mb-2">🔥 Pedidos Hoy</h6>
                    <h2 class="fw-bold mb-0 text-warning">
                        <asp:Label ID="lblPedidosHoy" runat="server" Text="0"></asp:Label></h2>
                </div>
            </div>
        </div>
    </div>

    <div class="row g-3">
        <div class="col-md-6">
            <div class="card shadow-sm h-100">
                <div class="card-header bg-dark text-white fw-bold">🔥 Platos Más Solicitados</div>
                <div class="card-body">
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="card shadow-sm h-100">
                <div class="card-header bg-dark text-white fw-bold">📊 Estado del Salón</div>
                <div class="card-body">
                    <p>
                        ⌛ Comandas esperando en cocina: <strong>
                            <asp:Label ID="lblPendientes" runat="server" Text="0"></asp:Label></strong>
                    </p>
                    <p>
                        📍 Mesa con mayor rotación: <strong>Mesa Nro
                        <asp:Label ID="lblMesaTop" runat="server" Text="-"></asp:Label></strong>
                    </p>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
