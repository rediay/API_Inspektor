using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class Imagene
    {
        public int IdImagen { get; set; }
        public int IdLista { get; set; }
        public byte[] Imagen { get; set; }
        public DateTime FechaRegsitro { get; set; }
        public int IdUsuario { get; set; }
    }
}
