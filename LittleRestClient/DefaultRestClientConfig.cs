
namespace LittleRestClient
{
    internal class DefaultRestClientConfig : IRestClientConfig
    {
        public string BaseUrl { get; internal set; } = string.Empty;
        public string ApiToken { get; } = string.Empty;
        public string UserAgent => "LittleRestClient";
        public string ContentType => "application/json";
        public string AcceptType => "application/json";
        public AuthorizationHeader AuthorizationHeader { get; set; } = default!;
        public Dictionary<string, string> CustomHeaders { get; set; } = new Dictionary<string,string>();
    }
}
