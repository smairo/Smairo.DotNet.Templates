using System.Threading.Tasks;
using Smairo.DependencyContainer.Azure;
using Smairo.Template.AzureFunctions.Timer.Services;

namespace Smairo.Template.AzureFunctions.Timer.Debugger
{
    /// <summary>
    /// Use this to debug application logic, services etc without running from azure function cli
    /// </summary>
    public class Program
    {
        public static FunctionDebuggerBuilder<Startup> Container =
            new FunctionDebuggerBuilder<Startup>();

        static async Task Main(string[] args)
        {
            var service = Container.GetService<IFunction1Service>();
            await service.RunAsync();
        }
    }
}
