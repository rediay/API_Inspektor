using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inspektor_API_REST.Models
{
    [Table("Roles", Schema = "Seguridad")]
    public class Roles
    {
        [Key]
        public int IdRol { get; set; }
        public string NombreRol { get; set; }
    }
}
