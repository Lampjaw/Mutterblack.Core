using Microsoft.AspNetCore.Http;
using Mutterblack.Core.Clients;
using System.Threading.Tasks;

namespace Mutterblack.Core.Commands.CommandGroups
{
    [CommandGroup("weather")]
    public class WeatherCommands : CommandGroup
    {
        private readonly IWeatherClient _weatherClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WeatherCommands(IWeatherClient weatherClient, IHttpContextAccessor httpContextAccessor)
        {
            _weatherClient = weatherClient;
            _httpContextAccessor = httpContextAccessor;
        }

        [CommandGroupAction("current")]
        public async Task<CommandResult> GetCurrentWeather(string location)
        {
            var result = await _weatherClient.GetWeatherByLocation(location, _httpContextAccessor.HttpContext.RequestAborted);
            return new CommandResult(result);
        }

        [CommandGroupAction("forecast")]
        public async Task<CommandResult> GetForecastWeather(string location)
        {
            var result = await _weatherClient.GetForecastWeatherByLocation(location, _httpContextAccessor.HttpContext.RequestAborted);
            return new CommandResult(result);
        }
    }
}
