using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Smairo.DependencyContainer;
using Smairo.Template.ConsoleApp.Model.Extensions;
namespace Smairo.Template.ConsoleApp
{
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
            services.AddSingleton<IApplicationName, ApplicationName>();
            services.AddModelDependencies(Configuration);

            services.BuildServiceProvider();
        }

        public static string GetEnvironmentAsString() =>
            Environment.GetEnvironmentVariable("CORE_ENVIRONMENT", EnvironmentVariableTarget.Process) ?? "Development";
    }
}