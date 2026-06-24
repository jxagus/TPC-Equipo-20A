using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Categorias
    {
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
        public int IdCategoriaPadre { get; set; } 
    }
}
