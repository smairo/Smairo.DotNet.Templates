using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace Smairo.Template.Api.Filters
{
    public class SwaggerOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Global responses for every endpoint
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized"});
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden (you are not allowed to access this resource)"});
            operation.Responses.Add("429", new OpenApiResponse { Description = "Too many requests"});
            operation.Responses.Add("500", new OpenApiResponse { Description = "Unknown server error occured"});
        }
    }
}