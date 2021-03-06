﻿using Mutterblack.Core.HttpAuthenticatedClient;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Mutterblack.Core.Commands;

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

        public async Task<CommandResult> GetCharacterStatsByName(string characterName, string platform)
        {
            var response = await _httpClient.GetAsync($"ps2/character/byname/{characterName}?platform={platform}");

            if (!response.IsSuccessStatusCode)
            {
                return new CommandResult
                {
                    Error = await response.Content?.ReadAsStringAsync()
                };
            }

            var result = await response.GetContentAsync<JToken>();
            return new CommandResult(result);
        }

        public async Task<CommandResult> GetCharacterWeaponStatsByName(string characterName, string weaponName, string platform)
        {
            var response = await _httpClient.GetAsync($"ps2/character/byname/{characterName}/weapon/{weaponName}?platform={platform}");

            if (!response.IsSuccessStatusCode)
            {
                return new CommandResult
                {
                    Error = await response.Content?.ReadAsStringAsync()
                };
            }

            var result = await response.GetContentAsync<JToken>();
            return new CommandResult(result);
        }

        public async Task<CommandResult> GetOutfitStatsByAlias(string outfitAlias, string platform)
        {
            var response = await _httpClient.GetAsync($"ps2/outfit/byalias/{outfitAlias}?platform={platform}");

            if (!response.IsSuccessStatusCode)
            {
                return new CommandResult
                {
                    Error = await response.Content?.ReadAsStringAsync()
                };
            }

            var result = await response.GetContentAsync<JToken>();
            return new CommandResult(result);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
