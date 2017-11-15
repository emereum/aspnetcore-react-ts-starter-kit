using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace TemplateProductName.WebApi.Extensions
{
    public static class SpaRoutingApplicationBuilderExtensions
    {
        /// <summary>
        /// Rewrite all requests to /index.html except for:
        /// * Requests to /api/*
        /// * Requests to files (any URL with a "." in the path)
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
                else
                {
                    // It's an api request that has 404'd from app.UseMvc();
                    // ensure we don't cache it
                    context.Response.Headers["Cache-Control"] = "no-store,no-cache";
                    context.Response.Headers["Pragma"] = "no-cache";
                }

                await next.Invoke();
            });

            return app;
        }
    }
}
