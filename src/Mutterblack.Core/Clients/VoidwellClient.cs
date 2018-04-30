using Mutterblack.Core.HttpAuthenticatedClient;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Mutterblack.Core.Clients
{
    public class VoidwellClient : IVoidwellClient, IDisposable
    {
        private readonly HttpClient _httpClient;

        public VoidwellClient(IAuthenticatedHttpClientFactory authenticatedHttpClientFactory)
        {
            _httpClient = authenticatedHttpClientFactory.GetHttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(60);
            _httpClient.BaseAddress = new Uri(Constants.Endpoints.VoidwellApi);
        }

        public async Task<JToken> GetCharacterStatsByName(string characterName)
        {
            var response = await _httpClient.GetAsync($"ps2/character/byname/{characterName}");
            return await response.GetContentAsync<JToken>();
        }

        public async Task<JToken> GetCharacterWeaponStatsByName(string characterName, string weaponName)
        {
            var response = await _httpClient.GetAsync($"ps2/character/byname/{characterName}/weapon/{weaponName}");
            return await response.GetContentAsync<JToken>();
        }

        public async Task<JToken> GetOutfitStatsByAlias(string outfitAlias)
        {
            var response = await _httpClient.GetAsync($"ps2/outfit/byalias/{outfitAlias}");
            return await response.GetContentAsync<JToken>();
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
