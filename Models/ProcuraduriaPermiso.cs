using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class ProcuraduriaPermiso
    {
        public decimal IdPermisosServicio { get; set; }
        public decimal? IdEmpresa { get; set; }
        public decimal? IdRol { get; set; }
        public decimal? IdUsuario { get; set; }
        public decimal? Estado { get; set; }
        public DateTime? FechaActivacion { get; set; }
        public DateTime? FechaDesactivacion { get; set; }
        public decimal? UsuarioRegistro { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
