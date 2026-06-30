using Antlr.Runtime.Misc;
using Dominio;
using Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Resto_Bar_Web
{
    public partial class Insumos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["idUsuario"] == null)
                {
                    Response.Redirect("Login.aspx", false);
                    return;
                }

                LoginNegocio negocio = new LoginNegocio();
                int idUsuario = Convert.ToInt32(Session["idUsuario"]);
                int rol = negocio.traerRol(idUsuario);

                if (rol == 0 || rol == 1)
                {
                    if (!IsPostBack)
                    {
                        CargarProductos();
                        cargarDDLS();
                    }
                }
                else
                {

                    throw new Exception("Permisos insuficientes: Su nivel de usuario no está autorizado para acceder a la gestión de productos.");
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }
        private void CargarProductos()
        {
            try
            {
                ProductoNegocio negocio = new ProductoNegocio();

                dgvProductos.DataSource = negocio.listar();

                dgvProductos.DataBind();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }
        protected void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }
            try
            {
                
                string nombre = txtNombreProducto.Text;
                string descripcion = txtDescripcion.Text;
                decimal precio = Convert.ToDecimal(txtPrecio.Text.Replace(",","."), System.Globalization.CultureInfo.InvariantCulture);
                int stock = Convert.ToInt32(txtStock.Text);

                ProductoNegocio negocio = new ProductoNegocio();
                /////////////Parte de modificar
                if (btnAgregarProducto.Text == "Modificar")
                {
                    int id = Convert.ToInt32(hfIdProducto.Value);
                    negocio.modificarProducto(id, nombre, descripcion, precio, stock);
                    CategoriaNegocio catNegocio = new CategoriaNegocio();
                    catNegocio.eliminarCategoriasProducto(id);
                    guardarCategoriasSeleccionadas(id);

                    hfIdProducto.Value = "";
                }
                else//////////////Parte de Agregar nuevo
                {
                    int idNuevoProducto = negocio.agregarProducto(nombre, descripcion, precio, stock);
                    guardarCategoriasSeleccionadas(idNuevoProducto);
                }

                txtDescripcion.Text = null;
                txtNombreProducto.Text = null;
                txtPrecio.Text = null;
                txtStock.Text = null;
                divSubcategorias.Visible = false;
                ddlCategoria.SelectedIndex = 0;
                IdOculto.Visible = false;
                btnCancelar.Visible = false;
                btnAgregarProducto.Text = "Agregar Producto";
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Producto agregado exitosamente!');", true);

                CargarProductos();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("error.aspx", false);
            }
        }

        protected void lbtnEditar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int idSeleccionado = Convert.ToInt32(btn.CommandArgument);

            txtId.Text = idSeleccionado.ToString();
            hfIdProducto.Value = idSeleccionado.ToString();
            IdOculto.Visible = true;
            btnCancelar.Visible = true;
            btnAgregarProducto.Text = "Modificar";
            cargarFormulario(idSeleccionado);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            txtNombreProducto.Text = null;
            txtDescripcion.Text = null;
            txtPrecio.Text = null;
            txtStock.Text = null;
            IdOculto.Visible = false;
            btnCancelar.Visible = false;
            divSubcategorias.Visible = false;
            ddlCategoria.SelectedIndex = 0;
            btnAgregarProducto.Text = "Agregar Producto";
        }
        
        void cargarFormulario(int id)
        {
            
            Productos producto = new Productos();
            ProductoNegocio negocio = new ProductoNegocio();
            producto = negocio.cargarProductoPorId(id);
            cargarFormularioCategorias(id);
            txtNombreProducto.Text = producto.NombreProducto;
            txtDescripcion.Text = producto.DescripcionProducto;
            txtPrecio.Text = producto.Precio.ToString();
            txtStock.Text = producto.Stock.ToString();
        
        }

        protected void lbtnEliminar_Click(object sender, EventArgs e)
        {

        }

        protected void btnAplicarEliminarProducto_Click(object sender, EventArgs e)
        {
            ProductoNegocio negocio = new ProductoNegocio();
            int id = Convert.ToInt32(hfIdProducto.Value);
            negocio.desactivarProducto(id);
            hfIdProducto.Value = "";
            CargarProductos();

        }

        protected void dgvProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AbrirModalProducto")
            {

                string idProducto = e.CommandArgument.ToString();
                Session["IdProductoAEliminar"] = idProducto;
                hfIdProducto.Value = idProducto;

                ProductoNegocio negocio = new ProductoNegocio();
                Productos seleccionado = negocio.cargarProductoPorId(int.Parse(idProducto));
                
                lblEliminarId.Text = idProducto;
                lblEliminarNombre.Text = seleccionado.NombreProducto;
                lblEliminarDescripcion.Text = seleccionado.DescripcionProducto.ToString();
                lblEliminarPrecio.Text = "$" + seleccionado.Precio.ToString("0.00");
                lblEliminarStock.Text = seleccionado.Stock.ToString();



                string script = "var miModal = new bootstrap.Modal(document.getElementById('modalEliminarProducto')); miModal.show();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PopProducto", script, true);
            }
        }

        protected void btnAgregarCategoria_Click(object sender, EventArgs e)
        {

        }

        protected void ddlCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idCategoriaSeleccionada = Convert.ToInt32(ddlCategoria.SelectedValue);

            if (idCategoriaSeleccionada == 0)
            {
                divSubcategorias.Visible = false;
                cklSubcategorias.Items.Clear();
                return;
            }
            CategoriaNegocio catNegocio = new CategoriaNegocio();
            List<Dominio.Categorias> listaTodas = catNegocio.listarTODAS();

            List<Dominio.Categorias> subcategorias = listaTodas.FindAll(c => c.IdCategoriaPadre == idCategoriaSeleccionada);

            if(subcategorias.Count > 0)
            {
                cklSubcategorias.DataSource = subcategorias;
                cklSubcategorias.DataValueField = "IdCategoria";
                cklSubcategorias.DataTextField = "NombreCategoria";
                cklSubcategorias.DataBind();

                divSubcategorias.Visible = true;
            }
            else
            {
                divSubcategorias.Visible = false;
            }
        }

        protected void guardarCategoriasSeleccionadas(int idProducto)
        {
            if (idProducto > 0)
            {
                CategoriaNegocio catNegocio = new CategoriaNegocio();
                int idCategoriaSelec = Convert.ToInt32(ddlCategoria.SelectedValue);
                if (idCategoriaSelec > 0)
                {
                    catNegocio.asignarCategoriaaProducto(idProducto, idCategoriaSelec);
                }
                foreach (ListItem item in cklSubcategorias.Items)
                {
                    if (item.Selected)
                    {
                        int idSubcategoria = Convert.ToInt32(item.Value);
                        catNegocio.asignarCategoriaaProducto(idProducto, idSubcategoria);

                    }
                }
            }
        }

        protected void cargarFormularioCategorias(int id)
        {
            CategoriaNegocio catnegocio = new CategoriaNegocio();
            List<Dominio.Categorias> categoriasAsignadas = catnegocio.listarCategoriasxProducto(id);
            Dominio.Categorias catPrincipal = categoriasAsignadas.Find(c => c.IdCategoriaPadre == 0);
            if (catPrincipal != null)
            {
                ddlCategoria.SelectedValue = catPrincipal.IdCategoria.ToString();
                List<Dominio.Categorias> todas = catnegocio.listarTODAS();
                List<Dominio.Categorias> subcategorias = todas.FindAll(c => c.IdCategoriaPadre == catPrincipal.IdCategoria);
                if(subcategorias.Count > 0)
                {
                    cklSubcategorias.DataSource = subcategorias;
                    cklSubcategorias.DataValueField = "IdCategoria";
                    cklSubcategorias.DataTextField = "NombreCategoria";
                    cklSubcategorias.DataBind();
                    divSubcategorias.Visible = true;

                    foreach (ListItem item in cklSubcategorias.Items)
                    {
                        int idSub = Convert.ToInt32(item.Value);
                        if (categoriasAsignadas.Exists(c => c.IdCategoria == idSub))
                        {
                            item.Selected = true;
                        }
                    }
                }
                else
                {
                    divSubcategorias.Visible = false;
                }
            }
            else
            {
                ddlCategoria.SelectedIndex = 0;
                divSubcategorias.Visible = false;
                cklSubcategorias.Items.Clear();
            }
        }

        protected void lbtnVerCategorias_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                int idProducto = Convert.ToInt32(btn.CommandArgument);
                CategoriaNegocio catnegocio = new CategoriaNegocio();
                List <Dominio.Categorias> listaCategorias = catnegocio.listarCategoriasxProducto(idProducto);

                if(listaCategorias != null && listaCategorias.Count > 0 )
                {
                    lblSinCategoria.Visible = false;
                    repCategoriasModal.DataSource = listaCategorias;
                    repCategoriasModal.DataBind();
                }
                else
                {
                    repCategoriasModal.DataSource = null;
                    repCategoriasModal.DataBind();
                    lblSinCategoria.Visible = true;

                }
                string script = "var myModal = new bootstrap.Modal(document.getElementById('modalCategorias')); myModal.show();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PopVerCategorias", script, true);
            }
            catch (Exception ex )
            {

                throw ex;
            }

        }

        protected void dgvProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvProductos.PageIndex = e.NewPageIndex;
            ProductoNegocio negocio = new ProductoNegocio();
            List<Productos> lista = negocio.listar();

            string criterio = ViewState["CriterioOrden"]?.ToString();
            string orden = ViewState["DireccionOrden"]?.ToString();
            if (criterio == "0" || orden == "0")
            {
                CargarProductos();
                return;
            }

            if (orden == "ASC")
            {
                if (criterio == "IdProducto") lista = lista.OrderBy(x => x.IdProducto).ToList();
                if (criterio == "Nombre") lista = lista.OrderBy(x => x.NombreProducto).ToList();
                if (criterio == "Stock") lista = lista.OrderBy(x => x.Stock).ToList();
                if (criterio == "Precio") lista = lista.OrderBy(x => x.Precio).ToList();
            }
            else
            {
                if (criterio == "IdProducto") lista = lista.OrderByDescending(x => x.IdProducto).ToList();
                if (criterio == "Nombre") lista = lista.OrderByDescending(x => x.NombreProducto).ToList();
                if (criterio == "Stock") lista = lista.OrderByDescending(x => x.Stock).ToList();
                if (criterio == "Precio") lista = lista.OrderByDescending(x => x.Precio).ToList();

            }

            dgvProductos.DataSource = lista;
            dgvProductos.DataBind();
        }

        protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlDireccion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlCategoriasFiltrado_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idCategoriaSeleccionada = Convert.ToInt32(ddlCategoriasFiltrado.SelectedValue);

            if (idCategoriaSeleccionada == 0)
            {
                divFiltroSubcategorias.Visible = false;
                cklFiltroSubcategorias.Items.Clear();
                return;
            }
            CategoriaNegocio catNegocio = new CategoriaNegocio();
            List<Dominio.Categorias> listaTodas = catNegocio.listarTODAS();

            List<Dominio.Categorias> subcategorias = listaTodas.FindAll(c => c.IdCategoriaPadre == idCategoriaSeleccionada);

            if (subcategorias.Count > 0)
            {
                cklFiltroSubcategorias.DataSource = subcategorias;
                cklFiltroSubcategorias.DataValueField = "IdCategoria";
                cklFiltroSubcategorias.DataTextField = "NombreCategoria";
                cklFiltroSubcategorias.DataBind();

                divFiltroSubcategorias.Visible = true;
            }
            else
            {
                divFiltroSubcategorias.Visible = false;
            }
        }

        protected void cargarDDLS()
        {
            CategoriaNegocio catnegocio = new CategoriaNegocio();

            ddlCategoria.DataSource = catnegocio.CargarCategorias();
            ddlCategoria.DataValueField = "IdCategoria";
            ddlCategoria.DataTextField = "NombreCategoria";
            ddlCategoria.DataBind();
            ddlCategoria.Items.Insert(0, new ListItem("Seleccione una categoría...", "0"));

            ddlCategoriasFiltrado.DataSource = catnegocio.CargarCategorias();
            ddlCategoriasFiltrado.DataValueField = "IdCategoria";
            ddlCategoriasFiltrado.DataTextField = "NombreCategoria";
            ddlCategoriasFiltrado.DataBind();
            ddlCategoriasFiltrado.Items.Insert(0, new ListItem("Seleccione una categoría...", "0"));

            ddlFiltro.Items.Add(new ListItem("ID", "IdProducto"));
            ddlFiltro.Items.Add(new ListItem("Nombre Producto", "Nombre"));
            ddlFiltro.Items.Add(new ListItem("Precio", "Precio"));
            ddlFiltro.Items.Add(new ListItem("Stock", "Stock"));
            ddlDireccion.Items.Add(new ListItem("Ascendente","ASC"));
            ddlDireccion.Items.Add(new ListItem("Descendente", "DESC"));

        }

        protected void btnAplicarFiltros_Click(object sender, EventArgs e)
        {
            ProductoNegocio negocio = new ProductoNegocio();
            CategoriaNegocio catNegocio = new CategoriaNegocio();
            List<Productos> lista = negocio.listar();
            string criterio = ddlFiltro.SelectedValue;
            string orden = ddlDireccion.SelectedValue;
            ViewState["CriterioOrden"] = criterio;
            ViewState["DireccionOrden"] = orden;
            List<int> idsMarcados = new List<int>();
            foreach (ListItem item in cklFiltroSubcategorias.Items)
            {
                if (item.Selected)
                {
                    idsMarcados.Add(int.Parse(item.Value));

                }
            }

            if (ddlCategoriasFiltrado.SelectedValue != "0")
            {
                int idCat = int.Parse(ddlCategoriasFiltrado.SelectedValue);
                List<int> idProductos = catNegocio.ListarIdsProductosPorCategoria(idCat);
                lista = lista.FindAll(p => idProductos.Contains(p.IdProducto));
            }

            if (idsMarcados.Count > 0)
            {
                List<int> idsFiltrados = catNegocio.ListarIdsProductosPorSubategoria(idsMarcados);
                lista = lista.FindAll(p => idsFiltrados.Contains(p.IdProducto));
                
            }

            if(orden == "ASC")
            {
                if(criterio == "IdProducto") lista = lista.OrderBy(x => x.IdProducto).ToList();
                if (criterio == "Nombre") lista = lista.OrderBy(x => x.NombreProducto).ToList();
                if (criterio == "Stock") lista = lista.OrderBy(x => x.Stock).ToList();
                if (criterio == "Precio") lista = lista.OrderBy(x => x.Precio).ToList();
            }
            else
            {
                if (criterio == "IdProducto") lista = lista.OrderByDescending(x => x.IdProducto).ToList();
                if (criterio == "Nombre") lista = lista.OrderByDescending(x => x.NombreProducto).ToList();
                if (criterio == "Stock") lista = lista.OrderByDescending(x => x.Stock).ToList();
                if (criterio == "Precio") lista = lista.OrderByDescending(x => x.Precio).ToList();

            }

            dgvProductos.DataSource = lista;
            dgvProductos.DataBind();


        }


        protected void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            ddlFiltro.SelectedIndex = 0;
            ddlDireccion.SelectedIndex = 0;
            ddlCategoriasFiltrado.SelectedIndex = 0;
            divFiltroSubcategorias.Visible = false;
            CargarProductos();
            ViewState["CriterioOrden"] = 0;
            ViewState["DireccionOrden"] = 0;
        }
    }
}