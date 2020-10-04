using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Smairo.DependencyContainer;
using Smairo.Template.ConsoleApp.Model.Extensions;
namespace Smairo.Template.ConsoleApp
{
    public class Startup : BaseStartup
    {
        public static string GetEnvironmentAsString() =>
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.Process)
            ?? "Development";

        public override IConfiguration SetupConfiguration()
        {
            var environment = GetEnvironmentAsString();
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());

            configBuilder
                .AddJsonFile("Settings\\appsettings.json", true, true)
                .AddJsonFile($"Settings\\appsettings.{environment}.json", true, true)
                .AddJsonFile("keyvault.json", true, true);

            TryAddKeyvaultAndUserSecrets(configBuilder);

            return configBuilder
                .Build();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            // Create application logger
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            services.AddLogging(log => log.AddSerilog(logger, true));

            // Options
            services.AddOptions();
            //services.Configure<AbcOptions>(opt => { opt.X = Configuration["X"]; });
            //services.Configure<AbcOptions>(Configuration.GetSection("X"));

            // Http
            services.AddHttpClient();

            // Our services
            services.AddSingleton<IApplicationName, ApplicationName>();
            services.AddModelDependencies(Configuration);

            services.BuildServiceProvider();
        }

        private static void TryAddKeyvaultAndUserSecrets(IConfigurationBuilder configBuilder)
        {
            var userSecretAssembly = Assembly.GetExecutingAssembly();

            // Add user secrets
            try
            {
                configBuilder.AddUserSecrets(userSecretAssembly);
            }
            catch
            {
                const string error = "Exception occured when trying to add UserSecrets to app configuration. " + 
                    "If your app does not support user secrets, this was expected.";
                Console.WriteLine($"[{DateTime.UtcNow:s}] [Warning] {error}");
            }

            // Add keyvault
            var temporaryConfiguration = configBuilder
                .Build();
            AddKeyvault(temporaryConfiguration, configBuilder);

            // Add user secrets again to allow keyvault overwrite
            try
            {
                configBuilder.AddUserSecrets(userSecretAssembly);
            }
            catch
            {
                const string error = "Exception occured when trying to add UserSecrets to app configuration. " + 
                    "If your app does not support user secrets, this was expected.";
                Console.WriteLine($"[{DateTime.UtcNow:s}] [Warning] {error}");
            }
        }

        private static void AddKeyvault(IConfiguration temporaryConfiguration, IConfigurationBuilder configBuilder)
        {
            var vaultSection = temporaryConfiguration?.GetSection("AzureKeyVault");
            var vaultUrl = vaultSection?["VaultUrl"];
            var clientId = vaultSection?["ClientId"];
            var clientSecret = vaultSection?["ClientSecret"];

            if (VaultSettingsHasValues(vaultUrl, clientId, clientSecret))
            {
                configBuilder.AddAzureKeyVault(vaultUrl, clientId, clientSecret);
                Console.WriteLine($"[{DateTime.UtcNow:s}] [Information] Valid Azure KeyVault configuration. Added to configuration sources");
            }
            else
            {
                const string error = "Azure key vault could not be added. This might indicate that AzureKeyVault section " +
                    "(or AzureKeyVault:ClientId, AzureKeyVault:ClientSecret) is missing from configuration";
                Console.WriteLine($"[{DateTime.UtcNow:s}] [Warning] {error}");
            }
        }

        /// <summary>
        /// Validate required vault parameters
        /// </summary>
        /// <param name="vaultUrl"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        internal static bool VaultSettingsHasValues(string vaultUrl, string clientId, string clientSecret)
        {
            return !string.IsNullOrWhiteSpace(vaultUrl)
                && !string.IsNullOrWhiteSpace(clientId)
                && !string.IsNullOrWhiteSpace(clientSecret);
        }
    }
}