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
        public Task<CommandResult> GetCharacter(string characterName, string platform)
        {
            return _voidwellClient.GetCharacterStatsByName(characterName, platform);
        }

        [CommandGroupAction("character-weapon")]
        public Task<CommandResult> GetCharacterWeapon(string characterName, string weaponName, string platform)
        {
            return _voidwellClient.GetCharacterWeaponStatsByName(characterName, weaponName, platform);
        }

        [CommandGroupAction("outfit")]
        public Task<CommandResult> GetOutfit(string outfitAlias, string platform)
        {
            return _voidwellClient.GetOutfitStatsByAlias(outfitAlias, platform);
        }
    }
}
