using System;

namespace Mutterblack.Core.Commands
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandGroupActionAttribute : Attribute
    {
        public string ActionName { get; private set; }

        public CommandGroupActionAttribute(string actionName)
        {
            ActionName = actionName;
        }
    }
}
