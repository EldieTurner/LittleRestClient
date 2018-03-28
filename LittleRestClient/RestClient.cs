using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LittleRestClient
{
    public class RestClient : IRestClient
    {
        private const string PATCH = nameof(PATCH);
        private readonly string _contenttype;
        private readonly DefaultRestClientConfig _defaultConfig = new DefaultRestClientConfig();

        protected readonly IRestClientConfig Config;
        protected readonly IHttpClient HttpClient;

        public RestClient(string baseUrl)
        {
            if (baseUrl.IsNullOrWhiteSpace())
                throw new ArgumentException($"{nameof(baseUrl)} is Required");

            _defaultConfig.BaseUrl = baseUrl;
            Config = _defaultConfig;
            _contenttype = Config.ContentType.WhiteSpaceIsNull() ?? _defaultConfig.ContentType;
            HttpClient = new HttpClientWrapper {BaseAddress = new Uri(Config.BaseUrl)};
            Addheaders();
        }
        public RestClient(IRestClientConfig config) : this(config, new HttpClientWrapper()){}
        
        /// <summary>
        /// This is only here for testing purposes.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="httpClient"></param>
        protected RestClient(IRestClientConfig config, IHttpClient httpClient)
        {
            if(config.BaseUrl.IsNullOrWhiteSpace())
                 throw new ArgumentException($"{nameof(IRestClientConfig.BaseUrl)} is required");
            
            HttpClient = httpClient ?? new HttpClientWrapper();
            Config = config;
            _contenttype = Config.ContentType.WhiteSpaceIsNull() ?? _defaultConfig.ContentType;
            HttpClient.BaseAddress = new Uri(Config.BaseUrl);
            Addheaders();
        }
        public virtual async Task<RestClientResponse<TResult>> GetAsync<TResult>(string route)
        {
            using (var response = HttpClient.GetAsync(route))
            {
                return await GetRestClientResponseAsync<TResult>(response).ConfigureAwait(false);
            }
        }
        public virtual async Task<RestClientResponse<TResult>> GetAsync<TResult>(string route, CancellationToken cancellationToken)
        {
            using (var response = HttpClient.GetAsync(route, cancellationToken))
            {
                return await GetRestClientResponseAsync<TResult>(response).ConfigureAwait(false);
            }
        }
        public virtual Task<string> GetStringAsync(string route)
            => HttpClient.GetStringAsync(route);
        public virtual async Task<RestClientResponse<TResult>> PostAsync<TBody, TResult>(string route, TBody body)
        {
            var content = CreateRequestContent(body);
            using (var response = HttpClient.PostAsync(route, content))
            {
                return await GetRestClientResponseAsync<TResult>(response).ConfigureAwait(false);
            }
        }
        public async Task<RestClientResponse<TResult>> PostAsync<TBody, TResult>(string route, TBody body, CancellationToken cancellationToken)
        {
            var content = CreateRequestContent(body);
            using (var response = HttpClient.PostAsync(route, content, cancellationToken))
            {
                return await GetRestClientResponseAsync<TResult>(response).ConfigureAwait(false);
            }
        }
        public virtual async Task<RestClientResponse> PostAsync<TBody>(string route, TBody body)
        {
            var content = CreateRequestContent(body);
            using (var response = await HttpClient.PostAsync(route, content).ConfigureAwait(false))
            {
                return GetRestClientResponse(response);
            }
        }
        public virtual async Task<RestClientResponse> PostAsync<TBody>(string route, TBody body, CancellationToken cancellationToken)
        {
            var content = CreateRequestContent(body);
            using (var response = await HttpClient.PostAsync(route, content, cancellationToken).ConfigureAwait(false))
            {
                return GetRestClientResponse(response);
            }
        }
        public virtual async Task<RestClientResponse<TResult>> PutAsync<TBody, TResult>(string route, TBody body)
        {
            var content = CreateRequestContent(body);
            using (var response = HttpClient.PutAsync(route, content))
            {
                return await GetRestClientResponseAsync<TResult>(response).ConfigureAwait(false);
            }
        }
        public virtual async Task<RestClientResponse<TResult>> PutAsync<TBody, TResult>(string route, TBody body, CancellationToken cancellationToken)
        {
            var content = CreateRequestContent(body);
            using (var response = HttpClient.PutAsync(route, content, cancellationToken))
            {
                return await GetRestClientResponseAsync<TResult>(response).ConfigureAwait(false);
            }
        }
        public virtual async Task<RestClientResponse> PutAsync<TBody>(string route, TBody body)
        {
            var content = CreateRequestContent(body);
            using (var response = await HttpClient.PutAsync(route, content).ConfigureAwait(false))
            {
                return GetRestClientResponse(response);
            }
        }
        public virtual async Task<RestClientResponse> PutAsync<TBody>(string route, TBody body, CancellationToken cancellationToken)
        {
            var content = CreateRequestContent(body);
            using (var response = await HttpClient.PutAsync(route, content, cancellationToken).ConfigureAwait(false))
            {
                return GetRestClientResponse(response);
            }
        }
        public virtual async Task<RestClientResponse<TResult>> PatchAsync<TBody, TResult>(string route, TBody body)
        {
            var request = GetPatchHttpRequestMessage(route, body);
            using (var response = HttpClient.SendAsync(request))
            {
                return await GetRestClientResponseAsync<TResult>(response).ConfigureAwait(false);
            }
        }
        public virtual async Task<RestClientResponse<TResult>> PatchAsync<TBody, TResult>(string route, TBody body, CancellationToken cancellationToken)
        {
            var request = GetPatchHttpRequestMessage(route, body);
            using (var response = HttpClient.SendAsync(request, cancellationToken))
            {
                return await GetRestClientResponseAsync<TResult>(response).ConfigureAwait(false);
            }
        }
        public virtual async Task<RestClientResponse> PatchAsync<TBody>(string route, TBody body)
        {
            var request = GetPatchHttpRequestMessage(route, body);
            using (var response = await HttpClient.SendAsync(request).ConfigureAwait(false))
            {
                return GetRestClientResponse(response);
            }
        }
        public virtual async Task<RestClientResponse> PatchAsync<TBody>(string route, TBody body, CancellationToken cancellationToken)
        {
            var request = GetPatchHttpRequestMessage(route, body);
            using (var response = await HttpClient.SendAsync(request, cancellationToken).ConfigureAwait(false))
            {
                return GetRestClientResponse(response);
            }
        }
        public virtual async Task<RestClientResponse<TResult>> DeleteAsync<TResult>(string route)
        {
            using (var response = HttpClient.DeleteAsync(route))
            {
                return await GetRestClientResponseAsync<TResult>(response).ConfigureAwait(false);
            }
        }
        public virtual async Task<RestClientResponse<TResult>> DeleteAsync<TResult>(string route, CancellationToken cancellationToken)
        {
            using (var response = HttpClient.DeleteAsync(route, cancellationToken))
            {
                return await GetRestClientResponseAsync<TResult>(response).ConfigureAwait(false);
            }
        }
        public virtual async Task<RestClientResponse> DeleteAsync(string route)
        {
            using (var response = await HttpClient.DeleteAsync(route).ConfigureAwait(false))
            {
                return GetRestClientResponse(response);
            }
        }
        public virtual async Task<RestClientResponse> DeleteAsync(string route, CancellationToken cancellationToken)
        {
            using (var response = await HttpClient.DeleteAsync(route, cancellationToken).ConfigureAwait(false))
            {
                return GetRestClientResponse(response);
            }
        }
        protected void Addheaders()
        {
            if (HttpClient.DefaultRequestHeaders == null) return;

            var userAgent = !Config.UserAgent.IsNullOrWhiteSpace() ? Config.UserAgent : _defaultConfig.UserAgent;
            var acceptType = !Config.AcceptType.IsNullOrWhiteSpace() ? Config.AcceptType : _defaultConfig.AcceptType;
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptType));
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", _contenttype);
            if (!Config.ApiToken.IsNullOrWhiteSpace()) HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Config.ApiToken);
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
            => JsonConvert.SerializeObject(body);
        protected virtual async Task<TResult> DeserializeObjectAsync<TResult>(HttpResponseMessage response)
            => JsonConvert.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
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
        private async Task<RestClientResponse<TResult>> GetRestClientResponseAsync<TResult>(Task<HttpResponseMessage> responseTask)
        {
            var response = await  responseTask.ConfigureAwait(false);
            var restClientResponse = new RestClientResponse<TResult>
            {
                Headers = response.Headers,
                IsSuccessStatusCode = response.IsSuccessStatusCode,
                ReasonPhrase = response.ReasonPhrase,
                StatusCode = response.StatusCode,
                Data = default(TResult)
            };
            if (response.IsSuccessStatusCode)
            {
                restClientResponse.Data = await DeserializeObjectAsync<TResult>(response).ConfigureAwait(false);
            }
            return restClientResponse;
        }
        public virtual void Dispose() => HttpClient.Dispose();
    }
}