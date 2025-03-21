using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inspektor_API_REST.Models
{
    [Table("Lista", Schema = "Listas")]
    public class Listas
    {
        [Key]
        public int IdLista { get; set; }
        public int? Orden { get; set; }
        public DateTime Fecha { get; set; }
        public string? Prioridad { get; set; }
        public string? NombreTipoLista { get; set; }
        public string? NombreGrupoLista { get; set; }
        public int IdGrupoLista { get; set; }
        string? NombreEntidad { get; set; }
        public string? TipoDocumento { get; set; }
        public string? DocumentoIdentidad { get; set; }
        public string? NombreCompleto { get; set; }
        public int? IdTipoLista { get; set; }
        public string? FuenteConsulta { get; set; }
        public string? TipoPersona { get; set; }
        public string? Alias { get; set; }
        public string? Delito { get; set; }
        public  string? Peps { get; set; }
        public string? Zona { get; set; }
        string? Link { get; set; }
        string? Imagen { get; set; }
        public string? OtraInformacion { get; set; }
        Boolean Estado { get; set; }
        public DateTime FechaActualizacion { get; set; }
        Boolean Validado { get; set; }
        public string? JustificacionCambio { get; set; }

        [ForeignKey("IdTipoLista")]
        protected virtual TipoLista? TipoLista { get; set; }

    }
}
