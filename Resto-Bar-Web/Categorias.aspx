<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Categorias.aspx.cs" Inherits="Resto_Bar_Web.Categorias" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Stylesheets" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

        <div class="container-fluid mt-2">
            <div class="row">
                <div class="col-md-4 pe-md-4 border-end">
                    <h2 class="border-bottom pb-2 mb-4">Carga de Categorias</h2>
                    <asp:HiddenField ID="hfIdCategoria" runat="server"/>


                    <div class="mb-3">
                        <label class="form-label fw-bold">Nombre de la Categoria</label>
                        <asp:TextBox ID="txtNombreCategoria" CssClass="form-control" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvNombreCategoria" runat="server" ControlToValidate="txtNombreCategoria" ForeColor="Red" Display="Dynamic" ErrorMessage="El nombre es obligatorio."></asp:RequiredFieldValidator>
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



            <div class="col-md-8 ps-md-4">
              <h2 class="border-bottom pb-2 mb-4">Categorias Existentes</h2>

                <div class="accordion" id="accorditionCategorias">
                    <asp:Repeater Id="repCategorias" runat="server" OnItemCommand="repCategorias_ItemCommand" OnItemDataBound="repCategorias_ItemDataBound">
                    <ItemTemplate>

                        <div class="accordion-item">
                            <h2 class="accordion-header d-flex" id='heading<%# Eval("IdCategoria") %>'>
                              <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target='#collapse<%# Eval("IdCategoria") %>' aria-expanded="false" aria-controls='collapse<%# Eval("IdCategoria") %>'>
                                <%# Eval("NombreCategoria") %>
                              </button>
                              <div class="d-flex align-items-center p-2 bg-light border-start">
                                  <asp:LinkButton ID="btnEditarCategoria" CommandName="EditarCategoria" CommandArgument='<%# Eval("IdCategoria") %>' CssClass="btn btn-sm btn-outline-secondary" runat="server" CausesValidation="false" ToolTip="Editar">🖊</asp:LinkButton>
                              </div>
                            </h2>

                            <div id='collapse<%# Eval("IdCategoria") %>' class="accordion-collapse collapse" aria-labelledby='heading<%# Eval("IdCategoria") %>' data-bs-parent="#accorditionCategorias">
                            <div class="accordion-body">
                                <ul class="list-group list-group-flush">
                                    <asp:Repeater ID="repSubcategorias" OnItemCommand="repSubcategorias_ItemCommand" runat="server">
                                        <ItemTemplate>
                                            <li class="list-group-item d-flex justify-content-between align-items-center">
                                                <%# Eval("NombreCategoria") %>
                                                <asp:LinkButton ID="btnEditarSubcategoria" CommandName="EditarSubcategoria" CommandArgument='<%# Eval("IdCategoria") %>' CssClass="btn btn-sm btn-outline-secondary" runat="server" CausesValidation="false" ToolTip="Editar">🖊</asp:LinkButton>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>

                                </div>
                            </div>

                        </div>

                    </ItemTemplate>
                    </asp:Repeater>
                </div>


            </div>
            </div>

        </div>


</asp:Content>
