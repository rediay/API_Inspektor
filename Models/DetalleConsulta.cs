using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inspektor_API_REST.Models
{
    [Table("DetalleConsulta", Schema = "Procuraduria")]
    public class DetalleConsulta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "decimal(18, 0)")]
        public decimal IdDetalleConsulta { get; set; }
        public decimal IdConsulta { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string NombreConsulta { get; set; }
        public string IdentificacionConsulta { get; set; }
        public string DocumentoCoincidencia { get; set; }
        public string NombreCoincidencia { get; set; }
        public string NombreLista { get; set; }
        public string Delito { get; set; }
        public string Peps { get; set; }
        public Boolean Validado { get; set; }
        public string TipoIdentificacion { get; set; }
        public string TipoVinculo { get; set; }
        public string AnoRenovacion { get; set; }
        public string EstadoMatricula { get; set; }
        public string Embargado { get; set; }
        public string Liquidacion { get; set; }
        public string Afiliado { get; set; }
        public string Multas { get; set; }
        public string Proponente { get; set; }
        public int ConsultaPrc { get; set; }
        //public string NombreConsulta1 { get; set; }

        [ForeignKey("IdConsulta")]
        public virtual Consultas Consulta { get; set; }
    }
}
