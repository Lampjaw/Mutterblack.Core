using Mutterblack.Core.Commands;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Mutterblack.Core.Clients
{
    public interface IVoidwellClient
    {
        Task<CommandResult> GetCharacterStatsByName(string characterName);
        Task<CommandResult> GetCharacterWeaponStatsByName(string characterName, string weaponName);
        Task<CommandResult> GetOutfitStatsByAlias(string outfitAlias);
    }
}
