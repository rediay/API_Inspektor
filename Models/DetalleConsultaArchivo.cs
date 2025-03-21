using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class DetalleConsultaArchivo
    {
        public DetalleConsultaArchivo()
        {
            DetalleConsultaPropiaArchivos = new HashSet<DetalleConsultaPropiaArchivo>();
        }

        public decimal IdConsulta { get; set; }
        public decimal IdDetalleConsultaArchivo { get; set; }
        public string NombreConsulta { get; set; }
        public string IdentificacionConsulta { get; set; }

        public virtual Consultum IdConsultaNavigation { get; set; }
        public virtual ICollection<DetalleConsultaPropiaArchivo> DetalleConsultaPropiaArchivos { get; set; }
    }
}
