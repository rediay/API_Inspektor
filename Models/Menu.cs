using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class Menu
    {
        public Menu()
        {
            Submenus = new HashSet<Submenu>();
        }

        public int IdMenu { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string IdReferencia { get; set; }
        public string EnUs { get; set; }
        public string Icon { get; set; }

        public virtual ICollection<Submenu> Submenus { get; set; }
    }
}
