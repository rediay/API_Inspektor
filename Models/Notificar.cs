using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class Notificar
    {
        public string TipoDocumento { get; set; }
        public string DocumentoIdentidad { get; set; }
        public string NombreCompleto { get; set; }
        public short? Prioridad { get; set; }
        public bool? Estado { get; set; }
    }
}
