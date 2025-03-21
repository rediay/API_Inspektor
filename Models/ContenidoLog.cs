using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class ContenidoLog
    {
        public int Id { get; set; }
        public int IdNoticia { get; set; }
        public DateTime FechaEvento { get; set; }
        public string Autor { get; set; }
        public DateTime? FechaNoticia { get; set; }
        public string Encabezado { get; set; }
        public string Detalle { get; set; }
        public string Fuente { get; set; }
        public int IdCategoria { get; set; }
        public int IdEstado { get; set; }
        public int IdContenido { get; set; }
        public string Accion { get; set; }
    }
}
