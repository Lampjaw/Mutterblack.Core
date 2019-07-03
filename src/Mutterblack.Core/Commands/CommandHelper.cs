using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Mutterblack.Core.Commands
{
    public class CommandHelper
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CommandHelper> _logger;

        public CommandHelper(IServiceProvider serviceProvider, ILogger<CommandHelper> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<CommandResult> ExecuteCommand(string commandGroupName, string commandActionName, Dictionary<string, object> commandArgs = null)
        {
            var commandGroupType = GetCommandGroupType(commandGroupName);
            if (commandGroupType == null)
            {
                return new CommandResult { Success = false, Error = "Command group not found" };
            }

            var commandAction = GetCommandActionMethod(commandGroupType, commandActionName);
            if (commandAction == null)
            {
                return new CommandResult { Success = false, Error = "Command action not found" };
            }

            var commandArgValues = new List<object>();

            if (commandArgs != null)
            {
                var requiredActionArgs = commandAction.GetParameters();
                foreach(var requiredActionArg in requiredActionArgs)
                {
                    if (commandArgs.ContainsKey(requiredActionArg.Name))
                    {
                        try
                        {
                            var typeValue = Convert.ChangeType(commandArgs[requiredActionArg.Name], requiredActionArg.ParameterType);
                            commandArgValues.Add(typeValue);
                        }
                        catch(Exception)
                        {
                            return new CommandResult { Success = false, Error = $"Failed to convert {requiredActionArg.Name} to required type" };
                        }
                    }
                    else if (requiredActionArg.HasDefaultValue || requiredActionArg.IsOptional)
                    {
                        commandArgValues.Add(null);
                    }
                    else
                    {
                        return new CommandResult { Success = false, Error = $"Command missing required argument parameter: {requiredActionArg.Name}" };
                    }
                }
            }

            var commandGroupInstance = _serviceProvider.GetRequiredService(commandGroupType);

            try
            {
                var args = commandArgValues.ToArray();
                _logger.LogInformation(10000, "Executing {0} - {1}: {2}", commandGroupName, commandActionName, JToken.FromObject(args).ToString());
                return await (Task<CommandResult>)commandAction.Invoke(commandGroupInstance, args);
            }
            catch (ClientResponseException ex)
            {
                throw ex;
            }
            catch(Exception ex)
            {
                _logger.LogError(42752, ex, "Command failed");
                return new CommandResult { Success = false, Error = "Command failed" };
            }
        }

        public Type GetCommandGroupType(string commandGroupName)
        {
            var commandGroupTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(a => a.GetCustomAttribute<CommandGroupAttribute>() != null);

            foreach (var commandGroupType in commandGroupTypes)
            {
                if (commandGroupType.GetCustomAttribute<CommandGroupAttribute>().CommandGroupName == commandGroupName)
                {
                    return commandGroupType;
                }
            }

            return null;
        }

        public MethodInfo GetCommandActionMethod(Type commandGroupType, string commandActionName)
        {
            var commandActions = commandGroupType
                        .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                        .Where(a => a.GetCustomAttribute<CommandGroupActionAttribute>() != null);

            foreach (var commandAction in commandActions)
            {
                if (commandAction.GetCustomAttribute<CommandGroupActionAttribute>().ActionName == commandActionName)
                {
                    return commandAction;
                }
            }

            return null;
        }
    }
}
