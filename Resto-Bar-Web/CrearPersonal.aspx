<%@ Page Title="Gestión de Personal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CrearPersonal.aspx.cs" Inherits="Resto_Bar_Web.CrearPersonal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-4">
        <h2 class="mb-4">Gestión de Personal</h2>
        <div class="row">
            <div class="col-md-4">
                <div class="card p-4 shadow-sm">
                    <h4 class="mb-3">Alta de Personal</h4>
                    <div class="mb-3">
                        <label class="form-label fw-bold">Nombre:</label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ej:Ramiro Agustin"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label fw-bold">Apellido:</label>
                        <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" placeholder="Ej:Alvarez"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label fw-bold">Usuario:</label>
                        <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control" placeholder="Ej:Ralvarez"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label fw-bold">Contraseña:</label>
                        <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password" placeholder="••••••••" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label fw-bold">Repetir Contraseña:</label>
                        <asp:TextBox ID="txtRepetirContrasena" runat="server" TextMode="Password" placeholder="••••••••" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label fw-bold">Rol:</label>
                        <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-select">
                            <asp:ListItem Value="2">Mozo</asp:ListItem>
                            <asp:ListItem Value="1">Gerente</asp:ListItem>
                            <asp:ListItem Value="0">Admin</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    
                    <div class="mb-3">
                        <asp:Label ID="lblMensajeError" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                    </div>

                    <div class="d-flex gap-2">
                        <asp:Button ID="btnRegistrar" runat="server" Text="Guardar Personal" CssClass="btn btn-primary" OnClick="btnRegistrar_Click" />
                        <a href="Mesas.aspx" class="btn btn-secondary">Cancelar</a>
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <div class="card p-4 shadow-sm mb-4">
                    <h4 class="mb-3">Personal Activo</h4>
                    <asp:GridView ID="dgvUsuarios" runat="server" AutoGenerateColumns="false" CssClass="table table-hover table-striped" AllowPaging="true" PageSize="5" OnPageIndexChanging="dgvUsuarios_PageIndexChanging" OnRowCommand="dgvUsuarios_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Personal"><ItemTemplate><%# Eval("Apellido") + ", " + Eval("Nombre") %></ItemTemplate></asp:TemplateField>
                            <asp:BoundField HeaderText="Usuario" DataField="NombreUsuario" />
                            <asp:TemplateField HeaderText="Rol"><ItemTemplate><%# ObtenerNombreRol((int)Eval("IdRol")) %></ItemTemplate></asp:TemplateField>
                            <asp:TemplateField HeaderText="Acción">
                                <ItemTemplate>
                                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CommandName="Eliminar" CommandArgument='<%# Eval("IdUsuario") %>' CssClass="btn btn-danger btn-sm" OnClientClick="return confirm('¿Dar de baja?');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="card p-4 shadow-sm">
                    <h4 class="mb-3 text-secondary">Personal dado de baja</h4>
                    <asp:GridView ID="dgvEliminados" runat="server" AutoGenerateColumns="false" CssClass="table table-hover table-secondary" AllowPaging="true" PageSize="5" OnPageIndexChanging="dgvEliminados_PageIndexChanging" OnRowCommand="dgvEliminados_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Personal"><ItemTemplate><%# Eval("Apellido") + ", " + Eval("Nombre") %></ItemTemplate></asp:TemplateField>
                            <asp:BoundField HeaderText="Usuario" DataField="NombreUsuario" />
                            <asp:TemplateField HeaderText="Acción">
                                <ItemTemplate>
                                    <asp:Button ID="btnReactivar" runat="server" Text="Reactivar" CommandName="Reactivar" CommandArgument='<%# Eval("IdUsuario") %>' CssClass="btn btn-success btn-sm" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>