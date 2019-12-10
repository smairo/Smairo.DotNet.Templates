using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Smairo.DependencyContainer;
using Smairo.Template.AzureFunctions.Timer.Services;

namespace Smairo.Template.AzureFunctions.Timer.Debugger
{
    /// <summary>
    /// Use this to debug application logic, services etc without running from azure function cli
    /// </summary>
    public class Program
    {
        public static IServiceProvider Container = new ContainerBuilder()
            .RegisterModule(new Startup())
            .Build();

        static async Task Main(string[] args)
        {
            var service = Container.GetService<IFunction1Service>();
            await service.RunAsync();
        }
    }
}
