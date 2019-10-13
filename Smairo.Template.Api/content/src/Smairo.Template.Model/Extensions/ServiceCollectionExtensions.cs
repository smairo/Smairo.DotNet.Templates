using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smairo.Template.Model.Repositories;
namespace Smairo.Template.Model.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModelDependencies(this IServiceCollection services, IConfiguration configuration, bool isProduction)
        {
            services.AddSingleton<IConfiguration>(_ => configuration);
            services.AddSingleton<IDapperRepository, DapperRepository>();
            services.AddScoped<IApiRepository, ApiRepository>();

            // AddDbContext
            services.AddDbContextPool<ApiContext>(opt =>
            {
                if (isProduction)
                {
                    opt.UseSqlServer(
                        configuration.GetConnectionString("DatabaseContext"),
                        sqlOptions => sqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, ApiContext.UsedSchema)
                    );
                }
                else
                {
                    opt.UseInMemoryDatabase(Guid.NewGuid().ToString());
                }
            });

            return services;
        }
    }
}