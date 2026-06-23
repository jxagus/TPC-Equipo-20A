<%@ Page Title="Usuario" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Resto_Bar_Web.WebForm2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="d-flex flex-grow-1 justify-content-center align-items-center" style="min-height: 70vh; width: 100%;">

        <asp:Panel ID="pnlLogin" runat="server" CssClass="card p-4 shadow-sm text-start" Style="width: 100%; max-width: 380px; border-radius: 12px; background-color: #ffffff;">
            <h2 class="text-center mb-4 fw-bold text-secondary">Inicio de Sesión</h2>

            <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block text-center py-2 mb-3" Visible="false"></asp:Label>

            <div class="mb-3">
                <label class="form-label fw-semibold">Nombre de Usuario</label>
                <asp:TextBox ID="txtUsuario" class="form-control" placeholder="Nombre de usuario" runat="server"></asp:TextBox>
            </div>

            <div class="mb-4">
                <label class="form-label fw-semibold">Contraseña</label>
                <asp:TextBox ID="txtContrasena" CssClass="form-control" placeholder="******" TextMode="Password" runat="server"></asp:TextBox>
            </div>

            <div class="d-flex justify-content-center">
                <asp:Button ID="btnIniciarSesion" CssClass="btn btn-primary px-5" OnClick="btnIniciarSesion_Click" runat="server" Text="Ingresar al Sistema" />
            </div>
        </asp:Panel>


        <asp:Panel ID="pnlPerfil" runat="server" CssClass="card p-4 shadow-sm text-center" Style="width: 100%; max-width: 380px; border-radius: 12px; background-color: #ffffff;" Visible="false">

            <div class="mb-3 mt-2">
                <span style="font-size: 60px;">👤</span>
            </div>

            <h3 class="fw-bold text-secondary mb-4">Sesión Activa</h3>

            <div class="d-grid gap-3 w-100 px-2">
                <asp:Button ID="btnCerrarSesion" CssClass="btn btn-danger btn-lg fs-6" OnClick="btnCerrarSesion_Click" runat="server" Text="Cerrar Sesión" />

                <asp:Button ID="btnIrDashboard" CssClass="btn btn-outline-secondary btn-lg fs-6" OnClick="btnIrDashboard_Click" runat="server" Text="Ir al Dashboard" />
            </div>

        </asp:Panel>

    </div>

</asp:Content>
