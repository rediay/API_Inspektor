using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class EmpresaTipoLista
    {
        public int IdEmpresaTipoListas { get; set; }
        public int IdEmpresa { get; set; }
        public int IdTipoLista { get; set; }
        public bool? Consultar { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaRegistro { get; set; }

        public virtual Empresa IdEmpresaNavigation { get; set; }
        public virtual TipoListum IdTipoListaNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
