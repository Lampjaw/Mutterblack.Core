using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Mutterblack.Core.Commands
{
    public static class CommandExtensions
    {
        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            var commandTypes = typeof(CommandGroup).GetTypeInfo().Assembly.GetTypes()
                .Where(a => typeof(CommandGroup).IsAssignableFrom(a) && a.GetTypeInfo().IsClass && !a.GetTypeInfo().IsAbstract)
                .ToList();

            commandTypes.ForEach(t => services.AddTransient(t));

            return services;
        }
    }
}
