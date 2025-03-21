using Inspektor_API_REST.Models.ServicesAdditional;
using System.Collections.Generic;

namespace Inspektor_API_REST.Models
{
    public class ObjetoRespuesta
    {
        public decimal NumConsulta { get; set; }
        public int CantCoincidencias { get; set; }
        public string? Nombre { get; set; }        
        public string? NumDocumento { get; set; }
        public IEnumerable<Listas>? Listas { get; set; }
        public IEnumerable<ListasPropias>? Listas_propias { get; set; }
        public cXsHttpResponse<Procuraduria> procuraduria { get; set; }
        public cXsHttpResponse<IEnumerable<RamaJudicial>> ramaJudicial { get; set; }
        public cXsHttpResponse<IEnumerable<RamaJudicialJEMPS>> ramaJudicialJEPMS { get; set; }
        public Usuarios? Usuario { get; set; }
    }
}
