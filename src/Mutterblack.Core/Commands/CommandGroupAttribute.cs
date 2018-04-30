using System;

namespace Mutterblack.Core.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandGroupAttribute : Attribute
    {
        public string CommandGroupName { get; private set; }

        public CommandGroupAttribute(string commandGroupName)
        {
            CommandGroupName = commandGroupName;
        }
    }
}
