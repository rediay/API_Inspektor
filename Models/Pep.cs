using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class Pep
    {
        public int IdPep { get; set; }
        public int IdLista { get; set; }
        public string PeriodoDesde { get; set; }
        public string PeriodoHasta { get; set; }
        public int? IdEntidad { get; set; }

        public virtual Listum IdListaNavigation { get; set; }
    }
}
