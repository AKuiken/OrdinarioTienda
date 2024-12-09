using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrdinarioTienda.Models
{
    public class Articulo
    {
        public int ID_Articulo { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string Marca { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad_Disponible { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha_Entrada { get; set; }
    }
}