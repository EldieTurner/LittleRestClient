using LittleRestClient;

namespace UnitTestProject
{
    public class TestRestConfig : IRestClientConfig
    {
        public string BaseUrl { get; set; } = "http://example.com";

        public string ApiToken { get; }

        public string UserAgent { get; }

        public string ContentType { get; }

        public string AcceptType { get; }
    }
}