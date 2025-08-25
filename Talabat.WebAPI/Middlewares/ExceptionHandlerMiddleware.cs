using System.Net;
using System.Text.Json;
using Talabat.WebAPI.Errors;

namespace Talabat.WebAPI.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IHostEnvironment environment, ILogger<ExceptionHandlerMiddleware> logger)
        {
            try
            {
                await next(context);
            }
            catch(Exception ex)
            {
                if (!context.Response.HasStarted)
                {
                    logger.LogError(ex, ex.Message);
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = context.GetEndpoint()?.DisplayName == "ServerError" ? 400 : (int)HttpStatusCode.BadRequest;

                    // ExceptionErrorReturn is a UDT
                    var responseObject = environment.IsDevelopment() ? new ExceptionErrorReturn((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                        : new ExceptionErrorReturn((int)HttpStatusCode.InternalServerError);

                    var responseBody = JsonSerializer.Serialize(responseObject);

                    await context.Response.WriteAsync(responseBody);
                }
                else
                    logger.LogError(ex, "The response was sent before handling the error");
            }
        }
    }
}
