using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mutterblack.Core.Data.PlatformConfigurations.Models
{
    public abstract class BasePlatformConfiguration
    {
        [BsonId]
        [BsonElement("Platform")]
        [BsonRepresentation(BsonType.ObjectId)]
        public abstract string Platform { get; }

        [BsonId]
        [BsonElement("Key")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Key { get; set; }
    }
}
