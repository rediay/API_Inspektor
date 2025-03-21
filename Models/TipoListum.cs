using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class TipoListum
    {
        public TipoListum()
        {
            EmpresaTipoLista = new HashSet<EmpresaTipoLista>();
            Lista = new HashSet<Listum>();
        }

        public int IdTipoLista { get; set; }
        public string NombreTipoLista { get; set; }
        public string Descripcion { get; set; }
        public string Fuente { get; set; }
        public int? IdGrupo { get; set; }
        public int? IdPeriodicidad { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public byte? CampoFechaDesde { get; set; }
        public byte? CampoFechaHasta { get; set; }
        public byte? CampoEntidad { get; set; }

        public virtual GrupoListum IdGrupoNavigation { get; set; }
        public virtual ICollection<EmpresaTipoLista> EmpresaTipoLista { get; set; }
        public virtual ICollection<Listum> Lista { get; set; }
    }
}
