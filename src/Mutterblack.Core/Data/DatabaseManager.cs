using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Mutterblack.Core.Data
{
    public class DatabaseManager
    {
        public readonly IMongoDatabase Database;

        public DatabaseManager(IOptions<MutterblackOptions> options)
        {
            var client = new MongoClient(options.Value.MongoConnectionString);
            Database = client.GetDatabase(options.Value.MongoDatabase);
        }
    }
}
