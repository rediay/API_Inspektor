using System.Net.Http;

namespace Inspektor_API_REST.Models.ServicesAdditional
{
    public class RequestAditionalServiceParams
    {
        public string url { get; set; }
        public HttpMethod method { get; set; }
        public string? token { get; set; }
        public StringContent? content { get; set; }
    }
}
