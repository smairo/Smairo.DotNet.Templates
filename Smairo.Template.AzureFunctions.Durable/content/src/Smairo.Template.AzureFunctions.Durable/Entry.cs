using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Smairo.Template.AzureFunctions.Durable.Serialization;

namespace Smairo.Template.AzureFunctions.Durable
{
    public static class Entry
    {
        [FunctionName(nameof(Entry))]
        public static async Task<IActionResult> StartAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient orchestrator,
            ILogger log)
        {
            var request = await req
                .Content
                .ReadAsAsync<StartRequest>();

            string instanceId = await orchestrator.StartNewAsync(nameof(Orchestrator), request);
            log.LogDebug($"{instanceId} is now running...");

            return new OkObjectResult(instanceId);
        }

        [FunctionName("GetAllStatus")]
        public static async Task GetAllStatuses(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient client,
            ILogger log)
        {
            var noFilter = new OrchestrationStatusQueryCondition();
            var result = await client.ListInstancesAsync(
                noFilter,
                CancellationToken.None);
            foreach (var instance in result.DurableOrchestrationState)
            {
                log.LogInformation(JsonConvert.SerializeObject(instance));
            }
        }
    }
}
