using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace TemplateProductName.WebApi.Extensions
{
    public static class SpaRoutingApplicationBuilderExtensions
    {
        /// <summary>
        /// Rewrite all non-api and non-file requests to /index.html.
        /// Specifically, requests starting with /api/ and requests
        /// to files (any URL with a "." in the path) are the only
        /// requests that will not be rewritten to index.html.
        /// 
        /// This should be used in conjunction with and before
        /// app.UseStaticFiles().
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSpaRouting(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                // See: https://github.com/aspnet/BasicMiddleware/blob/dev/src/Microsoft.AspNetCore.Rewrite/Internal/RewriteRule.cs
                var isSpaRequest =
                    !context.Request.Path.Value.Contains(".")
                    && (!context.Request.Path.Value?.StartsWith("/api/", StringComparison.OrdinalIgnoreCase) ?? false);

                if (isSpaRequest)
                {
                    context.Request.Path = PathString.FromUriComponent("/index.html");
                }

                await next.Invoke();
            });

            return app;
        }
    }
}
