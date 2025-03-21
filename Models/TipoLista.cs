using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inspektor_API_REST.Models
{
    [Table("TipoLista", Schema = "Listas")]
    public class TipoLista
    {
        [Key]
        public int IdTipoLista { get; set; }
        public string? NombreTipoLista { get; set; }
        public string? Descripcion { get; set; }
        public string? Fuente { get; set; }
        public int? IdGrupo { get; set; }
        [ForeignKey("IdGrupo")]
        public virtual GrupoLista GrupoLista { get; set; }
        public int? IdPeriodicidad { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaRegistro { get; set; }

    }
}
