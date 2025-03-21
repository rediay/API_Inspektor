using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class Categorium
    {
        public Categorium()
        {
            Contenidos = new HashSet<Contenido>();
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int IdContenido { get; set; }
        public int? Status { get; set; }
        public string Usuario { get; set; }

        public virtual TipoContenido IdContenidoNavigation { get; set; }
        public virtual ICollection<Contenido> Contenidos { get; set; }
    }
}
