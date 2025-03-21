using Inspektor_API_REST.Models;
using Inspektor_API_REST.Models.ServicesAdditional;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inspektor_API_REST.Servicios.MainQuery
{
    public interface IMainQuery
    {
        Task<DatosCantidadConsultas> ValidateQueryQuantity(string company);
        Task<cXsHttpResponse<Procuraduria>> QueryProcuraduria(string identification, int? typeDocument, HttpRequest Request, List<string> RolePermission);
        Task<cXsHttpResponse<IEnumerable<RamaJudicial>>> QueryRamaJudicial(string name, HttpRequest Request, List<string> RolePermission);
        Task<cXsHttpResponse<IEnumerable<RamaJudicialJEMPS>>> QueryRamaJudicialJEMPS(string document, HttpRequest Request, List<string> RolePermission);

    }
}
