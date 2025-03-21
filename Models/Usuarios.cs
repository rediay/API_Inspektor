using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inspektor_API_REST.Models
{
    [Table("Usuarios", Schema = "Seguridad")]
    public class Usuarios
    {
        [Key]
        public int IdUsuario { get; set; }
        public int IdEmpresa { get; set; }
        public int IdRol { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Usuario { get; set; }
        public Boolean Bloqueado { get; set; }        
        public string? CorreoElectronico { get; set; }
        public Boolean Terminos { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaRegistro { get; set; }

        [ForeignKey("IdRol")]
        public virtual Roles? Role { get; set; }

        [ForeignKey("IdEmpresa")]
        public virtual Empresas? Empresa { get; set; }
    }
}
