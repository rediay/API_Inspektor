using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class TipoTercero
    {
        public int IdTercero { get; set; }
        public string NombreTercero { get; set; }
        public int IdEmpresa { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IdUsuario { get; set; }
        public bool Estado { get; set; }

        public virtual Empresa IdEmpresaNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
