using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smairo.Template.Model.Repositories;

namespace Smairo.Template.Model
{
    public static class ServiceExtensions
    {
        public static void AddModelEntityFramework(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DatabaseContext")
            ));
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IDatabaseRepository, DatabaseRepository>();
        }
    }
}
