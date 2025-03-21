using System;
using System.ComponentModel.DataAnnotations;

namespace Inspektor_API_REST.Models
{
    public class DatosCantidadConsultas
    {
        [Key]
        public int IdPlan { get; set; }
        public int ConsultasContratadas { get; set; }
        public int? TotalConsultasRealizadas { get; set; }
        public DateTime? FechaEmailPorcentaje { get; set; }
    }
}
