<%@ Page Title="Crear Personal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CrearPersonal.aspx.cs" Inherits="Resto_Bar_Web.CrearPersonal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="container mt-4">
        <h2 class="mb-4">Alta de Personal</h2>
        
        <div class="card p-4 shadow-sm" style="max-width: 500px;">
            
            <div class="mb-3">
                <label class="form-label fw-bold">Nombre Completo:</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ej: Juan Pérez"></asp:TextBox>
            </div>

            <div class="mb-3">
                <label class="form-label fw-bold">Nombre de Usuario (Login):</label>
                <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control" placeholder="Ej: jperez"></asp:TextBox>
            </div>

            <div class="mb-3">
                <label class="form-label fw-bold">Contraseña:</label>
                <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password" CssClass="form-control" placeholder="••••••••"></asp:TextBox>
            </div>

            <div class="mb-4">
                <label class="form-label fw-bold">Rol Asignado:</label>
                <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-select">
                    <asp:ListItem Value="2">Mozo</asp:ListItem>
                    <asp:ListItem Value="1">Gerente</asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="d-flex gap-2">
                <asp:Button ID="btnRegistrar" runat="server" Text="Guardar Personal" CssClass="btn btn-primary" OnClick="btnRegistrar_Click" />

                <a href="Mesas.aspx" class="btn btn-secondary">Cancelar</a>
            </div>

        </div>
    </div>

</asp:Content>