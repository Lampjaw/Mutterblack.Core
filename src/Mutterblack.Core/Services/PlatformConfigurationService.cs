using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mutterblack.Core.Data.PlatformConfigurations;
using Mutterblack.Core.Data.PlatformConfigurations.Models;
using Mutterblack.Core.Exceptions;

namespace Mutterblack.Core.Services
{
    public class PlatformConfigurationService : IPlatformConfigurationService
    {
        private readonly IPlatformConfigurationRepository _repository;

        public PlatformConfigurationService(IPlatformConfigurationRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<object>> GetAllAsync(CancellationToken cancellationToken)
        {
            return _repository.GetAllAsync(cancellationToken);
        }

        public Task<IEnumerable<object>> GetAllAsync(Platforms platform, CancellationToken cancellationToken)
        {
            return _repository.GetAllAsync(platform, cancellationToken);
        }

        public Task<object> GetAsync(Platforms platform, string platformKey, CancellationToken cancellationToken)
        {
            return _repository.GetAsync(platform, platformKey, cancellationToken);
        }

        public Task RemoveAsync(Platforms platform, string platformKey, CancellationToken cancellationToken)
        {
            return _repository.RemoveAsync(platform, platformKey, cancellationToken);
        }

        public async Task<object> UpsertAsync(Platforms platform, string platformKey, object configuration, CancellationToken cancellationToken)
        {
            if (!(configuration is BasePlatformConfiguration baseConfig && baseConfig.Platform == platform.ToString() && baseConfig.Key == platformKey)) {
                throw new InvalidConfigurationException();
            }

            var config = await _repository.GetAsync(platform, platformKey, cancellationToken);
            if (config == null)
            {
                return await _repository.CreateAsync(platform, platformKey, configuration, cancellationToken);
            }

            return await _repository.UpdateAsync(platform, platformKey, configuration, cancellationToken);
        }
    }
}
