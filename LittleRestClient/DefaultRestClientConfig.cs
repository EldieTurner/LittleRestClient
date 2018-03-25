namespace LittleRestClient
{
    [ExcludeFromCoverage]
    internal class DefaultRestClientConfig : IRestClientConfig
    {
        public string BaseUrl { get; internal set; }
        public string ApiToken { get;}
        public string UserAgent => "LittleRestClient";

        public string ContentType => "application/json";

        public string AcceptType => "application/json";
    }
}
