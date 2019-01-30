using Mutterblack.Core.Clients.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Mutterblack.Core.Clients
{
    public interface IWeatherClient
    {
        Task<CurrentWeather> GetWeatherByLocation(string location, CancellationToken cancellationToken);
        Task<ForecastWeather> GetForecastWeatherByLocation(string location, CancellationToken cancellationToken);
    }
}