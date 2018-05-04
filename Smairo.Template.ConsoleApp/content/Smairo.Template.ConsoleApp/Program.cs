#region Using...
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
#endregion
namespace Smairo.Template.ConsoleApp
{
    public class Program
    {
        public static IConfiguration Configuration { get; set; }

        static async Task Main(string[] args)
        {
            // Build configuration
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            // Create application logger
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Logentries(Configuration["LogentriesTokens:ApplicationName"])
                .CreateLogger();

            // Create service container
            var services = new ServiceCollection()
                .AddSingleton<IApplicationName, ApplicationName>()
                .AddOptions()
                .AddLogging(log => log.AddSerilog(logger, true))
                .BuildServiceProvider();

            // Get application and run
            var application = services.GetService<IApplicationName>();
            await application.Run();
        }
    }
}