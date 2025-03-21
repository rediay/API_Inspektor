using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class EstadoOperacion
    {
        public EstadoOperacion()
        {
            Eventos = new HashSet<Evento>();
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Evento> Eventos { get; set; }
    }
}
