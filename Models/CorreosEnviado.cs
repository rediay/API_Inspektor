using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Inspektor_API_REST.Models
{
    [Keyless]
    [Table("CorreosEnviados", Schema= "Notificaciones")]
    public class CorreosEnviado
    {
        public int IdCorreoEnviado { get; set; }
        public int IdEmpresa { get; set; }
        public string Asunto { get; set; }
        public string Para { get; set; }
        public string Detalle { get; set; }
        public int IdUsuario { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaEnvio { get; set; }
        public byte? Estado { get; set; }

        [ForeignKey("IdEmpresa")]
        public virtual Empresas Empresa { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuarios Usuario { get; set; }
    }
}
