using LittleRestClient;

namespace DemoApp
{
    internal class RestConfig : IRestClientConfig
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string AcceptType { get; set; } = string.Empty;
        public Dictionary<string, string> CustomHeaders { get; set; } = new Dictionary<string, string>();
        public AuthorizationHeader AuthorizationHeader { get; set; } = default!;
    }
}