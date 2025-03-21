using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inspektor_API_REST.Models
{
    [Table("Consulta", Schema = "Procuraduria")]
    public class Consultas
    {
        [Key]
        public decimal IdConsulta { get; set; }
        public decimal IdConsultaEmpresa { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaConsulta { get; set; }
        public int IdEstadoConsulta { get; set; }
        public int IdEmpresa { get; set; }
        public int Responsable { get; set; }
        public int IdTipoConsulta { get; set; }
        public int? Prioridad4 { get; set; }
        public int? NoPalabras { get; set; }
        public int? TipoTercero { get; set; }
        public string? jsonReport { get; set; }
        public bool HasjsonReport { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuarios? Usuarios { get; set; }
        [ForeignKey("IdEmpresa")]
        public virtual Empresas? Empresas { get; set; }
    }
}
