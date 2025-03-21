namespace Inspektor_API_REST.Models
{
    public class RespuestaToken
    {
        public bool TraeResultados { get; set; }
        public bool Error { get; set; }
        public object Data { get; set; }
    }
}
