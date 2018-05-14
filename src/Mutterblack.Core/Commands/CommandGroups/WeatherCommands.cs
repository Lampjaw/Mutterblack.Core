using Mutterblack.Core.Clients;
using System.Threading.Tasks;

namespace Mutterblack.Core.Commands.CommandGroups
{
    [CommandGroup("weather")]
    public class WeatherCommands : CommandGroup
    {
        private readonly IWeatherClient _weatherClient;

        public WeatherCommands(IWeatherClient weatherClient)
        {
            _weatherClient = weatherClient;
        }

        [CommandGroupAction("current")]
        public async Task<CommandResult> GetCurrentWeather(string location)
        {
            var result = await _weatherClient.GetWeatherByLocation(location);
            return new CommandResult(result);
        }

        [CommandGroupAction("forecast")]
        public async Task<CommandResult> GetForecastWeather(string location)
        {
            var result = await _weatherClient.GetForecastWeatherByLocation(location);
            return new CommandResult(result);
        }
    }
}
