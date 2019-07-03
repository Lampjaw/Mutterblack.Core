using Microsoft.AspNetCore.Mvc;
using Mutterblack.Core.Data.PlatformConfigurations;
using Mutterblack.Core.Exceptions;
using Mutterblack.Core.Services;
using System;
using System.Threading.Tasks;

namespace Mutterblack.Core.Controllers
{
    [Route("configuration/{platformId}/{platformKey}")]
    public class PlatformConfigurationController : Controller
    {
        private readonly IPlatformConfigurationService _platformConfigurationService;

        public PlatformConfigurationController(IPlatformConfigurationService platformConfigurationService)
        {
            _platformConfigurationService = platformConfigurationService;
        }

        [HttpGet]
        public async Task<ActionResult> GetConfiguration(string platformId, string platformKey)
        {
            var platform = GetPlatform(platformId);
            var result = await _platformConfigurationService.GetAsync(platform, platformKey, Request.HttpContext.RequestAborted);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> UpsertConfiguration(string platformId, string platformKey, [FromBody] object configuration)
        {
            var platform = GetPlatform(platformId);
            var result = await _platformConfigurationService.UpsertAsync(platform, platformKey, configuration, Request.HttpContext.RequestAborted);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteConfiguration(string platformId, string platformKey)
        {
            var platform = GetPlatform(platformId);
            await _platformConfigurationService.RemoveAsync(platform, platformKey, Request.HttpContext.RequestAborted);
            return NoContent();
        }

        private Platforms GetPlatform(string platformId)
        {
            if (Enum.TryParse(platformId, out Platforms platform))
            {
                return platform;
            }

            throw new BadRequestException();
        }
    }
}
