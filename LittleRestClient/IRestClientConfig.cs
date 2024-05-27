namespace LittleRestClient;

public interface IRestClientConfig
{
    string BaseUrl { get; }
    string ApiToken { get; }
    string UserAgent { get; }
    string ContentType { get; }
    string AcceptType { get; }
}