namespace LittleRestClient
{
    public interface IRestClientFactory
    {
        IRestClient CreateClient(IRestClientConfig config);
        IRestClient CreateClient(string baseurl);
    }
}