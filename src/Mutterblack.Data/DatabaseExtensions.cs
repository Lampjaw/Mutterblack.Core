using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Reflection;
//using Mutterblack.Data.Repositories;

namespace Mutterblack.Data
{
    public static class DatabaseExtensions
    {
        private static string _migrationAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;

        public static IServiceCollection AddEntityFrameworkContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.AddSingleton(impl => impl.GetRequiredService<IOptions<DatabaseOptions>>().Value);
            services.Configure<DatabaseOptions>(configuration);

            var options = configuration.Get<DatabaseOptions>();

            services.AddEntityFrameworkNpgsql();

            services.AddDbContextPool<MutterblackContext>(builder =>
                builder.UseNpgsql(options.DBConnectionString, b => {
                    b.MigrationsAssembly(_migrationAssembly);
                }), 10);

            services.AddSingleton<IDbContextHelper, DbContextHelper>();

            return services;
        }

        public static IApplicationBuilder InitializeDatabases(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<MutterblackContext>();
                dbContext.Database.Migrate();
            }

            return app;
        }
    }
}
