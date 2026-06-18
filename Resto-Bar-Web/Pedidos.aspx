<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pedidos.aspx.cs" Inherits="Resto_Bar_Web.Pedidos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-3">
    <div class="d-flex justify-content-between align-items-center mb-4">
        <h2 class="mb-0">📋 Cola de Pedidos Activos</h2>
        
        <a href="CargarPedido.aspx" class="btn btn-primary fw-bold px-4 shadow-sm">
            ➕ Nueva Orden / Cargar Pedido
        </a>
    </div>

    </div>
</asp:Content>
