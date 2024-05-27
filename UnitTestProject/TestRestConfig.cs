using LittleRestClient;

internal class TestRestConfig : IRestClientConfig
{
    public string BaseUrl { get; set; } = "https://test.com/";
    public string ContentType { get; set; } = "application/json";
    public string UserAgent { get; set; } = "TestAgent";
    public string AcceptType { get; set; } = "application/json";
    public string ApiToken { get; set; }
}