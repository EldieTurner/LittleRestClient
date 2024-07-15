using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace LittleRestClient;

public class RestClient : IRestClient, IDisposable
{
    private const string PATCH = nameof(PATCH);
    private readonly string _contenttype;
    private readonly DefaultRestClientConfig _defaultConfig = new DefaultRestClientConfig();
    private readonly IHttpClientFactory _httpClientFactory;
    private HttpClient _httpClient;

    protected readonly IRestClientConfig Config;

    public RestClient(string baseUrl, IHttpClientFactory httpClientFactory)
    {
        if (baseUrl.IsNullOrWhiteSpace())
            throw new ArgumentException($"{nameof(baseUrl)} is required");
        if (httpClientFactory is null)
            throw new ArgumentException($"{nameof(httpClientFactory)} is required");

        _defaultConfig.BaseUrl = baseUrl;
        Config = _defaultConfig;
        _contenttype = !Config.ContentType.IsNullOrWhiteSpace() ? Config.ContentType : _defaultConfig.ContentType;
        _httpClientFactory = httpClientFactory;
        InitializeHttpClient();
    }

    public RestClient(IRestClientConfig config, IHttpClientFactory httpClientFactory)
    {
        if (config.BaseUrl.IsNullOrWhiteSpace())
            throw new ArgumentException($"{nameof(IRestClientConfig.BaseUrl)} is required");

        Config = config;
        _contenttype = !Config.ContentType.IsNullOrWhiteSpace() ? Config.ContentType : _defaultConfig.ContentType;
        _httpClientFactory = httpClientFactory;
        InitializeHttpClient();
    }

    private void InitializeHttpClient()
    {
        _httpClient = _httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri(Config.BaseUrl);
        AddHeaders();
    }

    public virtual async Task<RestClientResponse<TResult>> GetAsync<TResult>(string route, CancellationToken cancellationToken = default)
    {
        using (var response = await _httpClient.GetAsync(route, cancellationToken).ConfigureAwait(false))
        {
            return await GetRestClientResponseAsync<TResult>(response).ConfigureAwait(false);
        }
    }

    public virtual async Task<string> GetStringAsync(string route, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(route, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorContent}");
        }

        return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
    }

    public async Task<RestClientResponse<TResult>> PostAsync<TBody, TResult>(string route, TBody body, CancellationToken cancellationToken = default)
    {
        var content = CreateRequestContent(body);
        using (var response = await _httpClient.PostAsync(route, content, cancellationToken).ConfigureAwait(false))
        {
            return await GetRestClientResponseAsync<TResult>(response).ConfigureAwait(false);
        }
    }

    public virtual async Task<RestClientResponse> PostAsync<TBody>(string route, TBody body, CancellationToken cancellationToken = default)
    {
        var content = CreateRequestContent(body);
        using (var response = await _httpClient.PostAsync(route, content, cancellationToken).ConfigureAwait(false))
        {
            return GetRestClientResponse(response);
        }
    }

    public virtual async Task<RestClientResponse<TResult>> PutAsync<TBody, TResult>(string route, TBody body, CancellationToken cancellationToken = default)
    {
        var content = CreateRequestContent(body);
        using (var response = await _httpClient.PutAsync(route, content, cancellationToken).ConfigureAwait(false))
        {
            return await GetRestClientResponseAsync<TResult>(response).ConfigureAwait(false);
        }
    }

    public virtual async Task<RestClientResponse> PutAsync<TBody>(string route, TBody body, CancellationToken cancellationToken = default)
    {
        var content = CreateRequestContent(body);
        using (var response = await _httpClient.PutAsync(route, content, cancellationToken).ConfigureAwait(false))
        {
            return GetRestClientResponse(response);
        }
    }

    public virtual async Task<RestClientResponse<TResult>> PatchAsync<TBody, TResult>(string route, TBody body, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(new HttpMethod(PATCH), route)
        {
            Content = CreateRequestContent(body)
        };

        using (var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false))
        {
            return await GetRestClientResponseAsync<TResult>(response).ConfigureAwait(false);
        }
    }

    public virtual async Task<RestClientResponse> PatchAsync<TBody>(string route, TBody body, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(new HttpMethod(PATCH), route)
        {
            Content = CreateRequestContent(body)
        };

        using (var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false))
        {
            return GetRestClientResponse(response);
        }
    }

    public virtual async Task<RestClientResponse<TResult>> DeleteAsync<TResult>(string route, CancellationToken cancellationToken = default)
    {
        using (var response = await _httpClient.DeleteAsync(route, cancellationToken).ConfigureAwait(false))
        {
            return await GetRestClientResponseAsync<TResult>(response).ConfigureAwait(false);
        }
    }

    public virtual async Task<RestClientResponse> DeleteAsync(string route, CancellationToken cancellationToken = default)
    {
        using (var response = await _httpClient.DeleteAsync(route, cancellationToken).ConfigureAwait(false))
        {
            return GetRestClientResponse(response);
        }
    }

    protected void AddHeaders()
    {
        if (_httpClient.DefaultRequestHeaders == null) return;

        var userAgent = !Config.UserAgent.IsNullOrWhiteSpace() ? Config.UserAgent : _defaultConfig.UserAgent;
        var acceptType = !Config.AcceptType.IsNullOrWhiteSpace() ? Config.AcceptType : _defaultConfig.AcceptType;
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptType));
        _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", _contenttype);
        if (!string.IsNullOrWhiteSpace(Config?.AuthorizationHeader?.Scheme) && !string.IsNullOrWhiteSpace(Config?.AuthorizationHeader?.Value))
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Config.AuthorizationHeader.Scheme, Config.AuthorizationHeader.Value);
        foreach(var header in Config.CustomHeaders.EmptyIfNull())
        {
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
        }
    }

    protected virtual HttpRequestMessage GetPatchHttpRequestMessage<TBody>(string route, TBody body)
    {
        var method = new HttpMethod(PATCH);
        var content = CreateRequestContent(body);
        return new HttpRequestMessage(method, route) { Content = content };
    }

    protected virtual HttpContent CreateRequestContent<TBody>(TBody body)
        => new StringContent(SerializeObject(body), Encoding.UTF8, _contenttype);

    protected virtual string SerializeObject<TBody>(TBody body)
        => JsonSerializer.Serialize(body);

    protected virtual async Task<TResult> DeserializeObjectAsync<TResult>(HttpResponseMessage response)
    {
        var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

        // Check if the response stream is empty
        if (responseStream.Length == 0)
        {
            return default!;
        }
        return await JsonSerializer.DeserializeAsync<TResult>(responseStream).ConfigureAwait(false);
    }

    private RestClientResponse GetRestClientResponse(HttpResponseMessage response)
    {
        return new RestClientResponse
        {
            Headers = response.Headers,
            IsSuccessStatusCode = response.IsSuccessStatusCode,
            ReasonPhrase = response.ReasonPhrase,
            StatusCode = response.StatusCode
        };
    }

    private async Task<RestClientResponse<TResult>> GetRestClientResponseAsync<TResult>(HttpResponseMessage response)
    {
        var restClientResponse = new RestClientResponse<TResult>
        {
            Headers = response.Headers,
            IsSuccessStatusCode = response.IsSuccessStatusCode,
            ReasonPhrase = response.ReasonPhrase,
            StatusCode = response.StatusCode,
            Data = default!
        };

        if (response.IsSuccessStatusCode)
        {
            restClientResponse.Data = await DeserializeObjectAsync<TResult>(response).ConfigureAwait(false);
        }
        return restClientResponse;
    }

    public void Dispose() => _httpClient?.Dispose();
}
