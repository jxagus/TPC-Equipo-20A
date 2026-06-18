<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="Resto_Bar_Web.Insumos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid mt-2">
        <div class="row">

            <div class="col-md-4 pe-md-4 border-end">
                <h2 class="mb-4">Carga de Producto</h2>

                <div class="mb-3">
                    <label class="form-label fw-bold">Nombre del producto</label>
                    <asp:TextBox ID="txtNombreProducto" CssClass="form-control" runat="server"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <label class="form-label fw-bold">Descripción del Producto</label>
                    <asp:TextBox ID="txtDescripcion" CssClass="form-control" TextMode="MultiLine" Rows="3" runat="server"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <label class="form-label fw-bold">Precio</label>
                    <div class="input-group">
                        <span class="input-group-text">$</span>
                        <asp:TextBox ID="txtPrecio" CssClass="form-control" runat="server"></asp:TextBox>
                        <span class="input-group-text">.00</span>
                    </div>
                </div>

                <div class="mb-3">
                    <label class="form-label fw-bold">Stock</label>
                    <asp:TextBox ID="txtStock" CssClass="form-control" runat="server"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <asp:Button ID="btnAgregarProducto" CssClass="btn btn-primary w-100" OnClick="btnAgregarProducto_Click" runat="server" Text="Agregar Producto" />
                </div>
            </div>

            <div class="col-md-8 ps-md-4">
                <h2 class="mb-4">Productos Existentes</h2>

                <div class="table-responsive">
                    <asp:GridView ID="dgvProductos" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered align-middle">
                        <Columns>
                            <asp:BoundField DataField="IdProducto" HeaderText="ID" ItemStyle-Width="50px" />

                            <asp:BoundField DataField="NombreProducto" HeaderText="Producto" />
                            <asp:BoundField DataField="DescripcionProducto" HeaderText="Descripción" />
                            <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />

                            <asp:BoundField DataField="Stock" HeaderText="Stock" ItemStyle-HorizontalAlign="Center" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

        </div>
    </div>

</asp:Content>
