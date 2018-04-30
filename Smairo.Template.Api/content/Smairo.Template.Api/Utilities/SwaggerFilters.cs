using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace Smairo.Template.Api.Utilities
{
    /// <summary>
    /// Add to every swagger endpoints
    /// </summary>
    public class SwaggerFilters : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            // Content types
            //operation.Consumes.Add("application/json");
            operation.Produces.Add("application/json");
            
            // Responses
            //operation.Responses.Add("400", new Response { Description = "Bad request" });
            operation.Responses.Add("401", new Response { Description = "Unauthorized" });
            //operation.Responses.Add("403", new Response { Description = "Forbidden" });
            operation.Responses.Add("500", new Response { Description = "Server error" });
            //operation.Responses.Add("502", new Response { Description = "Upstream server error" });
        }
    }
}