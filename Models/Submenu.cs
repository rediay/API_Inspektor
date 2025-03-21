using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class Submenu
    {
        public Submenu()
        {
            SubmenusRoles = new HashSet<SubmenusRole>();
            SubmenusRolesEmpresas = new HashSet<SubmenusRolesEmpresa>();
        }

        public int IdSubmenu { get; set; }
        public int IdMenu { get; set; }
        public string Nombre { get; set; }
        public string Ruta { get; set; }
        public byte Blank { get; set; }
        public string EnUs { get; set; }
        public string Controller { get; set; }
        public string Method { get; set; }

        public virtual Menu IdMenuNavigation { get; set; }
        public virtual ICollection<SubmenusRole> SubmenusRoles { get; set; }
        public virtual ICollection<SubmenusRolesEmpresa> SubmenusRolesEmpresas { get; set; }
    }
}
