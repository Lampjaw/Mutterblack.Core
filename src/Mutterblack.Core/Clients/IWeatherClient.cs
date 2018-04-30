using Mutterblack.Core.Clients.Models;
using System.Threading.Tasks;

namespace Mutterblack.Core.Clients
{
    public interface IWeatherClient
    {
        Task<CurrentWeather> GetWeatherByLocation(string location);
    }
}