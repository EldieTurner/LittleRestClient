namespace LittleRestClient;

public class RestClientFactory : IRestClientFactory
{
    private readonly IHttpClientFactory _httpClientFactory;
    public RestClientFactory(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public IRestClient CreateClient(string baseurl)
    {
        return new RestClient(baseurl, _httpClientFactory);
    }
    public IRestClient CreateClient(IRestClientConfig config)
    {
        return new RestClient(config, _httpClientFactory);
    }
}
