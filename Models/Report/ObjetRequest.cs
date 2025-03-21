using System;

namespace Inspektor_API_REST.Models.Report
{
    public class ObjetRequest
    {
        public decimal idConsulta { get; set; }
        public decimal idConsultaEmpresa { get; set; }
        public string NombreConsulta { get; set; }
        public string IdentificacionConsulta { get; set; }
        public string FechaActualizacion { get; set; }
        public DateTime FechaConsulta { get; set; }
        public string? jsonReport { get; set; }
        public bool HasjsonReport { get; set; }
        public decimal idDetalleConsulta { get; set; }
        public int Responsable { get; set; }
    }
}
