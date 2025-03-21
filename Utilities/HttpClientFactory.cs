using System.Net.Http;

namespace Inspektor_API_REST.Utilities
{
    public class HttpClientFactory
    {
        private readonly IHttpClientFactory _clientFactory;
        public HttpClientFactory(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public HttpClient CreateClient()
        {
            return _clientFactory.CreateClient();
        }
    }
}
