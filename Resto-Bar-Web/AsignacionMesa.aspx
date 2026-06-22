<%@ Page Title="Gestión de Salón" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AsignacionMesa.aspx.cs" Inherits="Resto_Bar_Web.AsignacionMesa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Stylesheets" runat="server">
    <link href="~/Content/AsignacionMesa.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h3 class="mb-4">Gestion del salon</h3>

    <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="alert d-block mb-3" />

    <div class="container px-4 text-center">
        <div class="row gx-4 row-cols-1 row-cols-lg-3">

            <div class="col mb-4">
                <div class="card shadow-sm mx-auto" style="max-width: 500px;">
                    <div class="card-header bg-dark text-white fw-bold">Asignar Mesas</div>
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
                            <div class="col-6">
                                <asp:DropDownList ID="ddlMozo" runat="server" CssClass="form-select" />
                            </div>
                            <div class="col-6">
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
            </div>

            <div class="col mb-4">
                <div class="card shadow-sm mx-auto" style="max-width: 500px;">
                    <div class="card-header bg-dark text-white fw-bold">Cambiar Estado Mesa</div>
                    <div class="card-body">

                        <div class="row g-2 mb-1">
                            <div class="col-6">
                                <label class="form-label fw-semibold">Acción</label>
                            </div>
                            <div class="col-6">
                                <label class="form-label fw-semibold">Mesa</label>
                            </div>
                        </div>

                        <div class="row g-2 align-items-center mb-2">
                            <div class="col-6">
                                <asp:DropDownList ID="ddlAccion" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlAccion_SelectedIndexChanged" />
                            </div>
                            <div class="col-6">
                                <asp:DropDownList ID="ddlMesas" runat="server" CssClass="form-select" />
                            </div>
                        </div>

                        <div class="d-flex justify-content-end mt-3 me-2">
                            <asp:Button ID="btnAceptar" runat="server"
                                Text="Aceptar"
                                CssClass="btn btn-success px-4"
                                OnClick="btnAceptar_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="col mb-4">
                <div class="card shadow-sm mx-auto" style="max-width: 500px;">
                    <div class="card-header bg-dark text-white fw-bold">Estructura de Mesas</div>
                    <div class="card-body">

                        <div class="row align-items-center my-2">
                            <div class="col-7 text-start">
                                <label class="form-label fw-semibold mb-0">Próxima Mesa a Crear:</label>
                                <h4 class="text-primary fw-bold mt-1 mb-0">Mesa Nº
                                    <asp:Literal ID="litProximaMesa" runat="server"></asp:Literal>
                                </h4>
                            </div>
                            <div class="col-5 text-end">
                                <asp:Button ID="btnCrearMesa" runat="server"
                                    Text="Agregar Mesa"
                                    CssClass="btn btn-success w-100"
                                    OnClick="btnCrearMesa_Click" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
