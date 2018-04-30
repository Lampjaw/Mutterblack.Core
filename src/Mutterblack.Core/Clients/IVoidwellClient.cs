using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Mutterblack.Core.Clients
{
    public interface IVoidwellClient
    {
        Task<JToken> GetCharacterStatsByName(string characterName);
        Task<JToken> GetCharacterWeaponStatsByName(string characterName, string weaponName);
        Task<JToken> GetOutfitStatsByAlias(string outfitAlias);
    }
}
