using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class TUsuarioFuente
    {
        public string IdFuente { get; set; }
        public string IdUsuario { get; set; }
        public bool Activo { get; set; }
    }
}
