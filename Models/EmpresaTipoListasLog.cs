using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class EmpresaTipoListasLog
    {
        public int IdEmpresaTipoListasLog { get; set; }
        public int IdEmpresa { get; set; }
        public int IdTipoLista { get; set; }
        public bool? Consultar { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
