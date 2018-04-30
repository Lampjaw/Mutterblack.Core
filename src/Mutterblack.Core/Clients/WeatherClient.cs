using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Microsoft.Extensions.Options;
using Mutterblack.Core.Clients.Models;

namespace Mutterblack.Core.Clients
{
    public class WeatherClient : IWeatherClient, IDisposable
    {
        private readonly HttpClient _httpClient;

        public WeatherClient(IOptions<MutterblackOptions> options)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://query.yahooapis.com");
        }

        public async Task<CurrentWeather> GetWeatherByLocation(string location)
        {
            var url = $"v1/public/yql?q=select location, item.condition from weather.forecast where woeid in (select woeid from geo.places(1) where text=\"{location}\")&format=json";
            var response = await _httpClient.GetAsync(url);
            var result = await response.GetContentAsync<JToken>();

            var channelToken = result.SelectToken("query.results.channel");
            var locationToken = channelToken.SelectToken("location");
            var conditionToken = channelToken.SelectToken("item.condition");

            return new CurrentWeather
            {
                Country = locationToken.Value<string>("country"),
                Region = locationToken.Value<string>("region"),
                City = locationToken.Value<string>("city"),
                Condition = conditionToken.Value<string>("text"),
                Temperature = conditionToken.Value<int>("temp")
            };
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
