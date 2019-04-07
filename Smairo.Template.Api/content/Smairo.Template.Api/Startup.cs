#region Using...
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Serilog;
using Smairo.Template.Api.Services;
using Smairo.Template.Api.Utilities;
using Smairo.Template.Model;
using Swashbuckle.AspNetCore.Swagger;
#endregion
namespace Smairo.Template.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            SetupDependencyInjection(services);
            SetupAuthentication(services);
            SetupSwagger(services);
        }

        private void SetupSwagger(IServiceCollection services)
        {
            if (!Configuration.GetValue<bool>("UseSwagger")) return;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Smairo.Template.Api",
                    Description = "",
                    TermsOfService = "",
                    Contact = new Contact {
                        Email = "",
                        Name = "",
                        Url = ""
                    },
                    License = new License {
                        Name = "MIT",
                        Url = "https://opensource.org/licenses/MIT"
                    }
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "apiKeyAuth", Enumerable.Empty<string>() }
                });
                c.AddSecurityDefinition("apiKeyAuth", new ApiKeyScheme
                {
                    Description = "Api Authorization",
                    Name = "Authorization",
                    Type = "apiKey",
                    In = "header"
                });
                string xmlPath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, $"{AppDomain.CurrentDomain.FriendlyName}.xml");
                c.IncludeXmlComments(xmlPath);
                c.DescribeAllEnumsAsStrings();
                c.DescribeAllParametersInCamelCase();
                c.IgnoreObsoleteActions();
                c.OperationFilter<SwaggerFilters>();
            });
        }
        private void SetupAuthentication(IServiceCollection services)
        {
            if (!Configuration.GetValue<bool>("Authentication:UseAuthentication")) return;
            services.EnforceAuthentication();
            services
                .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(o => {
                    o.Authority = Configuration["Authentication:UseAuthentication"];
                    o.RequireHttpsMetadata = Configuration.GetValue<bool>("Authentication:RequireHttpsMetadata");
                });
            services.AddAuthorization();
        }
        private void SetupDependencyInjection(IServiceCollection services)
        {
            // Services
            services.AddScoped<IValuesService, ValuesService>();

            // Repo
            services.AddModelEntityFramework(Configuration);
            services.AddRepositories();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> log)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            if (Configuration.GetValue<bool>("UseSwagger")) {
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            if (Configuration.GetValue<bool>("UseAuthentication")) {
                app.UseAuthentication();
            }

            app.UseMvc();
            log.LogInformation("Smairo.Template.Api is now configured and will start to serve requests. " +
                            $"Current environment: {env.EnvironmentName}. " +
                            $"Version: {Assembly.GetExecutingAssembly().GetName().Version}");
        }
    }
}
