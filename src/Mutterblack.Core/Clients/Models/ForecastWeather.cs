using System.Collections.Generic;

namespace Mutterblack.Core.Clients.Models
{
    public class ForecastWeather
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public IEnumerable<WeatherDay> Forecast { get; set; }
    }
}
