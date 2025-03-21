using System.ComponentModel.DataAnnotations;

namespace Inspektor_API_REST.Models
{
    public class NuevoUsuarioToken
    {
        [Required]
        public string Usuario { get; set; }
        [Required]
        public string Contrasena{ get; set; }
        [Required]
        public int IdUsuarioToken { get; set; }
    }
}
