<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Insumos.aspx.cs" Inherits="Resto_Bar_Web.Insumos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h1 class="mb-4">Carga de Producto</h1>

    <div class="mb-3">
        <label for="exampleFormControlInput1" class="form-label">Nombre del producto</label>
        <asp:TextBox ID="txtNombreProducto" CssClass="form-control" runat="server"></asp:TextBox>
    </div>

    <div class="mb-3">
        <label for="exampleFormControlInput1" class="form-label">Descripcion del Producto</label>
        <div class="input-group">
        <asp:TextBox ID="txtDescripcion" CssClass="form-control" TextMode="MultiLine" Rows="3" runat="server"></asp:TextBox>        </div>
    </div>

    <div class="mb-3">
        <label for="exampleFormControlInput1" class="form-label">Precio</label>
        <div class="input-group">
            <span class="input-group-text">$</span>
            <asp:TextBox ID="txtPrecio" CssClass="form-control" runat="server"></asp:TextBox>
            <span class="input-group-text">.00</span>
        </div>
    </div>

    <div class="mb-3">
        <label for="exampleFormControlInput1" class="form-label">Stock</label>
        <div class="input-group">
            <asp:TextBox ID="txtStock" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
    </div>

    <div class="mb-3">
    <asp:Button ID="btnAgregarProducto" CssClass="btn btn-secondary" Onclick="btnAgregarProducto_Click" runat="server" Text="Agregar Producto" />
    </div>
</asp:Content>
