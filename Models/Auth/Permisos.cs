using System.ComponentModel.DataAnnotations;

namespace Inspektor_API_REST.Models.Auth
{
    public class Permisos
    {
        [Key]
        public int IdPermiso { get; set; }
        public string Tag { get; set; }
        public string Descripcion { get; set; }
    }
}
