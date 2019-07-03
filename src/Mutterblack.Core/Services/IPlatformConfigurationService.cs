using Mutterblack.Core.Data.PlatformConfigurations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mutterblack.Core.Services
{
    public interface IPlatformConfigurationService
    {
        Task<IEnumerable<object>> GetAllAsync(Platforms platform, CancellationToken cancellationToken);
        Task<IEnumerable<object>> GetAllAsync(CancellationToken cancellationToken);
        Task<object> GetAsync(Platforms platform, string platformKey, CancellationToken cancellationToken);
        Task<object> UpsertAsync(Platforms platform, string platformKey, object configuration, CancellationToken cancellationToken);
        Task RemoveAsync(Platforms platform, string platformKey, CancellationToken cancellationToken);
    }
}