using System.ComponentModel;

namespace Inspektor_API_REST.Models
{
    public class ObjetoConsulta
    {
        internal int IdUsuario { get; set; }
        internal int IdEmpresa { get; set; }
        public string? Nombre { get; set; }
        public int? TipoDocumento { get; set; }
        public string? Identificacion { get; set; }
        public string? CantidadPalabras { get; set; }
        [DefaultValue(false)]
        public bool? TienePrioridad_4 { get; set; } = false;
        [DefaultValue(false)]
        public bool? Procuraduria{ get; set; } = false;
        [DefaultValue(false)]
        public bool? RamaJudicial{ get; set; } = false;
        [DefaultValue(false)]
        public bool? RamaJEPMS{ get; set; } = false;

        public ObjetoConsulta()
        {
            TienePrioridad_4 = false;
            Procuraduria = false;
            RamaJudicial = false;
            RamaJEPMS = false;
        }
    }
}
