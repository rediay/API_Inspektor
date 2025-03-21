using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class TipoDocumento
    {
        public int IdTipoDocumento { get; set; }
        public string NombreTipoDocumento { get; set; }
        public int? IdUserCreate { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool? Estado { get; set; }

        public virtual Usuario IdUserCreateNavigation { get; set; }
    }
}
