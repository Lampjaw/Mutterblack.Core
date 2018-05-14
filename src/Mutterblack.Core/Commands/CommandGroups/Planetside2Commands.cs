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
        public Task<CommandResult> GetCharacter(string characterName)
        {
            return _voidwellClient.GetCharacterStatsByName(characterName);
        }

        [CommandGroupAction("character-weapon")]
        public Task<CommandResult> GetCharacterWeapon(string characterName, string weaponName)
        {
            return _voidwellClient.GetCharacterWeaponStatsByName(characterName, weaponName);
        }

        [CommandGroupAction("outfit")]
        public Task<CommandResult> GetOutfit(string outfitAlias)
        {
            return _voidwellClient.GetOutfitStatsByAlias(outfitAlias);
        }
    }
}
