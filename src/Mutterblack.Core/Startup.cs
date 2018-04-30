using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Mutterblack.Cache;
using Mutterblack.Core.HttpAuthenticatedClient;
using System.Collections.Generic;
using Mutterblack.Data;
using Mutterblack.Core.Clients;
using Mutterblack.Core.Commands;

namespace Mutterblack.Core
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath);

            if (env.IsDevelopment())
            {
                builder.AddJsonFile("devsettings.json", true, true);
            }

            builder.AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddDataAnnotations()
                .AddJsonFormatters(options =>
                {
                    options.NullValueHandling = NullValueHandling.Ignore;
                    options.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                })
                .AddMvcOptions(options =>
                {
                    options.Filters.AddService(typeof(ClientResponseExceptionFilter));
                });

            services.AddOptions();
            services.Configure<MutterblackOptions>(Configuration);

            var mutterblackOptions = Configuration.Get<MutterblackOptions>();

            services.AddEntityFrameworkContext(Configuration);
            services.AddCache(Configuration, "Mutterblack.Core");

            services.AddAuthenticatedHttpClient(options =>
            {
                options.TokenServiceAddress = "https://auth.voidwell.com/connect/token";
                options.ClientId = "mutterblack";
                options.ClientSecret = mutterblackOptions.ClientSecret;
                options.Scopes = new List<string>
                    {
                        "voidwell-daybreakgames",
                        "voidwell-api"
                    };
                options.MessageHandlerTimeout = 60000;
            });

            services.AddCommands();

            services.AddTransient<ClientResponseExceptionFilter>();
            services.AddTransient(typeof(CommandHelper));

            services.AddSingleton<IVoidwellClient, VoidwellClient>();
            services.AddSingleton<IWeatherClient, WeatherClient>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.InitializeDatabases();

            //app.UseAuthentication();

            app.UseMvc();
        }
    }
}
