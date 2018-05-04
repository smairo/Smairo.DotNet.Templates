using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
namespace Smairo.Template.AzureFunctions.Timer
{
    public class Function1Application : IFunction1Application
    {
        // Make all injections your "logic" needs
        private readonly ILogger<Function1Application> _logger;
        public Function1Application(ILogger<Function1Application> logger)
        {
            _logger = logger;
        }
        public async Task RunAsync()
        {
            _logger.LogInformation("Hello from timed function!");
        }
    }

    public interface IFunction1Application
    {
        Task RunAsync();
    }
}