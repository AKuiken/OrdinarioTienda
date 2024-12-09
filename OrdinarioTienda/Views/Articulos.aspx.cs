using System;
using System.Collections.Generic;
using OrdinarioTienda.Models;
using OrdinarioTienda.Controllers;
using System.Web.UI.WebControls;

namespace OrdinarioTienda.Views
{
    public partial class Articulos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarArticulos();
            }
        }

        private void CargarArticulos()
        {
            List<Articulo> articulos = TiendaController.ObtenerArticulos();
            RepeaterArticulos.DataSource = articulos;
            RepeaterArticulos.DataBind();
        }

        // Guardar (Agregar o Actualizar) artículo
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string marca = txtMarca.Text.Trim();
            string descripcion = txtDescripcion.Text.Trim();
            decimal precio;

            if (decimal.TryParse(txtPrecio.Text, out precio) && !string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(marca))
            {
                int articuloId = string.IsNullOrEmpty(hfArticuloId.Value) ? 0 : int.Parse(hfArticuloId.Value);

                Articulo articulo = new Articulo
                {
                    ID_Articulo = articuloId,
                    Nombre = nombre,
                    Marca = marca,
                    Precio = precio,
                    Descripcion = descripcion
                };

                bool resultado = articuloId == 0
                    ? TiendaController.GuardarArticulo(articulo)
                    : TiendaController.ActualizarArticulo(articuloId, articulo);

                lblMensaje.Text = resultado
                    ? articuloId == 0 ? "Artículo agregado correctamente." : "Artículo actualizado correctamente."
                    : "Error al guardar el artículo.";

                if (resultado)
                {
                    LimpiarCampos();
                    CargarArticulos();
                }
            }
            else
            {
                lblMensaje.Text = "Por favor, complete todos los campos y asegúrese de que el precio sea válido.";
            }
        }

        // Editar artículo
        protected void btnEditar_Command(object sender, CommandEventArgs e)
        {
            int idArticulo = int.Parse(e.CommandArgument.ToString());
            Articulo articulo = TiendaController.ObtenerArticuloPorId(idArticulo);

            if (articulo != null)
            {
                hfArticuloId.Value = articulo.ID_Articulo.ToString();
                txtNombre.Text = articulo.Nombre;
                txtMarca.Text = articulo.Marca;
                txtPrecio.Text = articulo.Precio.ToString("F2");
                txtDescripcion.Text = articulo.Descripcion;

                lblMensaje.Text = "Modo de edición activado. Realice los cambios y guarde.";
            }
            else
            {
                lblMensaje.Text = "No se pudo cargar el artículo para edición.";
            }
        }

        // Eliminar artículo
        protected void btnEliminar_Command(object sender, CommandEventArgs e)
        {
            int idArticulo = int.Parse(e.CommandArgument.ToString());
            bool resultado = TiendaController.EliminarArticulo(idArticulo);

            lblMensaje.Text = resultado
                ? "Artículo eliminado correctamente."
                : "Error al eliminar el artículo.";

            if (resultado)
            {
                CargarArticulos();
            }
        }
        private void LimpiarCampos()
        {
            hfArticuloId.Value = string.Empty;
            txtNombre.Text = string.Empty;
            txtMarca.Text = string.Empty;
            txtPrecio.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
        }

        // Agregar al carrito
        protected void btnAgregar_Command(object sender, CommandEventArgs e)
        {
            int idArticulo = int.Parse(e.CommandArgument.ToString());
            Articulo articulo = TiendaController.ObtenerArticuloPorId(idArticulo);

            if (articulo != null)
            {
                Carrito.AgregarArticulo(articulo);
                lblMensaje.Text = "Artículo agregado al carrito.";
            }
            else
            {
                lblMensaje.Text = "No se pudo agregar el artículo al carrito.";
            }
        }
        // Agregar al carrito individualmente
        protected void btnAgregarCarrito_Command(object sender, CommandEventArgs e)
        {
            int idArticulo = int.Parse(e.CommandArgument.ToString());
            Articulo articulo = TiendaController.ObtenerArticuloPorId(idArticulo);

            if (articulo != null)
            {
                Carrito.AgregarArticulo(articulo);

                lblMensaje.Text = "¡Artículo agregado al carrito!";
            }
            else
            {
                lblMensaje.Text = "No se pudo agregar el artículo al carrito.";
            }
        }

    }
}
