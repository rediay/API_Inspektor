using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class Consultum1
    {
        public decimal IdConsulta { get; set; }
        public int IdUsuario { get; set; }
        public int IdEmpresa { get; set; }
        public DateTime FechaConsulta { get; set; }
        public int IdEstadoConsulta { get; set; }
        public int Responsable { get; set; }
        public decimal? IdConsultaEmpresa { get; set; }
        public int? IdTipoConsulta { get; set; }
        public int? Prioridad4 { get; set; }
        public int? NoPalabras { get; set; }
        public int? TipoTercero { get; set; }
        public string JsonReport { get; set; }
        public int? IdConsultaTemp { get; set; }
        public byte? HasFile { get; set; }

        public virtual Empresa IdEmpresaNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
