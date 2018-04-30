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
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost
                    .CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                    .UseKestrel(kestrelOptions => {
                        kestrelOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
                        kestrelOptions.Limits.MaxConcurrentConnections = 1000;
                        kestrelOptions.Limits.MaxConcurrentUpgradedConnections = 1000;
                    })
                    .ConfigureAppConfiguration(SetupAppConfiguration)
                    .UseSerilog()
                    .UseApplicationInsights()
                    .Build();
        }


        private static void SetupAppConfiguration(WebHostBuilderContext webHostBuilderContext, IConfigurationBuilder configurationBuilder)
        {
            IHostingEnvironment env = webHostBuilderContext.HostingEnvironment;
            configurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            // Add user secrets in development environment
            if (env.IsDevelopment()) {
                configurationBuilder.AddUserSecrets<Startup>();
            }

            // Add Azure keyvault if we can
            var configuration = configurationBuilder.Build();
            if (!string.IsNullOrEmpty(configuration["KeyVault:VaultName"]) && !string.IsNullOrEmpty(configuration["KeyVault:ClientId"]) && !string.IsNullOrEmpty(configuration["KeyVault:ClientSecret"])) {
                configurationBuilder.AddAzureKeyVault(
                    $"https://{configuration["KeyVault:VaultName"]}.vault.azure.net/",
                    configuration["KeyVault:ClientId"],
                    configuration["KeyVault:ClientSecret"]
                );
            }
            configurationBuilder.Build();
        }
    }
}