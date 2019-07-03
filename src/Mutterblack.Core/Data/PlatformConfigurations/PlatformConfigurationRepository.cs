using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Mutterblack.Core.Data.PlatformConfigurations.Models;
using Mutterblack.Core.Exceptions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mutterblack.Core.Data.PlatformConfigurations
{
    public class PlatformConfigurationRepository : IPlatformConfigurationRepository
    {
        private IMongoCollection<object> _collection;

        public PlatformConfigurationRepository(DatabaseManager databaseManager)
        {
            _collection = databaseManager.Database.GetCollection<object>("PlatformConfigurations");

        }

        public async Task<IEnumerable<object>> GetAllAsync(Platforms platform, CancellationToken cancellationToken)
        {
            var filter = GetBsonFilter(platform);
            var cursor = await _collection.FindAsync(filter, cancellationToken: cancellationToken);
            return await cursor.ToListAsync();
        }

        public async Task<IEnumerable<object>> GetAllAsync(CancellationToken cancellationToken)
        {
            var cursor = await _collection.FindAsync(configuration => true, cancellationToken: cancellationToken);
            return await cursor.ToListAsync();
        }

        public async Task<object> GetAsync(Platforms platform, string platformKey, CancellationToken cancellationToken)
        {
            var filter = GetBsonFilter(platform, platformKey);
            var cursor = await _collection.FindAsync(filter, cancellationToken: cancellationToken);
            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<object> CreateAsync(Platforms platform, string platformKey, object configuration, CancellationToken cancellationToken)
        {
            configuration = ToPlatformConfiguration(platform, platformKey, configuration);

            await _collection.InsertOneAsync(configuration, cancellationToken: cancellationToken);

            return configuration;
        }

        public async Task<object> UpdateAsync(Platforms platform, string platformKey, object modifiedConfiguration, CancellationToken cancellationToken)
        {
            var filter = GetBsonFilter(platform, platformKey);
            await _collection.ReplaceOneAsync(filter, modifiedConfiguration, cancellationToken: cancellationToken);

            return modifiedConfiguration;
        }

        public Task RemoveAsync(Platforms platform, string platformKey, CancellationToken cancellationToken)
        {
            var filter = GetBsonFilter(platform, platformKey);
            return _collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        }

        private BsonDocument GetBsonFilter(Platforms platform, string platformKey = null)
        {
            var builder = new FilterDefinitionBuilder<BsonDocument>();
            var filter = builder.Eq("Platform", platform.ToString()) & builder.Eq("Key", platformKey);
            return filter.ToBsonDocument();
        }

        private object ToPlatformConfiguration(Platforms platform, string platformKey, object configuration)
        {
            switch(platform)
            {
                case Platforms.Discord:
                    var document = configuration.ToBsonDocument();
                    return BsonSerializer.Deserialize<DiscordConfiguration>(document);
            }

            throw new InvalidConfigurationException();
        }
    }
}
