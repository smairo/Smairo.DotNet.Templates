using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using Smairo.DependencyContainer;
using Smairo.Template.AzureFunctions.Timer.Modules;

namespace Smairo.Template.AzureFunctions.Timer
{
    public static class Function1
    {
        // Making serviceprovider static, so we can actually use singletons for the lifetime of the app (between restarts)
        public static IServiceProvider Container = new ContainerBuilder()
            .RegisterModule(new Function1Module())
            .Build();

        [FunctionName("Function1")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, TraceWriter log, ExecutionContext context)
        {
            var application = Container.GetService<IFunction1Application>();
            await application.RunAsync();
        }
    }
}
