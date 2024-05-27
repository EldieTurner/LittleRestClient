using LittleRestClient;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace DemoApp
{
    internal class Program
    {
        private static string city = "London";
        private static string apiKey = "44383cf7546a09fd17d8ba3ca552fdd3";
        private static string units = "imperial";
        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleLittleRestClient("http://api.openweathermap.org/data/2.5/");
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var restClient = serviceProvider.GetRequiredService<IRestClient>();

            try
            {
                var cancellationToken = new CancellationTokenSource().Token;
                var route = $"weather?q={city}&appid={apiKey}&units={units}";
                var response = await restClient.GetAsync<WeatherResponse>(route, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Weather data:");
                    Console.WriteLine($"City: {response.Data.name}");
                    Console.WriteLine($"Temperature: {response.Data.main.temp}°{(units == "metric" ? "C" : "F")}");
                    Console.WriteLine($"Weather: {response.Data.weather[0].description}");
                }
                else
                {
                    Console.WriteLine($"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            Console.ReadLine();
        }
    }
}
