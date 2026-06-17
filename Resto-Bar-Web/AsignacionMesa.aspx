<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AsignacionMesa.aspx.cs" Inherits="Resto_Bar_Web.AsignacionMesa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Stylesheets" runat="server">
    <link href="~/Content/AsignacionMesa.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h3 class="mb-4">Nueva Asignación de Mesa</h3>

    <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="alert d-block mb-3" />

    <div class="card shadow-sm" style="max-width: 900px;">
        <div class="card-header bg-dark text-white">Datos de la asignación</div>
        <div class="card-body">

            <div class="row g-2 mb-1">
                <div class="col-5">
                    <label class="form-label fw-semibold">Mozo</label>
                </div>
                <div class="col-5">
                    <label class="form-label fw-semibold">Mesa</label>
                </div>
                <div class="col-2"></div>
            </div>

            <div class="row g-2 align-items-center mb-2">
                <div class="col-5">
                    <asp:DropDownList ID="ddlMozo" runat="server" CssClass="form-select" />
                </div>
                <div class="col-5">
                    <asp:DropDownList ID="ddlMesa" runat="server" CssClass="form-select" />
                </div>
            </div>

            <div class="d-flex justify-content-end mt-3 me-2">
                <asp:Button ID="btnAsignar" runat="server"
                            Text="Asignar"
                            CssClass="btn btn-success px-4"
                            OnClick="btnAsignar_Click" />
            </div>
        </div>
    </div>
</asp:Content>
