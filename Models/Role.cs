using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class Role
    {
        public Role()
        {
            SubmenusRoles = new HashSet<SubmenusRole>();
            SubmenusRolesEmpresas = new HashSet<SubmenusRolesEmpresa>();
            Usuarios = new HashSet<Usuario>();
        }

        public int IdRol { get; set; }
        public string NombreRol { get; set; }

        public virtual ICollection<SubmenusRole> SubmenusRoles { get; set; }
        public virtual ICollection<SubmenusRolesEmpresa> SubmenusRolesEmpresas { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
