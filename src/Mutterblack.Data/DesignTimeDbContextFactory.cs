using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Mutterblack.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MutterblackContext>
    {
        public MutterblackContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<MutterblackContext>();

            var connectionString = configuration.GetValue<string>("DBConnectionString");

            builder.UseNpgsql(connectionString, o =>
            {
                o.CommandTimeout(180);
            });

            return new MutterblackContext(builder.Options);
        }
    }
}
