using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
namespace Smairo.Template.ConsoleApp
{
    public class ApplicationName : IApplicationName
    {
        private readonly ILogger<ApplicationName> _logger;
        public ApplicationName(ILogger<ApplicationName> logger)
        {
            _logger = logger;
        }

        public async Task Run()
        {
            _logger.LogInformation("Hello world!");
        }
    }

    public interface IApplicationName
    {
        Task Run();
    }
}