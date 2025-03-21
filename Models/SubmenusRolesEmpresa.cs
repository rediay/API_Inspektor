using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class SubmenusRolesEmpresa
    {
        public int IdSubmenuRoleEmpresa { get; set; }
        public int IdEmpresa { get; set; }
        public int IdSubmenu { get; set; }
        public int IdRol { get; set; }
        public bool Activo { get; set; }

        public virtual Empresa IdEmpresaNavigation { get; set; }
        public virtual Role IdRolNavigation { get; set; }
        public virtual Submenu IdSubmenuNavigation { get; set; }
    }
}
