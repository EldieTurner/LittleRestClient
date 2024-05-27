using LittleRestClient;

namespace DemoApp
{
    internal class RestConfig : IRestClientConfig
    {
        public string BaseUrl { get; set; } = string.Empty;

        public string ApiToken { get; set; } = string.Empty;

        public string UserAgent { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public string AcceptType { get; set; } = string.Empty;
    }
}