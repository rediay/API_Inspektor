namespace Inspektor_API_REST.Models.ServicesAdditional
{
    public class RamaJudicial
    {
        public int? idProceso { get; set; }
        public int? idConexion { get; set; }
        public string llaveProceso { get; set; }
        public string fechaProceso { get; set; }
        public string fechaUltimaActuacion { get; set; }
        public string despacho { get; set; }
        public string departamento { get; set; }
        public string sujetosProcesales { get; set; }
        public bool? esPrivado { get; set; }
        public int? cantFilas { get; set; }
    }
}
