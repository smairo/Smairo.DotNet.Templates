using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Smairo.Template.AzureFunctions.Durable.Activities;
using Smairo.Template.AzureFunctions.Durable.Serialization;

namespace Smairo.Template.AzureFunctions.Durable
{
    public class Orchestrator
    {
        [FunctionName(nameof(Orchestrator))]
        public static async Task<string> RunOrchestratorAsync(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var payload = context.GetInput<StartRequest>();
            var result = await context.CallActivityAsync<ActivityResult>(nameof(DoSomethingActivity), payload.Something);

            //var retryOptions = new RetryOptions(firstRetryInterval: TimeSpan.FromSeconds(30), maxNumberOfAttempts: 1)
            //{
            //    Handle = exception =>
            //        exception
            //            ?.InnerException
            //            ?.Message == "Activity failed" 
            //};
            //await context.CallActivityWithRetryAsync(nameof(DoSomethingWithReplyActivity), retryOptions, result);

            return result.Message;
        }
    }
}
