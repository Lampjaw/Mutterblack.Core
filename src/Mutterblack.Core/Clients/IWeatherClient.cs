using Mutterblack.Core.Clients.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mutterblack.Core.Clients
{
    public interface IWeatherClient
    {
        Task<CurrentWeather> GetWeatherByLocation(string location);
        Task<ForecastWeather> GetForecastWeatherByLocation(string location);
    }
}