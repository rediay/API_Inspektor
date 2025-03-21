using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inspektor_API_REST.Models
{
    [Table("GrupoLista", Schema = "Listas")]
    public class GrupoLista
    {
        [Key]
        public int IdGrupoLista { get; set; }
        public string? NombreGrupoLista { get; set; }
        public int Prioridad { get; set; }
        public int? Orden { get; set; }
    }
}
