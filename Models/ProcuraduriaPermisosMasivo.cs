using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class ProcuraduriaPermisosMasivo
    {
        public decimal IdPermisosMasivo { get; set; }
        public decimal? IdEmpresa { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
