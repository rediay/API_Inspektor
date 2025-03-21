using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class EstadoTraza
    {
        public EstadoTraza()
        {
            TrazaNotificaciones = new HashSet<TrazaNotificacione>();
        }

        public decimal IdEstadoTraza { get; set; }
        public int CodigoEstadoTraza { get; set; }
        public string SmtpCodigoEstadoTraza { get; set; }
        public string NombreEstadoTraza { get; set; }
        public string DescripcionEstadoTraza { get; set; }
        public string Estado { get; set; }
        public string UsuarioCrea { get; set; }
        public DateTime FechaCrea { get; set; }
        public string UsuarioModifica { get; set; }
        public DateTime? FechaModifica { get; set; }

        public virtual ICollection<TrazaNotificacione> TrazaNotificaciones { get; set; }
    }
}
