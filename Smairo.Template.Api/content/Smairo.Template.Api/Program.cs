using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
namespace Smairo.Template.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .Run();
        }

        internal static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel(kestrelOptions =>
                {
                    kestrelOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
                    kestrelOptions.Limits.MaxConcurrentConnections = 1000;
                    kestrelOptions.Limits.MaxConcurrentUpgradedConnections = 1000;
                })
                .ConfigureAppConfiguration(SetupAppConfiguration)
                .UseIISIntegration()
                .UseSerilog(SetupSerilog)
                .UseApplicationInsights();

        internal static void SetupSerilog(WebHostBuilderContext hostingContext, LoggerConfiguration loggerConfiguration)
            => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);

        internal static void SetupAppConfiguration(WebHostBuilderContext webHostBuilderContext, IConfigurationBuilder configurationBuilder)
        {
            IHostingEnvironment env = webHostBuilderContext.HostingEnvironment;
            configurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<Startup>()
                .AddEnvironmentVariables();

            
            var configuration = configurationBuilder
                .Build();

            // Add Azure key vault if we can
            if (!string.IsNullOrEmpty(configuration["KeyVault:VaultName"]) && !string.IsNullOrEmpty(configuration["KeyVault:ClientId"]) && !string.IsNullOrEmpty(configuration["KeyVault:ClientSecret"])) {
                configurationBuilder.AddAzureKeyVault(
                    $"https://{configuration["KeyVault:VaultName"]}.vault.azure.net/",
                    configuration["KeyVault:ClientId"],
                    configuration["KeyVault:ClientSecret"]
                );
            }

            configurationBuilder
                .Build();
        }
    }
}