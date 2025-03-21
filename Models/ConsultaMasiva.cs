using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class ConsultaMasiva
    {
        public decimal IdConsultaMasiva { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaConsulta { get; set; }
        public int IdTipoConsulta { get; set; }
    }
}
