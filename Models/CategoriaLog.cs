using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class CategoriaLog
    {
        public int Id { get; set; }
        public int IdCategoria { get; set; }
        public string Descripcion { get; set; }
        public int IdContenido { get; set; }
        public int? Status { get; set; }
        public string Usuario { get; set; }
        public string Accion { get; set; }
        public DateTime FechaEvento { get; set; }
    }
}
