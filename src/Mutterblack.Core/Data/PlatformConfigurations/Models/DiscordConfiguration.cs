using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Mutterblack.Core.Data.PlatformConfigurations.Models
{
    public class DiscordConfiguration : BasePlatformConfiguration
    {
        public override string Platform { get => Platforms.Discord.ToString(); }

        [BsonElement("CommandPrefix")]
        public string CommandPrefix { get; set; }

        [BsonElement("AllowedChannels")]
        public IEnumerable<string> AllowedChannels { get; set; } = new string[0];

        [BsonElement("AllowedRoles")]
        public IEnumerable<string> AllowedRoles { get; set; } = new string[0];

        [BsonElement("CommandConfigurations")]
        public Dictionary<string, DiscordCommandConfiguration> CommandConfigurations { get; set; } = new Dictionary<string, DiscordCommandConfiguration>();

        public class DiscordCommandConfiguration
        {
            [BsonElement("CommandId")]
            public string CommandId { get; set; }

            [BsonElement("Enabled")]
            public bool Enabled { get; set; } = true;

            [BsonElement("AllowedChannels")]
            public IEnumerable<string> AllowedChannels { get; set; } = new string[0];

            [BsonElement("AllowedRoles")]
            public IEnumerable<string> AllowedRoles { get; set; } = new string[0];
        }
    }
}
