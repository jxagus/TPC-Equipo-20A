<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="Resto_Bar_Web.Insumos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid mt-2">
        <div class="row">

            <div class="col-md-4 pe-md-4 border-end">
                <h2 class="mb-4">Carga de Producto</h2>
                <asp:HiddenField ID="hfIdProducto" runat="server"/>
                <div class="mb-3" id="IdOculto" visible="false" runat="server" >
                    <label class="form-label fw-bold">ID Del Producto</label>
                    <asp:TextBox ID="txtId" CssClass="form-control" placeholder="" runat="server" disabled></asp:TextBox>
                </div>

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
                    <asp:Button ID="btnAgregarProducto" CssClass="btn btn-primary" OnClick="btnAgregarProducto_Click" runat="server" Text="Agregar Producto" />
                    <asp:Button ID="btnCancelar" CssClass="btn btn-danger " OnClick="btnCancelar_Click" runat="server" Text="Cancelar" Visible="false" />
                </div>
            </div>

            <div class="col-md-8 ps-md-4">
                <h2 class="mb-4">Productos Existentes</h2>

                <div class="table-responsive">
                    <asp:GridView ID="dgvProductos" runat="server" OnRowCommand="dgvProductos_RowCommand" AutoGenerateColumns="False" CssClass="table table-striped table-hover table-bordered align-middle">
                        <Columns>
                            <asp:BoundField DataField="IdProducto" HeaderText="ID" ItemStyle-Width="50px" />

                            <asp:BoundField DataField="NombreProducto" HeaderText="Producto" />
                            <asp:BoundField DataField="DescripcionProducto" HeaderText="Descripción" />
                            <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />

                            <asp:BoundField DataField="Stock" HeaderText="Stock" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnEditar" runat="server" OnClick="lbtnEditar_Click" CommandArgument='<%# Eval("IdProducto") %>' CssClass="text-decoration-none" ToolTip="Modificar">🖊</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnEliminar" runat="server" OnClick="lbtnEliminar_Click" CommandArgument='<%# Eval("IdProducto") %>' CommandName="AbrirModalProducto" CssClass="text-decoration-none" ToolTip="Eliminar">🗑️</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

        </div>
    </div>






         <div class="modal fade" id="modalEliminarProducto" tabindex="-1" aria-hidden="true">
          <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title" id="tituloModalEliminarProducto" runat="server">¿Esta seguro de eliminar este producto?</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
              </div>
              <div class="modal-body text-center">
        
                <div class="aler alert-secondary text-start mx-auto" style="max-width: 400px;">
                    <div class="mb-2">
                        <strong>Id del Producto: </strong>
                        <asp:Label ID="lblEliminarId" runat="server" CssClass=""></asp:Label>
                    </div>
                    <div class="mb-2">
                        <strong>Nombre: </strong>
                        <asp:Label ID="lblEliminarNombre" runat="server" CssClass=""></asp:Label>

                    </div>
                    <div class="mb-2">
                        <strong>Descripcion: </strong>
                        <asp:Label ID="lblEliminarDescripcion" runat="server" CssClass=""></asp:Label>

                    </div>
                    <div class="mb-2">
                        <strong>Precio: </strong>
                        <asp:Label ID="lblEliminarPrecio" runat="server" CssClass=""></asp:Label>

                    </div>
                    <div class="mb-2">
                        <strong>Stock Actual: </strong>
                        <asp:Label ID="lblEliminarStock" runat="server" CssClass=""></asp:Label>

                    </div>
                </div>

              </div>

                <div id="alertaEliminarProducto" class="mt-3 w-100 user-select-none">
                    <div class="alert alert-danger d-flex align-items-center justify-content-center text-center shadow-sm m-0 alertaFade" role="alert">
                        <div>
                        En caso de error puede recuperar el producto en la seccion de "Productos Eliminados"
                      </div>
                    </div>
                </div>

                <div class="d-flex flex-column align-items-center my-3 p-3 bg-light rounded-3">
            <div class="modal-footer">
                <asp:Button ID="btnCancelarEliminarProducto" runat="server" Text="Cancelar" CssClass="btn btn-secondary" data-bs-dismiss="modal" OnClientClick="return false;" />
                <asp:Button ID="btnAplicarEliminarProducto" runat="server" Text="Eliminar" CssClass="btn btn-danger"  OnClick="btnAplicarEliminarProducto_Click" />
            </div>

        </div>
            </div>
          </div>
        </div>

     
</asp:Content>
