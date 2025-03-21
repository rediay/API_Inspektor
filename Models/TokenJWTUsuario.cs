using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Inspektor_API_REST.Models
{
    [Table("TokenJWT", Schema="Seguridad")]
    public partial class TokenJWTUsuario
    {
        [Key]
        public int IdToken { get; set; }
        public string TokenJWT { get; set; }
        public int IdUsuario { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaCreacion { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuarios Usuario { get; set; }
    }
}
