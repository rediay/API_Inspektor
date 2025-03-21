using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class EstadoServicio
    {
        public decimal IdEstadoServicio { get; set; }
        public decimal? IdEmpresa { get; set; }
        public decimal? Estado { get; set; }
        public DateTime? FechaActivacion { get; set; }
        public DateTime? FechaDesactivacion { get; set; }
        public decimal? UsuarioRegistro { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
