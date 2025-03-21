using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class GrupoListum
    {
        public GrupoListum()
        {
            TipoLista = new HashSet<TipoListum>();
        }

        public int IdGrupoLista { get; set; }
        public string NombreGrupoLista { get; set; }
        public int Prioridad { get; set; }
        public int? Orden { get; set; }

        public virtual ICollection<TipoListum> TipoLista { get; set; }
    }
}
