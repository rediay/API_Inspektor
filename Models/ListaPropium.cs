using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class ListaPropium
    {
        public int IdListaPropia { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdEmpresa { get; set; }
        public string TipoDocumento { get; set; }
        public string DocumentoIdentidad { get; set; }
        public string NombreCompleto { get; set; }
        public string FuenteConsulta { get; set; }
        public string TipoPersona { get; set; }
        public string Alias { get; set; }
        public string Delito { get; set; }
        public string Zona { get; set; }
        public string Link { get; set; }
        public string OtraInformacion { get; set; }
        public decimal? IdTipoListaPropia { get; set; }
    }
}
