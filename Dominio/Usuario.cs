using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Usuario
    {
        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Contrasena { get; set; }
        public int IdRol { get; set; }
        public int IdUsuario { get; set; }

    }
}
