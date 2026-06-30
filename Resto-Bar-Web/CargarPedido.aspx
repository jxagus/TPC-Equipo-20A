<%@ Page Title="Cargar Pedido" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CargarPedido.aspx.cs" Inherits="Resto_Bar_Web.CargarPedido" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Stylesheets" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-3">
        
        <div class="d-flex justify-content-between align-items-center mb-4">
            <h2 class="mb-0">📋 Nueva Orden / Cargar Pedido</h2>
            
            <div>
                <button type="button" class="btn btn-warning btn-lg position-relative fw-bold shadow-sm" data-bs-toggle="modal" data-bs-target="#modalRevision">
                    🛒 Revisar Pedido Actual
                    <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
                        <asp:Label ID="lblCantidadItems" runat="server" Text="0"></asp:Label>
                    </span>
                </button>
            </div>
        </div>

        <div class="card mb-4 shadow-sm">
            <div class="card-header bg-degrade-pasos fw-bold text-white">
                Información del Pedido
            </div>
            <div class="card-body">
                <div class="row align-items-center">
                    <div class="col-md-6">
                        <div class="d-flex align-items-center gap-2">
                            <label class="form-label fw-bold mb-0 fs-5 text-secondary">Mesa Seleccionada:</label>
                            <asp:Label ID="lblMesaSeleccionada" runat="server" CssClass="badge bg-primary fs-5 px-3 py-2" Text="Ninguna (Error)"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="card mb-4 shadow-sm">
            <div class="card-header bg-degrade-pasos fw-bold text-white">
                Paso 2: Seleccionar Productos
            </div>
            <div class="card-body">
                <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-4 g-4">
                    <asp:Repeater ID="repProductos" runat="server" OnItemCommand="repProductos_ItemCommand">
                        <ItemTemplate>
                            <div class="col">
                                <div class="card h-100 border-secondary shadow-sm d-flex flex-column justify-content-between">
                                    
                                    <div class="card-body text-center">
                                        <h5 class="card-title fw-bold text-uppercase mb-2">
                                            <%# Eval("NombreProducto") %>
                                        </h5>
                                        <p class="card-text text-muted small">
                                            <%# Eval("DescripcionProducto") %>
                                        </p>
                                    </div>

                                    <div class="card-footer bg-transparent border-0 text-center pb-3">
                                        <div class="badge bg-light text-dark border w-100 py-2 mb-2">
                                            Disponibles: <span class="fw-bold text-danger"><%# Eval("Stock") %></span> uds.
                                        </div>
                                        <h4 class="text-success fw-bold mb-3">
                                            <%# string.Format("{0:C}", Eval("Precio")) %>
                                        </h4>
                                        <asp:Button ID="btnAgregar" runat="server" Text="➕ Añadir"
                                            CssClass="btn btn-outline-primary w-100 fw-bold"
                                            CommandName="AgregarProducto"
                                            CommandArgument='<%# Eval("IdProducto") %>' />
                                    </div>

                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modalRevision" tabindex="-1" aria-labelledby="modalRevisionLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header bg-degrade-pasos text-white">
                        <h5 class="modal-title fw-bold" id="modalRevisionLabel">🛒 Revisar Detalle del Pedido</h5>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <%///LIMPIAR CADA VEZ QUE SE ENTRA A CARGAR PEDIDO %>
                        <asp:GridView ID="dgvPedidoActual" runat="server" AutoGenerateColumns="False"
                            CssClass="table table-hover table-striped align-middle" OnRowDeleting="dgvPedidoActual_RowDeleting">
                            <Columns>
                                <asp:BoundField DataField="NombreProducto" HeaderText="Producto" />
                                <asp:BoundField DataField="Cantidad" HeaderText="Cant." ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio Unit." DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" />
                                <asp:CommandField ShowDeleteButton="True" DeleteText="❌ Quitar" ControlStyle-CssClass="btn btn-sm btn-outline-danger" />
                            </Columns>
                        </asp:GridView>

                        <div class="text-end mt-3 pe-2">
                            <h3>Total: <span class="text-success fw-bold">
                                <asp:Label ID="lblTotalPedido" runat="server" Text="$0.00"></asp:Label></span></h3>
                        </div>
                    </div>
                    <div class="modal-footer bg-light">
                        <asp:Button ID="btnConfirmarPedido" runat="server"
                            Text="Confirmar y Enviar a Cocina" 
                            CssClass="btn btn-success fw-bold px-4" 
                            UseSubmitBehavior="false"
                            OnClick="btnConfirmarPedido_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>