using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using OrdinarioTienda.Models;
using OrdinarioTienda.Controllers;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;



namespace OrdinarioTienda.Views
{
    public partial class Carrito : System.Web.UI.Page
    {
        private static List<Articulo> carrito = new List<Articulo>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCarrito();
            }
        }

        private void CargarCarrito()
        {
            RepeaterCarrito.DataSource = carrito;
            RepeaterCarrito.DataBind();
            lblTotal.Text = carrito.Sum(a => a.Precio * a.Cantidad_Disponible).ToString("F2");
        }

        public static void AgregarArticulo(Articulo articulo)
        {
            carrito.Add(articulo);
        }

        protected void btnEliminar_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            int idArticulo = int.Parse(e.CommandArgument.ToString());
            var articulo = carrito.FirstOrDefault(a => a.ID_Articulo == idArticulo);

            if (articulo != null)
            {
                carrito.Remove(articulo);
                CargarCarrito();
            }
        }

        protected void btnVaciar_Click(object sender, EventArgs e)
        {
            carrito.Clear();
            CargarCarrito();
        }

        protected void btnFinalizarCompra_Click(object sender, EventArgs e)
        {
            try
            {
                string htmlCuerpo = GenerarCuerpoHTMLCarrito();

                string rutaPDF = Server.MapPath("~/Archivos/ResumenCompra.pdf");
                GenerarPDF(htmlCuerpo, rutaPDF);

                string correoUsuario = Session["CorreoUsuario"]?.ToString() ?? "kuiken2211@gmail.com";
                CorreoController.EnviarCorreo(
                    correoUsuario,
                    "Resumen de tu compra",
                    "Gracias por tu compra. En el archivo se mostraran los detalles.",
                    rutaPDF
                );

                Response.Write("<script>alert('Compra finalizada, se ha enviado un resumen a tu correo.');</script>");

                carrito.Clear();
                CargarCarrito();
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error: {ex.Message}');</script>");
            }
        }

        private string GenerarCuerpoHTMLCarrito()
        {
            // Comenzamos el cuerpo del correo
            var cuerpoCorreo = "Resumen de tu compra:\n\n";
            cuerpoCorreo += "------------------------------------------------------------\n";
            cuerpoCorreo += String.Format("{0,-20} {1,-10} {2,-12} {3,-12}", "Producto", "Cantidad", "Precio", "Total");
            cuerpoCorreo += "\n------------------------------------------------------------\n";

            // Recorremos el carrito y añadimos los artículos
            foreach (var articulo in carrito)
            {
                var totalArticulo = articulo.Precio * articulo.Cantidad_Disponible;
                cuerpoCorreo += String.Format("{0,-20} {1,-10} {2,-12:F2} {3,-12:F2}",
                                              articulo.Nombre,
                                              articulo.Cantidad_Disponible,
                                              articulo.Precio,
                                              totalArticulo);
                cuerpoCorreo += "\n";
            }

            cuerpoCorreo += "------------------------------------------------------------\n";

            // Añadimos el total general
            cuerpoCorreo += $"\nTotal: {"$" + carrito.Sum(a => a.Precio * a.Cantidad_Disponible):F2}\n";

            return cuerpoCorreo;
        }

        private void GenerarPDF(string htmlCuerpo, string rutaPDF)
        {
            using (FileStream stream = new FileStream(rutaPDF, FileMode.Create))
            {
                Document pdfDoc = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                pdfDoc.Add(new Paragraph(htmlCuerpo));
                pdfDoc.Close();
            }
        }

        [System.Web.Services.WebMethod]
        public static void ActualizarArticulo(int idArticulo, int cantidad)
        {
            // Buscar el artículo en el carrito y actualizar la cantidad
            var articulo = carrito.FirstOrDefault(a => a.ID_Articulo == idArticulo);
            if (articulo != null)
            {
                articulo.Cantidad_Disponible = cantidad;
            }
        }
        protected void btnRegresarArticulos_Click(object sender, EventArgs e)
        {
            Response.Redirect("Articulos.aspx");
        }

    }
}

