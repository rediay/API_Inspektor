using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class Plane
    {
        public int IdPlan { get; set; }
        public string NombrePlan { get; set; }
        public int CantidadConsultas { get; set; }
        public int PagoMensual { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IdUsuario { get; set; }
        public bool Estado { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
