using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Smairo.DependencyContainer;

namespace Smairo.Template.ConsoleApp
{
    public class Startup : IModule
    {
        /// <inheritdoc />
        public void Load(IServiceCollection services)
        {
            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            // Create application logger
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Logentries(configuration["LogentriesTokens:ApplicationName"])
                .CreateLogger();

            // Create service container
            services
                .AddSingleton<IApplicationName, ApplicationName>()
                .AddOptions()
                .AddLogging(log => log.AddSerilog(logger, true))
                .BuildServiceProvider();
        }
    }
}