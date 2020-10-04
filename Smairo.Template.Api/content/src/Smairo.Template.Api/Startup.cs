using System;
using System.IO;
using System.Reflection;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Smairo.AspNetHosting;
using Smairo.Template.Api.Filters;
using Smairo.Template.Model.Extensions;
namespace Smairo.Template.Api
{
    public class Startup : ApiStartup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
            : base(configuration, environment)
        {
        }

        public override void AddBuiltInServices(IServiceCollection services)
        {
            // Adds [Authorize] to all endpoints
            var authorizationPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            services.AddControllers(opt =>
            {
                //opt.Filters.Add(new AuthorizeFilter(authorizationPolicy));
                opt.Filters.Add(new GeneralExceptionFilter());
            });

            base.AddBuiltInServices(services);
        }

        public override void AddAndConfigureOptions(IServiceCollection services)
        {
        }

        public override void AddAuthenticationAndAuthorization(IServiceCollection services)
        {
            //services
            //    .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            //    .AddIdentityServerAuthentication(opt =>
            //    {
            //        opt.Authority = Configuration["Authentication:Authority"];
            //        opt.RequireHttpsMetadata = true;
            //    });

            //services.AddAuthorization(opt =>
            //{
            //    opt.AddPolicy("MyPolicy",
            //        policy =>
            //        {
            //            policy.RequireClaim("sub");
            //        });
            //});
        }

        public override void AddDatabase(IServiceCollection services)
        {
            services.AddModelDependencies(Configuration, Environment.IsProduction());
        }

        public override void AddOurServices(IServiceCollection services)
        {
            services.AddLogging();

        }

        public override OpenApiInfo ApiInfo => new OpenApiInfo
        {
            Version = "v1",
            Title = "Smairo.Template.Api",
            Description = "A simple example ASP.NET Core Web API",
            TermsOfService = new Uri("https://example.com/terms"),
            Contact = new OpenApiContact
            {
                Name = "Example person",
                Email = string.Empty,
                Url = new Uri("https://twitter.com/..."),
            },
            License = new OpenApiLicense
            {
                Name = "Use under LICX",
                Url = new Uri("https://example.com/license"),
            }
        };

        public override string ApiDocumentationXmlPath => Path.Combine(
            AppContext.BaseDirectory,
            $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
    }
}