using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class ListaBlanca
    {
        public int IdListaBlanca { get; set; }
        public int IdEmpresa { get; set; }
        public int IdLista { get; set; }

        public virtual Empresa IdEmpresaNavigation { get; set; }
        public virtual Listum IdListaNavigation { get; set; }
    }
}
