using LittleRestClient;

namespace UnitTestProject
{
    internal class TestRestClient : RestClient
    {
        internal TestRestClient(IRestClientConfig config, IHttpClient httpClient) : base(config, httpClient)
        {
        }
    }
}
