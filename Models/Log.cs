using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class Log
    {
        public decimal IdLog { get; set; }
        public int IdUsuario { get; set; }
        public int IdEmpresa { get; set; }
        public DateTime FechaConsulta { get; set; }
        public int IdTipoLista { get; set; }
        public string Documento { get; set; }
        public string Nombre { get; set; }
        public string Alias { get; set; }
        public int NumeroRegistros { get; set; }
        public string Operacion { get; set; }
        public string OtrosParametrosConsulta { get; set; }
    }
}
