using System.Threading;
using System.Threading.Tasks;

namespace LittleRestClient
{
    public interface IRestClient
    {       
        /// <summary>
        /// Does a GET and deserializes the response into 
        /// a TResult object, can pass CancellationToken
        /// </summary>
        /// <typeparam name="TResult">The object type you are expecting to get back</typeparam>
        /// <param name="route">The route for the endpoint you want to hit</param>
        /// <param name="cancellationToken">The Cancellation Token in case you need to abort</param>
        /// <returns>Task<RestClientResponse<TResult>></returns>
        Task<RestClientResponse<TResult>> GetAsync<TResult>(string route, CancellationToken cancellationToken);
        
        /// <summary>
        /// Does a GET but returns the raw data as a string
        /// </summary>
        /// <param name="route">The route for the endpoint you want to hit</param>
        /// <returns>Task<string></string></returns>
        Task<string> GetStringAsync(string route);
        
        /// <summary>
        /// Does a POST, posts the body and expects a TResult response
        /// </summary>
        /// <typeparam name="TBody">The object type of the payload you a sumitting</typeparam>
        /// <typeparam name="TResult">The object type you are expecting to get back</typeparam>
        /// <param name="route">The route for the endpoint you want to hit</param>
        /// <param name="body">the payload you want to send</param>
        /// <param name="cancellationToken">The Cancellation Token in case you need to abort</param>
        /// <returns>Task<RestClientResponse<TResult>></returns>
        Task<RestClientResponse<TResult>> PostAsync<TBody, TResult>(string route, TBody body, CancellationToken cancellationToken);

        /// <summary>
        /// Does a POST, posts the body, only expects a status in response
        /// </summary>
        /// <typeparam name="TBody">The object type of the payload you a sumitting</typeparam>
        /// <param name="route">The route for the endpoint you want to hit</param>
        /// <param name="body">the payload you want to send</param>
        /// <param name="cancellationToken">The Cancellation Token in case you need to abort</param>
        /// <returns>Task<RestClientResponse></returns>
        Task<RestClientResponse> PostAsync<TBody>(string route, TBody body, CancellationToken cancellationToken);

        /// <summary>
        /// Does a PUT, puts the body and expects a TResult response
        /// </summary>
        /// <typeparam name="TBody">The object type of the payload you a sumitting</typeparam>
        /// <typeparam name="TResult">The object type you are expecting to get back</typeparam>
        /// <param name="route">The route for the endpoint you want to hit</param>
        /// <param name="body">the payload you want to send</param>
        /// <param name="cancellationToken">The Cancellation Token in case you need to abort</param>
        /// <returns>Task<RestClientResponse<TResult>></returns>
        Task<RestClientResponse<TResult>> PutAsync<TBody, TResult>(string route, TBody body, CancellationToken cancellationToken);

        /// <summary>
        /// Does a PUT, puts the body, only expects a status in response
        /// </summary>
        /// <typeparam name="TBody">The object type of the payload you a sumitting</typeparam>
        /// <param name="route">The route for the endpoint you want to hit</param>
        /// <param name="body">the payload you want to send</param>
        /// <param name="cancellationToken">The Cancellation Token in case you need to abort</param>
        /// <returns>Task<RestClientResponse></returns>
        Task<RestClientResponse> PutAsync<TBody>(string route, TBody body, CancellationToken cancellationToken);

        /// <summary>
        /// Does a PATCH, patches the body and expects a TResult response
        /// </summary>
        /// <typeparam name="TBody">The object type of the payload you a sumitting</typeparam>
        /// <typeparam name="TResult">The object type you are expecting to get back</typeparam>
        /// <param name="route">The route for the endpoint you want to hit</param>
        /// <param name="body">the payload you want to send</param>
        /// <param name="cancellationToken">The Cancellation Token in case you need to abort</param>
        /// <returns>Task<RestClientResponse<TResult>></returns>
        Task<RestClientResponse<TResult>> PatchAsync<TBody, TResult>(string route, TBody body, CancellationToken cancellationToken);

        /// <summary>
        /// Does a PATCH, patches the body, only expects a status in response
        /// </summary>
        /// <typeparam name="TBody">The object type of the payload you a sumitting</typeparam>
        /// <param name="route">The route for the endpoint you want to hit</param>
        /// <param name="body">the payload you want to send</param>
        /// <param name="cancellationToken">The Cancellation Token in case you need to abort</param>
        /// <returns>Task<RestClientResponse></returns>
        Task<RestClientResponse> PatchAsync<TBody>(string route, TBody body, CancellationToken cancellationToken);

        /// <summary>
        /// Does a DELETE and deserializes the response into 
        /// a TResult object
        /// </summary>
        /// <typeparam name="TResult">The object type you are expecting to get back</typeparam>
        /// <param name="route">The route for the endpoint you want to hit</param>
        /// <param name="cancellationToken">The Cancellation Token in case you need to abort</param>
        /// <returns>Task<RestClientResponse<TResult>></returns>
        Task<RestClientResponse<TResult>> DeleteAsync<TResult>(string route, CancellationToken cancellationToken);

        /// <summary>
        /// Does a DELETE and only expects a status in response
        /// </summary>
        /// <param name="route">The route for the endpoint you want to hit</param>
        /// <param name="cancellationToken">The Cancellation Token in case you need to abort</param>
        /// <returns>Task<RestClientResponse></returns>
        Task<RestClientResponse> DeleteAsync(string route, CancellationToken cancellationToken);

        /// <summary>
        /// Disposes
        /// </summary>
        void Dispose();
    }
}