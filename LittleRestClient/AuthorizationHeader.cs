namespace LittleRestClient;

public class AuthorizationHeader
{
    public AuthorizationHeader(string Scheme, string Value)
    {
        this.Scheme = Scheme;
        this.Value = Value;
    }
    public string Scheme { get; set; }
    public string Value { get; set; }
}
