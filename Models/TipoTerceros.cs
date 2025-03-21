using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inspektor_API_REST.Models
{
    [Table("TipoTercero", Schema = "Seguridad")]
    public class TipoTerceros
    {
        [Key]
        public int IdTercero { get; set; }
        public string? NombreTercero { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IdEmpresa { get; set; }
        public int IdUsuario { get; set; }

        public Boolean Estado { get; set; }

        Usuarios? User { get; set; }
        Empresas? Empresa { get; set; }
    }
}