#region Using...

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Smairo.DependencyContainer;

#endregion
namespace Smairo.Template.ConsoleApp
{
    public class Program
    {
        // Create container using Startup class as a module, then build it to static variable
        public static IServiceProvider Provider = new ContainerBuilder()
            .RegisterModule(new Startup())
            .Build();

        static async Task Main(string[] args)
        {
            // Get application and run
            var application = Provider.GetService<IApplicationName>();
            await application.RunAsync();
        }
    }
}