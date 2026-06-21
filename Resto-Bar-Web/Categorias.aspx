<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Categorias.aspx.cs" Inherits="Resto_Bar_Web.Categorias" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Stylesheets" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

        <div class="container-fluid mt-2">
            <div class="row">
                <div class="col-md-4 pe-md-4 border-end">
                    <h2 class="mb-4">Carga de Categorias</h2>
                    <asp:HiddenField ID="hfIdCategoria" runat="server"/>


                    <div class="mb-3">
                        <label class="form-label fw-bold">Nombre de la Categoria</label>
                        <asp:TextBox ID="txtNombreCategoria" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>

         
                    <div class="mb-3">
                        <div class="form-check">
                          <asp:CheckBox ID="chkSubcategoria" runat="server" AutoPostBack="true" OnCheckedChanged="chkSubcategoria_CheckedChanged"/>
                          <label class="form-check-label" for="chkSubcategoria">
                            Es Subcategoria
                          </label>
                        </div> 
                    </div>
                    <div class="mb-3" id="divSubcategorias" runat="server" visible="false">
                        <label class="form-label fw-bold">Subcategoria de:</label>
                        <asp:DropDownList ID="ddlCategoriaPadre" CssClass="form-select" runat="server"></asp:DropDownList>
                    </div>

                    <div class="mb-3">
                        <asp:Button ID="btnAgregarCategoria" CssClass="btn btn-primary" OnClick="btnAgregarCategoria_Click"  runat="server" Text="Agregar Categoria" />
                        <asp:Button ID="btnCancelar" CssClass="btn btn-danger " OnClick="btnCancelar_Click" runat="server" Text="Cancelar" Visible="false" />
                    </div>
                </div>


            </div>
        </div>


</asp:Content>
