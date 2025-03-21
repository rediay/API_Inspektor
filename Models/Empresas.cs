using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inspektor_API_REST.Models
{
    [Table("Empresas", Schema = "Seguridad")]
    public class Empresas
    {
        [Key]
        public int IdEmpresa { get; set; }
        public int IdPlan { get; set; }
        public int IdUsuario { get; set; }
        public string? NombreEmpresa { get; set; }
        public Boolean Bloqueado { get; set; }
        public Boolean RenovacionAut { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaContrato { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? DuracionMeses { get; set; }
        public Byte[]? Imagen { get; set; }
        public string? CorreoElectronico { get; set; }
    }
}
