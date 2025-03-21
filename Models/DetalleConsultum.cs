using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class DetalleConsultum
    {
        public decimal IdConsulta { get; set; }
        public decimal IdDetalleConsulta { get; set; }
        public int IdUsuario { get; set; }
        public int Prioridad { get; set; }
        public decimal IdLista { get; set; }

        public virtual Consultum IdConsultaNavigation { get; set; }
    }
}
