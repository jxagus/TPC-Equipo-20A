<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MetodosDePago.aspx.cs" Inherits="Resto_Bar_Web.MetodosDePago" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Stylesheets" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-4">
        <h2 class="mb-4">Gestion de Metodos de Pago</h2>
        <div class="row">
            <div class="col-md-4">
                <div class="card p-4 shadow-sm">
                    <h4 class="mb-3">Carga de Metodos de Pago</h4>
                    <div class="mb-3">
                        <label class="form-label fw-bold">Nombre del Metodo de Pago:</label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Escriba el nombre aca..."></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvNombreMetodo" runat="server" ControlToValidate="txtNombre" ForeColor="Red" Display="Dynamic" ErrorMessage="El nombre es obligatorio."></asp:RequiredFieldValidator>
                    </div>

                    <div class="d-flex gap-2">
                        <asp:Button ID="btnAgregarMetodoPago" CssClass="btn btn-primary" OnClick="btnAgregarMetodoPago_Click" runat="server" Text="Agregar Metodo" />
                        <asp:Button ID="btnCancelar" CssClass="btn btn-danger " OnClick="btnCancelar_Click" runat="server" Text="Cancelar" CausesValidation="false" Visible="false" />
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <div class="card p-4 shadow-sm mb-4">
                    <h4 class="mb-3">Metodos de Pago Activos</h4>
                    <asp:GridView ID="dgvMetodosDePago" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="5" OnPageIndexChanging="dgvMetodosDePago_PageIndexChanging" CssClass="table table-hover table-striped" OnRowCommand="dgvMetodosDePago_RowCommand">
                        <Columns>
                            <asp:BoundField HeaderText="IdMetodoPago" DataField="IdMetodo" />
                            <asp:BoundField HeaderText="NombreMetodoPago" DataField="NombreMetodo" />
                            <asp:TemplateField HeaderText="Acción">
                                <ItemTemplate>
                                    <asp:Button ID="btnModificar" runat="server" Text="Modificar" CommandName="Modificar" CommandArgument='<%# Eval("IdMetodo") %>' CausesValidation="false" CssClass="btn btn-secondary btn-sm" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acción">
                                <ItemTemplate>
                                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CommandName="Eliminar" CommandArgument='<%# Eval("IdMetodo") %>' CssClass="btn btn-danger btn-sm" CausesValidation="false" OnClientClick="return confirm('¿Dar de baja?');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="card p-4 shadow-sm">
                    <h4 class="mb-3 text-secondary">Metodos de Pago Inhabilitados</h4>
                    <asp:GridView ID="dgvEliminados" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="3" OnPageIndexChanging="dgvEliminados_PageIndexChanging" CssClass="table table-hover table-secondary" OnRowCommand="dgvEliminados_RowCommand">
                        <Columns>
                            <asp:BoundField HeaderText="IdMetodoPago" DataField="IdMetodo" />
                            <asp:BoundField HeaderText="NombreMetodoPago" DataField="NombreMetodo" />
                            <asp:TemplateField HeaderText="Acción">
                                <ItemTemplate>
                                    <asp:Button ID="btnReactivar" runat="server" Text="Reactivar" CommandName="Reactivar" CommandArgument='<%# Eval("IdMetodo") %>' CssClass="btn btn-success btn-sm" CausesValidation="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
