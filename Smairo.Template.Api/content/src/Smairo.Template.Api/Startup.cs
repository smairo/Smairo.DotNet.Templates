using System;
using System.IO;
using System.Reflection;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Smairo.Template.Api.Filters;
namespace Smairo.Template.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // In case you need to access http requests (eg find username etc)
            services.AddHttpContextAccessor();

            AddSwagger(services);
            AddGlobalMvcFilters(services);
            AddAuthentication(services);
        }

        private void AddAuthentication(IServiceCollection services)
        {
            if (!Configuration.GetValue<bool>("Authentication:UseAuthentication"))
            {
                return;
            }

            services
                .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(opt =>
                {
                    opt.Authority = Configuration["Authentication:Authority"];
                    opt.RequireHttpsMetadata = true;
                });

            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseRouting();

            if (Configuration.GetValue<bool>("Authentication:UseAuthentication"))
            {
                app.UseAuthorization();
            }
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddSwagger(IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
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
                });

                if (Configuration.GetValue<bool>("Authentication:UseAuthentication"))
                {
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "JWT Authorization header using the Bearer scheme."
                    }); 
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference 
                                { 
                                    Type = ReferenceType.SecurityScheme, 
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    });
                }

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.DescribeAllParametersInCamelCase();
                c.IgnoreObsoleteActions();
                c.OperationFilter<SwaggerOperationFilter>();
            });
        }

        private void AddGlobalMvcFilters(IServiceCollection services)
        {
            // Adds [Authorize] to all endpoints
            var authorizationPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            // Add json
            var producesJson = new ProducesAttribute("application/json");
            var consumesJson = new ConsumesAttribute("application/json");

            services.AddMvc(options => {
                if (Configuration.GetValue<bool>("Authentication:UseAuthentication"))
                {
                    options.Filters.Add(new AuthorizeFilter(authorizationPolicy));
                }
                options.Filters.Add(producesJson);
                options.Filters.Add(consumesJson);
                options.Filters.Add(new GeneralExceptionFilter());
            });
        }
    }
}