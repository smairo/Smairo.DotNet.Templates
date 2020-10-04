using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Smairo.Template.AzureFunctions.Timer.Services;
namespace Smairo.Template.AzureFunctions.Timer
{
    public class Function1
    {
        private readonly IFunction1Service _service;
        public Function1(IFunction1Service service)
        {
            _service = service;
        }

        [FunctionName("Function1")]
        public async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            log.LogTrace("Function called. Running application...");

            await _service.RunAsync();

            log.LogTrace("Function completed");
        }
    }
}