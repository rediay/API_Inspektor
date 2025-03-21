using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inspektor_API_REST.Models
{    
    [Table("CorreosDestino", Schema = "Notificaciones")]
    public class CorreosDestino
    {
        [Key]
        public int IdCorreosDestino { get; set; }
        public int IdEmpresa { get; set; }
        public bool Prioridad1 { get; set; }
        public string CorreosPrioridad1 { get; set; }
        public bool Prioridad2 { get; set; }
        public string CorreosPrioridad2 { get; set; }
        public bool Prioridad3 { get; set; }
        public string CorreosPrioridad3 { get; set; }
        public int IdUsuario { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaRegitro { get; set; }
        public bool Coincidencias { get; set; }
        public string CorreosCoincidencias { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaCoincidencias { get; set; }
        public int? IdUsuarioCoincidencias { get; set; }
        public bool Prioridad4 { get; set; }
        public string CorreosPrioridad4 { get; set; }
        public bool SrvAdd { get; set; }
        public string CorreosPrioridadSrvAdd { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaMonitoreoTx { get; set; }
        public int? IdUsuarioMonitoreoTx { get; set; }

        [ForeignKey("IdEmpresa")]
        public virtual Empresas Empresa { get; set; }
        [ForeignKey("IdUsuario")]
        public virtual Usuarios Usuarios { get; set; }
    }
}
