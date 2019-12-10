using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Smairo.AspNetHosting;
namespace Smairo.Template.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using var logger = HostExtensions.CreateLogger(new string[0]);
            try
            {
                CreateHostBuilder(args)
                    .Build()
                    .Run();
            }
            catch (Exception e)
            {
                logger.Fatal("Aspnet host crashed! ", e);
                Console.WriteLine(e);
            }
            finally
            {
                logger.Dispose();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            new HostBuilder()
                .CreateExtendedBuilderWithSerilog<Startup>(args)
                .ConfigureWebHostDefaults(configure => { configure.UseStartup<Startup>(); })
                .UseSerilog();
    }
}