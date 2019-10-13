using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smairo.Template.ConsoleApp.Model.Repositories;
namespace Smairo.Template.ConsoleApp.Model.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModelDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<MyContext>(opt => configuration.GetConnectionString("MyContext"));
            services.AddSingleton<IApplicationNameRepository, ApplicationNameRepository>();

            return services;
        }
    }
}