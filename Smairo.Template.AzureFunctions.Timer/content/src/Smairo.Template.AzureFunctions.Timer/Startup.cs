using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Smairo.DependencyContainer;
using Smairo.Template.AzureFunctions.Timer.Services;
namespace Smairo.Template.AzureFunctions.Timer
{
    /// <inheritdoc />
    /// <summary>
    /// This represents the module entity for dependencies.
    /// </summary>
    public class Startup : IModule
    {
        public IConfiguration Configuration;

        /// <inheritdoc />
        public void Load(IServiceCollection services)
        {
            var environment = GetEnvironmentAsString();
            Configuration = new ConfigurationBuilder()
                .CreateBasicConfigurations(new []
                {
                    "Settings\\appsettings.json",
                    $"Settings\\appsettings.{environment}.json"
                }, "keyvault.json");

            // Create application logger
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
            services.AddLogging(log => log.AddSerilog(logger, true));

            // Options
            services.AddOptions();
            //services.Configure<AbcOptions>(opt => { opt.X = Configuration["X"]; });
            //services.Configure<AbcOptions>(Configuration.GetSection("X"));

            // Http
            services.AddHttpClient();

            // Our services
            services.AddSingleton<IFunction1Service, Function1Service>();

            services
                .BuildServiceProvider();
        }

        public static string GetEnvironmentAsString() =>
            Environment.GetEnvironmentVariable("CORE_ENVIRONMENT", EnvironmentVariableTarget.Process) ?? "Development";
    }
}