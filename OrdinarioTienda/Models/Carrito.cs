using System;
using System.Collections.Generic;
using System.Linq;

namespace OrdinarioTienda.Models
{
    public class Carrito
    {
        public List<Articulo> Articulos { get; set; }

        public Carrito()
        {
            Articulos = new List<Articulo>();
        }

        // Agregar un artículo al carrito
        public void AgregarArticulo(Articulo articulo)
        {
            Articulos.Add(articulo);
        }

        // Eliminar un artículo del carrito
        public void EliminarArticulo(int idArticulo)
        {
            var articuloAEliminar = Articulos.FirstOrDefault(a => a.ID_Articulo == idArticulo);
            if (articuloAEliminar != null)
            {
                Articulos.Remove(articuloAEliminar);
            }
        }

        // Propiedad para obtener el total del carrito
        public decimal Total
        {
            get
            {
                return Articulos.Sum(a => a.Precio);
            }
        }

        // Vaciar el carrito
        public void VaciarCarrito()
        {
            Articulos.Clear();
        }
    }
}
