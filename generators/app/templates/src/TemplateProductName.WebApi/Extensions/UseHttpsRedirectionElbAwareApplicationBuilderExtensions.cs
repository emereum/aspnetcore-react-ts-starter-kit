using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Primitives;

namespace TemplateProductName.WebApi.Extensions
{
    public static class UseHttpsRedirectionElbAwareApplicationBuilderExtensions
    {
        /// <summary>
        /// Redirect http requests to https. Recognises if SSL offloading
        /// is being done with an ELB (by checking the X-Forwarded-Proto header)
        /// </summary>
        public static IApplicationBuilder UseHttpsRedirectionElbAware(this IApplicationBuilder app)
        {
            app.Use(async (context, next) => {
                var req = context.Request;
                var resp = context.Response;

                if (req.IsHttps)
                {
                    await next.Invoke();
                    return;
                }

                // Check if it's an SSL offloaded https request from an Amazon
                // Elastic Load Balancer. When an Amazon Elastic Load Balancer
                /// is doing SSL offloading it will set an X-Forwarded-Proto
                /// header which will be http or https depending on the protocol
                /// requested by the user. We can detect this and redirect to
                /// https accordingly
                StringValues forwardedProtocol;
                if (req.Headers.TryGetValue("X-Forwarded-Proto", out forwardedProtocol)
                    && forwardedProtocol.ToString() == "https")
                {
                    await next.Invoke();
                    return;
                }

                resp.Redirect($"https://{req.Host}{req.PathBase}{req.Path}{req.QueryString}", true);
            });

            return app;
        }
    }
}
