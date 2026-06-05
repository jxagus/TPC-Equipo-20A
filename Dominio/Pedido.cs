using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public int NroMesa { get; set; }
        public int IdUsuario { get; set; } //correlacion con la asignacion de la meza
        public DateTime FechayHoraPedido { get; set; }
        public decimal PrecioTotal { get; set; } = 0;
        public int IdMetodo { get; set; }
        public int IdEstadoPedido { get; set; }
    }
}
