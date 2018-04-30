namespace Mutterblack.Core.Clients.Models
{
    public class CurrentWeather
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public int Temperature { get; set; }
        public string Condition { get; set; }
    }
}
