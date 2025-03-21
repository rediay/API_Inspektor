namespace Inspektor_API_REST.Models
{
    public class ProcuraduriaInhabilidades
    {
        public string siri { get; set; }
        public string modulo { get; set; }
        public string inhabilidad_legal { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
        public string? Suspension_art { get; set; }
    }
}