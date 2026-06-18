<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="Resto_Bar_Web.WebForm2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="mb-4">Inicio de Sesion</h1>

    <div class="mb-3">
        <label for="exampleFormControlInput1" class="form-label">Nombre de Usuario</label>
        <asp:TextBox ID="txtUsuario" class="form-control" placeholder="Usuario" runat="server"></asp:TextBox>
    </div>
    <div class="mb-3">
        <label for="inputPassword5" class="form-label">Password</label>
        <asp:TextBox ID="txtContrasena" CssClass="form-control" aria-describedby="passwordHelpBlock" placeholder="******" type="password" runat="server"></asp:TextBox>
    </div>
    <div class="mb-3">
        <asp:Button ID="btnIniciarSesion" CssClass="btn btn-secondary btn-sm" OnClick="btnIniciarSesion_Click" runat="server" Text="Iniciar Sesion" />
    </div>
</asp:Content>
