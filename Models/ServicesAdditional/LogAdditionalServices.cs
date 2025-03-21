using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inspektor_API_REST.Models.ServicesAdditional
{
    [Table("LogAdditionalServices", Schema = "Seguridad")]
    public class LogAdditionalServices
    {
        [Key]
        public int IdLogAdditionalService { get; set; }
        public string Data { get; set; }
        public string Datatype { get; set; }
        public string Ip { get; set; }
        public int IdServicio { get; set; }
        public int IdUsuario { get; set; }
        public int IdEmpresa { get; set; }
        public DateTime FechaConsulta { get; set; }
    }
}
