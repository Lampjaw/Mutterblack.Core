using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Microsoft.Extensions.Options;
using Mutterblack.Core.Clients.Models;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using System.Net.Http.Headers;

namespace Mutterblack.Core.Clients
{
    public class WeatherClient : IWeatherClient, IDisposable
    {
        const string WEATHER_URL = "https://weather-ydn-yql.media.yahoo.com/forecastrss";

        private readonly HttpClient _httpClient;
        private readonly MutterblackOptions _options;

        private readonly DateTime epochUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private readonly HMACSHA1 sigHasher;

        public WeatherClient(IOptions<MutterblackOptions> options)
        {
            _options = options.Value;
            sigHasher = new HMACSHA1(new ASCIIEncoding().GetBytes(string.Format("{0}&", _options.YahooSecret)));

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(WEATHER_URL);
            _httpClient.DefaultRequestHeaders.Add("Yahoo-App-Id", _options.YahooAppId);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<CurrentWeather> GetWeatherByLocation(string location)
        {
            var loc = HttpUtility.UrlEncode(location, Encoding.UTF8);
            var url = $"?location={loc}&format=json";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", GetAuthorizationHeader(WEATHER_URL, location));

            var response = await _httpClient.GetAsync(url);
            var result = await response.GetContentAsync<JToken>();

            var locationToken = result.SelectToken("location");
            var conditionToken = result.SelectToken("current_observation.condition");

            return new CurrentWeather
            {
                Country = locationToken.Value<string>("country"),
                Region = locationToken.Value<string>("region"),
                City = locationToken.Value<string>("city"),
                Condition = conditionToken.Value<string>("text"),
                Temperature = conditionToken.Value<int>("temperature")
            };
        }

        public async Task<ForecastWeather> GetForecastWeatherByLocation(string location)
        {
            var loc = HttpUtility.UrlEncode(location, Encoding.UTF8);
            var url = $"?location={loc}&format=json";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", GetAuthorizationHeader(WEATHER_URL, location));

            var response = await _httpClient.GetAsync(url);
            var result = await response.GetContentAsync<JToken>();

            var locationToken = result.SelectToken("location");
            var forecastToken = result.SelectToken("forecasts");

            var forecasts = forecastToken.ToObject<IEnumerable<WeatherDay>>();

            return new ForecastWeather
            {
                Country = locationToken.Value<string>("country"),
                Region = locationToken.Value<string>("region"),
                City = locationToken.Value<string>("city"),
                Forecast = forecasts
            };
        }

        private string GetAuthorizationHeader(string url, string location)
        {
            var nonce = Guid.NewGuid().ToString("N");
            var timestamp = ((int)(DateTime.UtcNow - epochUtc).TotalSeconds).ToString();

            var data = new Dictionary<string, string>();

            data.Add("oauth_consumer_key", _options.YahooClientId);
            data.Add("oauth_signature_method", "HMAC-SHA1");
            data.Add("oauth_timestamp", timestamp);
            data.Add("oauth_nonce", nonce);
            data.Add("oauth_version", "1.0");
            data.Add("format", "json");
            data.Add("location", location);

            data.Add("oauth_signature", GenerateSignature(url, data));

            return GenerateOAuthHeader(data);
        }

        private string GenerateOAuthHeader(Dictionary<string, string> data)
        {
            return string.Join(
                ",",
                data
                    .Where(kvp => kvp.Key.StartsWith("oauth_"))
                    .Select(kvp => string.Format("{0}=\"{1}\"", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value)))
                    .OrderBy(s => s)
            );
        }

        private string GenerateSignature(string url, Dictionary<string, string> data)
        {
            var sigString = string.Join(
                "&",
                data
                    .Union(data)
                    .Select(kvp => string.Format("{0}={1}", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value)))
                    .OrderBy(s => s)
            );

            var fullSigData = string.Format(
                "{0}&{1}&{2}",
                "GET",
                Uri.EscapeDataString(url),
                Uri.EscapeDataString(sigString.ToString())
            );

            return Convert.ToBase64String(sigHasher.ComputeHash(new ASCIIEncoding().GetBytes(fullSigData.ToString())));
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
