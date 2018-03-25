using System.Net;
using System.Net.Http.Headers;

namespace LittleRestClient
{
    public class RestClientResponse
    {
        public virtual HttpResponseHeaders Headers { get; internal set; }
        public virtual bool IsSuccessStatusCode { get; internal set; }
        public virtual HttpStatusCode StatusCode { get; internal set; }
        public virtual string ReasonPhrase { get; internal set; }
    }

    public class RestClientResponse<TResult> : RestClientResponse
    {
        public virtual TResult Data { get; internal set; }
    }
}
