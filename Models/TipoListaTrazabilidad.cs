using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class TipoListaTrazabilidad
    {
        public decimal IdTrazabilidad { get; set; }
        public string Usuario { get; set; }
        public string Accion { get; set; }
        public string TipoListaAnt { get; set; }
        public string TipoListaNew { get; set; }
        public int? GrupoListaAnt { get; set; }
        public int? GrupoListaNew { get; set; }
        public int? PeriodicidadAnt { get; set; }
        public int? PeriodicidadNew { get; set; }
        public int? IdTipoLista { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
