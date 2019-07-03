using Mutterblack.Core.Commands;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Mutterblack.Core.Clients
{
    public interface IVoidwellClient
    {
        Task<CommandResult> GetCharacterStatsByName(string characterName, string platform);
        Task<CommandResult> GetCharacterWeaponStatsByName(string characterName, string weaponName, string platform);
        Task<CommandResult> GetOutfitStatsByAlias(string outfitAlias, string platform);
    }
}
