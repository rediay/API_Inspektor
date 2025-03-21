using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class ActualizacionLista
    {
        public decimal IdActualizacion { get; set; }
        public string Campos { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public decimal? IdUsuario { get; set; }
        public int? IdLista { get; set; }
        public bool? Validado { get; set; }
    }
}
