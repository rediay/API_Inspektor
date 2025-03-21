namespace Inspektor_API_REST.Models.ServicesAdditional
{
    public class cXsHttpResponseRama<T>
    {
        public bool HasError { get; set; } = false;

        public string Message { get; set; }

        public T Data { get; set; }
    }
}
