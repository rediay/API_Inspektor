using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class DetalleConsultaArchivoResultado
    {
        public decimal IdDetalleConsultaArchivo { get; set; }
        public decimal IdDetalleConsultaArchivoRes { get; set; }
        public int Prioridad { get; set; }
        public decimal IdLista { get; set; }

        public virtual DetalleConsultaArchivo IdDetalleConsultaArchivoNavigation { get; set; }
    }
}
