namespace LittleRestClient;

public interface IRestClientConfig
{
    string BaseUrl { get; }
    AuthorizationHeader AuthorizationHeader { get; }
    string UserAgent { get; }
    string ContentType { get; }
    string AcceptType { get; }
    Dictionary<string,string> CustomHeaders { get; }
}