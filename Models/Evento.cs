using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class Evento
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public double MontoEstimado { get; set; }
        public string Identificacion { get; set; }
        public string Observaciones { get; set; }
        public string Usuario { get; set; }
        public bool Leido { get; set; }
        public int IdTipoOperacion { get; set; }
        public int IdEstadoOperacion { get; set; }

        public virtual EstadoOperacion IdEstadoOperacionNavigation { get; set; }
        public virtual TipoOperacion IdTipoOperacionNavigation { get; set; }
    }
}
