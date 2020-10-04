using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Smairo.DependencyContainer.Azure;
using Smairo.Template.AzureFunctions.Timer.Services;

[assembly: FunctionsStartup(typeof(Smairo.Template.AzureFunctions.Timer.Startup))]
namespace Smairo.Template.AzureFunctions.Timer
{
    /// <inheritdoc />
    /// <summary>
    /// This represents the module entity for dependencies.
    /// </summary>
    public class Startup : AzureFunctionStartup<Startup>
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // Create application logger
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            services.AddLogging(log => log.AddSerilog(logger, true));

            // Options
            services.AddOptions();
            //services.Configure<AbcOptions>(opt => { opt.X = Configuration["X"]; });
            //services.Configure<AbcOptions>(Configuration.GetSection("X"));

            // Our services
            services.AddSingleton<IFunction1Service, Function1Service>();
        }
    }
}