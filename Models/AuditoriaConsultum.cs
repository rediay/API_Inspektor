using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class AuditoriaConsultum
    {
        public decimal IdAuditoriaConsulta { get; set; }
        public decimal? IdConsulta { get; set; }
        public string IdentificacionConsulta { get; set; }
        public string Auditoria { get; set; }
        public DateTime FechaAuditoria { get; set; }
    }
}
