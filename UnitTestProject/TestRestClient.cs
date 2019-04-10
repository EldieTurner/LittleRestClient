using LittleRestClient;
using System.Linq;

namespace UnitTestProject
{
    internal class TestRestClient : RestClient
    {
        internal IRestClientConfig RestConfig => Config;
        internal IHttpClient HttpClient => base.HttpClient;

        internal TestRestClient(IRestClientConfig config, IHttpClient httpClient) : base(config, httpClient) { }
        internal TestRestClient(IRestClientConfig config) : base(config) { }
        internal TestRestClient(string baseUrl) : base(baseUrl) { }
    }
}
