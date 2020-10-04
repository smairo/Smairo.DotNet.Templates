using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Smairo.Template.AzureFunctions.Durable.Serialization;
using Smairo.Template.AzureFunctions.Durable.Services;

namespace Smairo.Template.AzureFunctions.Durable.Activities
{
    public class DoSomethingActivity
    {
        private readonly IFunction1Service _function1Service;
        public DoSomethingActivity(IFunction1Service function1Service)
        {
            _function1Service = function1Service;
        }

        [FunctionName(nameof(DoSomethingActivity))]
        public async Task<ActivityResult> DoSomethingAsync(
            [ActivityTrigger] string incomingMessage,
            ILogger log)
        {
            await Task.Delay(1000000);

            await _function1Service.RunAsync();
            return new ActivityResult
            {
                Message = $"Activity for {incomingMessage} done!",
                Success = true
            };
        }
    }
}