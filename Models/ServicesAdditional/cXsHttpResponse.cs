using System.Collections.Generic;

namespace Inspektor_API_REST.Models.ServicesAdditional
{
    public class cXsHttpResponse<T>
    {
        public bool HasError { get; set; } = false;

        public string ErrorMessage { get; set; }

        public T Data { get; set; }
        public List<T> ListData { get; set; }
    }
}
