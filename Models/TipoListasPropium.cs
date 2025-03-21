using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class TipoListasPropium
    {
        public decimal IdTipoListaPropia { get; set; }
        public string NombreTipoListaPropia { get; set; }
        public string DescripcionTipoLista { get; set; }
        public int? IdUsuario { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? Estado { get; set; }
    }
}
