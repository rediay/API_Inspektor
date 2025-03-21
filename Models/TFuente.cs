using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class TFuente
    {
        public string IdFuente { get; set; }
        public string IdLink { get; set; }
        public DateTime? FechFuente { get; set; }
        public string Zona { get; set; }
        public string Periodicidad { get; set; }
        public short? Lun { get; set; }
        public short? Mar { get; set; }
        public short? Mie { get; set; }
        public short? Jue { get; set; }
        public short? Vie { get; set; }
        public short? Sab { get; set; }
        public short? Dom { get; set; }
        public short? Activa { get; set; }
    }
}
