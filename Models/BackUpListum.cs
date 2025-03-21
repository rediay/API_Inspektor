using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class BackUpListum
    {
        public int IdLista { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoDocumento { get; set; }
        public string DocumentoIdentidad { get; set; }
        public string NombreCompleto { get; set; }
        public int IdTipoLista { get; set; }
        public string FuenteConsulta { get; set; }
        public string TipoPersona { get; set; }
        public string Alias { get; set; }
        public string Delito { get; set; }
        public string Peps { get; set; }
        public string Zona { get; set; }
        public string Link { get; set; }
        public string Imagen { get; set; }
        public string OtraInformacion { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public int IdUsuarioActualizacion { get; set; }
        public bool Validado { get; set; }
        public int IdUsuarioRegistro { get; set; }
        public string NombreCompletoLogico { get; set; }
    }
}
