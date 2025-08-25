using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Immutable;
using System.Text;
using Talabat.Core.Services;

namespace Talabat.WebAPI.Utilities
{
    public class Caching : Attribute, IAsyncActionFilter
    {
        // The caching life time
        private readonly int lifeTimeInSeconds;

        public Caching(int lifeTimeInSeconds)
        {
            this.lifeTimeInSeconds = lifeTimeInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // RequestServices is the IServiceProvider container instance
            var cachingService = context.HttpContext.RequestServices.GetService<ICachingService>();
            // A unique key by its path, query string whatever their order
            string cachingKey = GenerateCachingKey(context.HttpContext.Request);
            string response = await cachingService.GetAsync(cachingKey);
            // The result is already cached
            if(response is not null)
            {
                var executionResult = new ContentResult
                {
                    ContentType = "application/json",
                    StatusCode = 200,
                    Content = response
                };
                context.Result = executionResult;
                return;
            }

            var actionExecutedContext = await next();
            if(actionExecutedContext.Result is OkObjectResult result)
                await cachingService.SetAsync(cachingKey, result, TimeSpan.FromSeconds(lifeTimeInSeconds));
        }

        private string GenerateCachingKey(HttpRequest Request)
        {
            StringBuilder key = new StringBuilder();
            key.Append(Request.Path);
            foreach (var (k, v) in Request.Query)
                key.Append($"{k}{v}");
            return new string( key.ToString().ToLower().OrderBy(C => C).ToArray() );
        }
    }
}
