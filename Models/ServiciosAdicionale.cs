using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class ServiciosAdicionale
    {
        public int IdServicioAdicional { get; set; }
        public string Nombre { get; set; }
        public string IdHtml { get; set; }
        public byte Activo { get; set; }
        public string IdElementAction { get; set; }
        public string ElementValue { get; set; }
    }
}
