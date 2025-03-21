using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class LoginBloqueo
    {
        public decimal IdLoginBloqueo { get; set; }
        public string UsuarioLogin { get; set; }
        public decimal? Intentos { get; set; }
        public DateTime? FechaIntento { get; set; }
        public DateTime? FechaBloqueo { get; set; }
    }
}
