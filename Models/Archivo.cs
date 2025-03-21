using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class Archivo
    {
        public decimal IdArchivo { get; set; }
        public string Nombre { get; set; }
        public decimal? Length { get; set; }
        public byte[] Archivo1 { get; set; }
        public string Modulo { get; set; }
        public string Extension { get; set; }
        public decimal? IdRol { get; set; }
    }
}
