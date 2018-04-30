using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
namespace Smairo.Template.Api.Utilities
{
    public static class EnforceAutenticationToEveryEndpoint
    {
        /// <summary>
        /// This makes sure that every enpoint is "decorated" with '[Authorize]'
        /// </summary>
        /// <param name="services"></param>
        public static void EnforceAuthentication(this IServiceCollection services)
        {
            AuthorizationPolicy authorizationPolicy = new AuthorizationPolicyBuilder()
                                                            .RequireAuthenticatedUser()
                                                            .Build();
            services.AddMvc(options => {
                options.Filters.Add(new AuthorizeFilter(authorizationPolicy));
            });
        }
    }
}