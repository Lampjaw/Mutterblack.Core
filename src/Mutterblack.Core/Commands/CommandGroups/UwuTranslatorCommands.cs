using System.Linq;
using System.Threading.Tasks;

namespace Mutterblack.Core.Commands.CommandGroups
{
    [CommandGroup("uwutranslator")]
    public class UwuTranslatorCommands : CommandGroup
    {
        private readonly char[] _replaceableChars = {'l', 'r' };

        [CommandGroupAction("translate")]
        public Task<CommandResult> TranslateText(string text)
        {
            var chars = text.ToList();
            for (var i = 0; i < chars.Count; i++)
            {
                if (_replaceableChars.Any(keyChar => char.ToLowerInvariant(chars[i]) == char.ToLowerInvariant(keyChar)))
                {
                    chars[i] = char.IsUpper(chars[i]) ? char.ToUpperInvariant('w') : char.ToLowerInvariant('w');
                }
            }

            return Task.FromResult(new CommandResult(new string(chars.ToArray())));
        }
    }
}