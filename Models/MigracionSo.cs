using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class MigracionSo
    {
        public int IdMigracionSos { get; set; }
        public decimal? IdConsultaEmpresa { get; set; }
        public decimal? NidConsultaEmpresa { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
