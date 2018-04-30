using Mutterblack.Core.Clients;
using System.Threading.Tasks;

namespace Mutterblack.Core.Commands.CommandGroups
{
    [CommandGroup("planetside2")]
    public class Planetside2Commands : CommandGroup
    {
        private readonly IVoidwellClient _voidwellClient;

        public Planetside2Commands(IVoidwellClient voidwellClient)
        {
            _voidwellClient = voidwellClient;
        }

        [CommandGroupAction("character")]
        public async Task<CommandResult> GetCharacter(string characterName)
        {
            var result = await _voidwellClient.GetCharacterStatsByName(characterName);
            return new CommandResult(result);
        }

        [CommandGroupAction("character-weapon")]
        public async Task<CommandResult> GetCharacterWeapon(string characterName, string weaponName)
        {
            var result = await _voidwellClient.GetCharacterWeaponStatsByName(characterName, weaponName);
            return new CommandResult(result);
        }

        [CommandGroupAction("outfit")]
        public async Task<CommandResult> GetOutfit(string outfitAlias)
        {
            var result = await _voidwellClient.GetOutfitStatsByAlias(outfitAlias);
            return new CommandResult(result);
        }
    }
}
