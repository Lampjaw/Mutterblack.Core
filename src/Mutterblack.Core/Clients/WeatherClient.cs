using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Mutterblack.Core.Clients.Models;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Net.Http.Headers;
using System.Threading;
using System.Linq;
using Newtonsoft.Json;
using Mutterblack.Core.JsonConverters;

namespace Mutterblack.Core.Clients
{
    public class WeatherClient : IWeatherClient, IDisposable
    {
        const string WEATHER_URL = "https://weather-ydn-yql.media.yahoo.com/forecastrss";

        private readonly HttpOAuthClient _httpClient;
        private readonly MutterblackOptions _options;
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new UnderscorePropertyNamesContractResolver(),
            Converters = new JsonConverter[]
                {
                    new BooleanJsonConverter(),
                    new DateTimeJsonConverter()
                }
        };

        public WeatherClient(IOptions<MutterblackOptions> options)
        {
            _options = options.Value;

            _httpClient = new HttpOAuthClient(_options.YahooClientId, _options.YahooSecret)
            {
                BaseAddress = new Uri(WEATHER_URL)
            };
            _httpClient.DefaultRequestHeaders.Add("Yahoo-App-Id", _options.YahooAppId);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<CurrentWeather> GetWeatherByLocation(string location, CancellationToken cancellationToken)
        {
            var result = await GetWeatherAsync(location, cancellationToken);

            var currentDay = result.Forecasts.FirstOrDefault();

            return new CurrentWeather
            {
                Country = result.Location.Country,
                Region = result.Location.Region,
                City = result.Location.City,
                Condition = result.CurrentObservation.Condition.Text,
                Temperature = result.CurrentObservation.Condition.Temperature,
                Humidity = result.CurrentObservation.Atmosphere.Humidity,
                WindChill = result.CurrentObservation.Wind.Chill,
                WindSpeed = result.CurrentObservation.Wind.Speed,
                ForecastHigh = currentDay.High,
                ForecastLow = currentDay.Low,
            };
        }

        public async Task<ForecastWeather> GetForecastWeatherByLocation(string location, CancellationToken cancellationToken)
        {
            var result = await GetWeatherAsync(location, cancellationToken);

            var forecasts = result.Forecasts.Select(a => new WeatherDay { Date = a.Date.ToShortDateString(), Day = a.Day, High = a.High, Low = a.Low, Text = a.Text });

            return new ForecastWeather
            {
                Country = result.Location.Country,
                Region = result.Location.Region,
                City = result.Location.City,
                Forecast = forecasts
            };
        }

        private async Task<YahooWeatherResponse> GetWeatherAsync(string location, CancellationToken cancellationToken)
        {
            var loc = HttpUtility.UrlEncode(location, Encoding.UTF8);
            var url = $"?location={loc}&format=json";

            var locationParam = new KeyValuePair<string, string>("location", location);

            var response = await _httpClient.GetAsync(url, cancellationToken, locationParam);
            return await response.GetContentAsync<YahooWeatherResponse>(_serializerSettings);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
