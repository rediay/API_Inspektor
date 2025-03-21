using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class TipoContenido
    {
        public TipoContenido()
        {
            Categoria = new HashSet<Categorium>();
            Contenidos = new HashSet<Contenido>();
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Categorium> Categoria { get; set; }
        public virtual ICollection<Contenido> Contenidos { get; set; }
    }
}
