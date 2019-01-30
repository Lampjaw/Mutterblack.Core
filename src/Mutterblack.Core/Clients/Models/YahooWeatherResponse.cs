using System;
using System.Collections.Generic;

namespace Mutterblack.Core.Clients.Models
{
    public class YahooWeatherResponse
    {
        public YahooWeatherResponseLocation Location { get; set; }
        public YahooWeatherResponseCurrentObservation CurrentObservation { get; set; }
        public IEnumerable<YahooWeatherResponseForecast> Forecasts { get; set; }

        public sealed class YahooWeatherResponseLocation
        {
            public string City { get; set; }
            public string Region { get; set; }
            public string Country { get; set; }
            public float Lat { get; set; }
            public float Long { get; set; }
            public string TimezoneId { get; set; }
        }

        public sealed class YahooWeatherResponseCurrentObservation
        {
            public YahooWeatherResponseCurrentObservationWind Wind { get; set; }
            public YahooWeatherResponseCurrentObservationAtmosphere Atmosphere { get; set; }
            public YahooWeatherResponseCurrentObservationAstronomy Astronomy { get; set; }
            public YahooWeatherResponseCurrentObservationCondition Condition { get; set; }

            public sealed class YahooWeatherResponseCurrentObservationWind
            {
                public int Chill { get; set; }
                public int Direction { get; set; }
                public double Speed { get; set; }
            }

            public sealed class YahooWeatherResponseCurrentObservationAtmosphere
            {
                public int Humidity { get; set; }
                public double Visibility { get; set; }
                public double Pressure { get; set; }
            }

            public sealed class YahooWeatherResponseCurrentObservationAstronomy
            {
                public string Sunrise { get; set; }
                public string Sunset { get; set; }
            }

            public sealed class YahooWeatherResponseCurrentObservationCondition
            {
                public string Text { get; set; }
                public int Code { get; set; }
                public int Temperature { get; set; }
            }
        }

        public sealed class YahooWeatherResponseForecast
        {
            public string Day { get; set; }
            public DateTime Date { get; set; }
            public int Low { get; set; }
            public int High { get; set; }
            public string Text { get; set; }
            public int Code { get; set; }
        }
    }
}
