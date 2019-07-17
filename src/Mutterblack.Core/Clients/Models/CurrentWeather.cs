namespace Mutterblack.Core.Clients.Models
{
    public class CurrentWeather
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public int Temperature { get; set; }
        public string Condition { get; set; }
        public int Humidity { get; set; }
        public int WindChill { get; set; }
        public double WindSpeed { get; set; }
        public int ForecastHigh { get; set; }
        public int ForecastLow { get; set; }
        public int HeatIndex { get; set; }
    }
}
