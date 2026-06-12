<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AsignacionMesa.aspx.cs" Inherits="Resto_Bar_Web.AsignacionMesa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Stylesheets" runat="server">
    <link href="~/Content/AsignacionMesa.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h3 class="mb-4">Nueva Asignación de Mesa</h3>

    <div class="card shadow-sm" style="max-width: 750px;">
        <div class="card-header bg-dark text-white">
            Datos de la asignación
        </div>
        <div class="card-body">

            <%-- ENCABEZADO COLUMNAS --%>
            <div class="row g-2 mb-1">
                <div class="col-5">
                    <label class="form-label fw-semibold">Mozo</label>
                </div>
                <div class="col-5">
                    <label class="form-label fw-semibold">Mesa</label>
                </div>
                <div class="col-2"></div>
            </div>

            <%-- FILAS DE ASIGNACIÓN --%>
            <div id="contenedorFilas">
                <div class="row g-2 align-items-center mb-2 fila-asignacion">
                    <div class="col-5">
                        <select class="form-select"></select>
                    </div>
                    <div class="col-5">
                        <select class="form-select"></select>
                    </div>
                    <div class="col-2 d-flex gap-1">
                        <button type="button" class="btn btn-outline-secondary btnAgregar" onclick="agregarFila()">
                            <i class="bi bi-plus"></i>
                        </button>
                        <button type="button" class="btn btn-outline-danger btnEliminar" style="display: none;" onclick="eliminarFila(this)">
                            <i class="bi bi-dash"></i>
                        </button>
                    </div>
                </div>
            </div>

            
            <div class="d-flex justify-content-end mt-3 me-2">
                <asp:Button runat="server" Text="✅ Asignar" CssClass="btn btn-success px-2" />
            </div>

        </div>
    </div>

    <script>
        function agregarFila() {
            var contenedor = document.getElementById('contenedorFilas');
            var filas = contenedor.querySelectorAll('.fila-asignacion');
            var ultima = filas[filas.length - 1];

            var nueva = ultima.cloneNode(true);
            nueva.querySelectorAll('select').forEach(s => s.selectedIndex = 0);

            // En la nueva fila: ocultar +, mostrar -
            nueva.querySelector('.btnAgregar').style.display = 'none';
            nueva.querySelector('.btnEliminar').style.display = 'inline-block';

            // En la anterior: ocultar +, mostrar -
            ultima.querySelector('.btnAgregar').style.display = 'none';
            ultima.querySelector('.btnEliminar').style.display = 'inline-block';

            contenedor.appendChild(nueva);
            actualizarBotones();
        }

        function eliminarFila(btn) {
            var contenedor = document.getElementById('contenedorFilas');
            btn.closest('.fila-asignacion').remove();
            actualizarBotones();
        }

        function actualizarBotones() {
            var filas = document.querySelectorAll('.fila-asignacion');
            filas.forEach(function (fila, index) {
                var btnAgregar = fila.querySelector('.btnAgregar');
                var btnEliminar = fila.querySelector('.btnEliminar');

                // + solo en la última fila
                btnAgregar.style.display = index === filas.length - 1 ? 'inline-block' : 'none';

                // - en todas MENOS en la última
                if (filas.length === 1) {
                    btnEliminar.style.display = 'none';
                } else {
                    btnEliminar.style.display = index === filas.length - 1 ? 'none' : 'inline-block';
                }
            });
        }
    </script>

</asp:Content>
