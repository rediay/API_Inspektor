using System;
using System.Collections.Generic;

#nullable disable

namespace Inspektor_API_REST.Models
{
    public partial class Consultum
    {
        public Consultum()
        {
            DetalleConsultaArchivos = new HashSet<DetalleConsultaArchivo>();
            DetalleConsultaPropia = new HashSet<DetalleConsultaPropium>();
        }

        public decimal IdListaConsulta { get; set; }
        public decimal IdConsulta { get; set; }
        public DateTime FechaConsulta { get; set; }
        public int IdTipoListaConsulta { get; set; }
        public string DocumentoConsulta { get; set; }
        public string NombreConsulta { get; set; }
        public string AliasConsulta { get; set; }
        public int CantCoincidencias { get; set; }
        public string TipoConsulta { get; set; }
        public int IdUsuario { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<DetalleConsultaArchivo> DetalleConsultaArchivos { get; set; }
        public virtual ICollection<DetalleConsultaPropium> DetalleConsultaPropia { get; set; }
    }
}
