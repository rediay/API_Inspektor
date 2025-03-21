using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class TipoIdentificacion
    {
        public int IdIdentificacion { get; set; }
        public string Acronimo { get; set; }
        public string Nombre { get; set; }
        public string Pais { get; set; }
        public bool Activo { get; set; }
    }
}
