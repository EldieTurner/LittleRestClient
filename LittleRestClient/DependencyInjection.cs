using Microsoft.Extensions.DependencyInjection;

namespace LittleRestClient;

public static class DependencyInjection
{
    public static IServiceCollection AddSingleLittleRestClient(this IServiceCollection services, string baseUrl)
    {
        services.AddHttpClient();
        services.AddTransient<IRestClient>(provider =>
        {
            var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
            return new RestClient(baseUrl, httpClientFactory);
        });
        return services;
    }

    public static IServiceCollection AddSingleLittleRestClient(this IServiceCollection services, IRestClientConfig config)
    {
        services.AddHttpClient();
        services.AddTransient<IRestClient>(provider =>
        {
            var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
            return new RestClient(config, httpClientFactory);
        });
        return services;
    }

    public static IServiceCollection AddLittleRestClientFactory(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddSingleton<IRestClientFactory, RestClientFactory>();
        return services;
    }
}
