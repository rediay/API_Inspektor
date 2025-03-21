using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class RegistroIp
    {
        public decimal IdRegistroip { get; set; }
        public string DireccionIpIngreso { get; set; }
        public string DireccionIpRestablecer { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public DateTime? FechaRestablecer { get; set; }
        public string UserInspektor { get; set; }
        public int? IdUsuario { get; set; }
    }
}
