using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class EmpresasServiciosAdicionale
    {
        public int IdServicioAdicional { get; set; }
        public int IdEmpresa { get; set; }
        public byte Activo { get; set; }
        public byte? AutoCheck { get; set; }

        public virtual Empresa IdEmpresaNavigation { get; set; }
        public virtual ServiciosAdicionale IdServicioAdicionalNavigation { get; set; }
    }
}
