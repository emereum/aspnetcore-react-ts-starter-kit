using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace TemplateProductName.WebApi.Extensions
{
    public static class MinimalCachingApplicationBuilderExtensions
    {
        /// <summary>
        /// Never cache index.html or any /api/ requests
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMinimalCaching(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                var shouldNotBeCached = 
                    context.Request.Path.Value == "/index.html"
                    || (context.Request.Path.Value?.StartsWith("/api/", StringComparison.OrdinalIgnoreCase) ?? false);

                if (shouldNotBeCached)
                {
                    // See: https://stackoverflow.com/questions/49547/how-do-we-control-web-page-caching-across-all-browsers
                    // See: https://dev.to/jamesthomson/spas-have-your-cache-and-eat-it-too-iel
                    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                    context.Response.Headers["Pragma"] = "no-cache";
                    context.Response.Headers["Expires"] = "0";
                }

                await next.Invoke();
            });

            return app;
        }
    }
}
