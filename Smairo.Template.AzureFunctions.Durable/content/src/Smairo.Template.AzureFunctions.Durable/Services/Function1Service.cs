using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Smairo.Template.AzureFunctions.Durable.Services
{
    public class Function1Service : IFunction1Service
    {
        private readonly ILogger<Function1Service> _logger;
        public Function1Service(ILogger<Function1Service> logger)
        {
            _logger = logger;
        }
        public async Task RunAsync()
        {
            _logger.LogInformation("Hello from the Function1 service");
        }
    }

    public interface IFunction1Service
    {
        Task RunAsync();
    }
}