using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class TrazaNotificacione
    {
        public decimal IdTrazaNotificaciones { get; set; }
        public int IdCorreoEnviado { get; set; }
        public int CodigoEstadoTraza { get; set; }
        public string DestinatarioCorreo { get; set; }
        public string UsuarioCrea { get; set; }
        public DateTime FechaCrea { get; set; }
        public string UsuarioModifica { get; set; }
        public DateTime? FechaModifica { get; set; }

        public virtual EstadoTraza CodigoEstadoTrazaNavigation { get; set; }
    }
}
