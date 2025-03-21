using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inspektor_API_REST.Models
{
    [Table("Servicios", Schema = "Seguridad")]
    public class Servicios
    {
        [Key]
        public int IdServicio { get; set; }
        public string? NombreServicio { get; set; }
        public Boolean Estado { get; set; }
        public string? URL_SRV { get; set; }
    }
}
