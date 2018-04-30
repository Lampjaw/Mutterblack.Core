using System;
using System.Threading.Tasks;

namespace Mutterblack.Cache
{
    public interface ICache
    {
        Task SetAsync(string key, object value);
        Task SetAsync(string key, object value, TimeSpan expires);
        Task<T> GetAsync<T>(string key);
        Task RemoveAsync(string key);
    }
}
