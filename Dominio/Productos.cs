using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public enum EstadoProducto
    {
        Activo = 1,
        Inactivo = 0
    }
    public class Productos
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public string DescripcionProducto { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public EstadoProducto Activo { get; set; }

        // PROPIEDADES AGREGADAS PARA ESTADISTICAS DEL DASHBOARD
        public int CantidadVendida { get; set; }
        public int Porcentaje { get; set; }
    }
}
