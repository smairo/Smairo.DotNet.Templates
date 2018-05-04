using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
namespace Smairo.Template.AzureFunctions.Timer.Modules
{
    /// <inheritdoc />
    /// <summary>
    /// This represents the module entity for dependencies.
    /// </summary>
    public class Function1Module : Module
    {
        public static IConfiguration Configuration { get; set; }

        /// <inheritdoc />
        public override void FunctionStartup(IServiceCollection services)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Function1.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables() // Adds portal settings
                .Build();

            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Logentries(Configuration["LogentriesTokens:Function1Application"])
                .CreateLogger();

            services.AddScoped<IFunction1Application, Function1Application>(); // My app logic.
            services.AddLogging(log => log.AddSerilog(logger, true));
            //services.AddSingleton(configuration);
            //services.AddSingleton<HttpClient>();
        }
    }
}