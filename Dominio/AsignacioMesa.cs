using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class AsignacionMesa
    {
        public int NroComanda { get; set; }       // Número de comanda, identificador único de la asignación
        public Usuario Mozo { get; set; }      // Debe tener Rol = "Mozo". Validar en negocio.
        public List<Mesa> Mesas { get; set; }
        public List<Pedido> Pedidos { get; set; }
        public string Estado { get; set; }    //Cancelada/Cerrada/Abierta
        public DateTime HoraInicio { get; set; }
        public DateTime HoraTerminada { get; set; }  //Puede ser cerrada por el usuario o bien por el sistema. validar en negocio
    }
}
