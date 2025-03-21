using Inspektor_API_REST.Models;
using Inspektor_API_REST.Models.ServicesAdditional;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inspektor_API_REST.Servicios.ServicesAdditional
{
    public interface IServicesAdditional
    {
        Task<cXsHttpResponse<Procuraduria>> makeProcuraduriaRequest(string identification, int? typeDocument, HttpRequest Request);
        Task<cXsHttpResponse<IEnumerable<RamaJudicial>>> makeRamaJudicialRequest(string name, HttpRequest Request);
        Task<cXsHttpResponse<IEnumerable<RamaJudicialJEMPS>>> makeRamaJudicialJEMPSRequest(string document, HttpRequest Request);
    }
}
