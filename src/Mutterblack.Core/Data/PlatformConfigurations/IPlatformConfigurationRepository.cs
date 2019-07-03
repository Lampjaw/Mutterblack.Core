using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mutterblack.Core.Data.PlatformConfigurations
{
    public interface IPlatformConfigurationRepository
    {
        Task<IEnumerable<object>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<object>> GetAllAsync(Platforms platform, CancellationToken cancellationToken);
        Task<object> GetAsync(Platforms platform, string platformKey, CancellationToken cancellationToken);
        Task<object> CreateAsync(Platforms platform, string platformKey, object configuration, CancellationToken cancellationToken);
        Task<object> UpdateAsync(Platforms platform, string platformKey, object modifiedConfiguration, CancellationToken cancellationToken);
        Task RemoveAsync(Platforms platform, string platformKey, CancellationToken cancellationToken);
    }
}