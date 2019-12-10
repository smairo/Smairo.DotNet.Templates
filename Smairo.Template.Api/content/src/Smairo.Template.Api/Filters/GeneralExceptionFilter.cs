using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Exceptions;
using Serilog;

namespace Smairo.Template.Api.Filters
{
    public class GeneralExceptionFilter : ExceptionFilterAttribute
    {
        public override Task OnExceptionAsync(ExceptionContext context)
        {
            string errorMessage;
            switch (context.Exception)
            {
                //case SwaggerException swaggerException:
                //case ApiException apiException:
                case HttpRequestException httpReqE:
                case OpenApiException oApiE:
                {
                    errorMessage = $"Api exception occured: {context.Exception.Message}.";
                    context.Result = new ContentResult()
                    {
                        Content = "Could not get correct response from upstream api",
                        StatusCode = 424
                    };
                    break;
                }
                case SqlException sqlE:
                case System.Data.SqlClient.SqlException sqlE2:
                {
                    errorMessage = $"SqlException occured: {context.Exception.Message}.";
                    context.Result = new ContentResult()
                    {
                        Content = "Database error occured",
                        StatusCode = 502
                    };
                    break;
                }
                default:
                {
                    errorMessage = $"Unknown error occured: {context.Exception.Message}.";
                    context.Result = new ContentResult()
                    {
                        Content = "Unknown error occured",
                        StatusCode = 500
                    };
                    break;
                }
            }
            Log.Error(errorMessage, context.Exception);
            return Task.CompletedTask;
        }
    }
}