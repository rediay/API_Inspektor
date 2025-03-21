using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class Contenido
    {
        public int Id { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public string Autor { get; set; }
        public DateTime? FechaNoticia { get; set; }
        public string Encabezado { get; set; }
        public string Detalle { get; set; }
        public string Fuente { get; set; }
        public int IdCategoria { get; set; }
        public int IdEstado { get; set; }
        public int IdContenido { get; set; }

        public virtual Categorium IdCategoriaNavigation { get; set; }
        public virtual TipoContenido IdContenidoNavigation { get; set; }
        public virtual Estado IdEstadoNavigation { get; set; }
    }
}
