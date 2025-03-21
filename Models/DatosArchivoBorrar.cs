using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class DatosArchivoBorrar
    {
        public int? IdDetalleConsultaArchivo { get; set; }
        public int? IdDetalleConsultaArchivoRes { get; set; }
        public int? Prioridad { get; set; }
        public int? IdLista { get; set; }
    }
}
