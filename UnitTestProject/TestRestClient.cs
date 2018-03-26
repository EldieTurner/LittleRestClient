using LittleRestClient;

namespace UnitTestProject
{
    internal class TestRestClient : RestClient
    {
        public IRestClientConfig RestConfig => Config;
        public IHttpClient HttpClient => _httpClient;

        internal TestRestClient(IRestClientConfig config, IHttpClient httpClient) : base(config, httpClient){}
        internal TestRestClient(IRestClientConfig config) : base(config) { }
        internal TestRestClient(string baseUrl) : base(baseUrl){}
    }
}
