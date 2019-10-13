using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Smairo.DependencyContainer;
using Smairo.Template.AzureFunctions.Timer.Services;

namespace Smairo.Template.AzureFunctions.Timer
{
    public static class Function1
    {
        // Making serviceprovider static, so we can actually use singletons for the lifetime of the app (between restarts)
        public static IServiceProvider Container = new ContainerBuilder()
            .RegisterModule(new Startup())
            .Build();

        [FunctionName("Function1")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            log.LogTrace("Function called. Running application...");

            var service = Container.GetService<IFunction1Service>();
            await service.RunAsync();

            log.LogTrace("Function completed");
        }
    }
}