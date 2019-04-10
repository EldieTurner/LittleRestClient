using Microsoft.Extensions.DependencyInjection;

namespace LittleRestClient
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddLittleRestClient(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddHttpClient<IHttpClient, HttpClientWrapper>();
            return services;
        }
    }
}
