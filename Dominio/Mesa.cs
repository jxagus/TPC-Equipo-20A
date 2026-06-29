using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public enum EstadoMesa
    {
        Habilitada = 1,
        Inhabilitada = 0
    }
    public class Mesa
    {
        public int IdMesa { get; set; }
        public int IdUsuario { get; set; }
        public string NombreMesero { get; set; }
        public string MesaUrlImagen { get; set; }
        public EstadoMesa EstadoMesa { get; set; } /*No confundir con el estado del PEDIDO. Este es el estado de la porpia mesa, por si se inhabilita una mesa por X razon*/
    }
}
