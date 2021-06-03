using System;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TemplateProductName.Domain;
using TemplateProductName.Persistence;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using TemplateProductName.WebApi.Extensions;
using TemplateProductName.WebApi.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace TemplateProductName.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add
        // services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });

            // Add framework services.
            services.AddControllers(options =>
                {
                    // Automatically set Http status codes depending on the type
                    // of action result returned from a controller action.
                    options.Filters.Add(new HttpStatusCodeConventionFilter());

                    // Populate controller action parameters from the JSON body
                    // by default. Equivalent to adding [FromBody] to every
                    // complex type parameter.
                    options.Conventions.Add(new BindComplexTypesFromBodyConvention());
                })
                .AddNewtonsoftJson(jsonOptions =>
                {
                    // Trim strings when deserializing JSON payloads.
                    jsonOptions.SerializerSettings.Converters.Add(new TrimStringsJsonConverter());

                    // Serialize enums as strings, not ints.
                    jsonOptions.SerializerSettings.Converters.Add(new StringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy() });

                    // Assume dates are being submitted in the same timezone
                    // as the server's timezone. This is only appropriate
                    // when an app is being developed for a specific city.
                    // Apps that span multiple timezones should remove this
                    // and communicate times in UTC.
                    jsonOptions.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                });

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

            services.AddHttpContextAccessor(); // Enable resolving IHttpContextAccessor through DI.

            services.AddOpenApiDocument(); // For NSwagStudio "Use document provider" option

            // Allow localhost cross-origin requests so
            // TemplateProductName.WebClient can communicate with the API
            // during development
            services.AddCors(x => x.AddPolicy("allowlocal", new CorsPolicy
            {
                Origins = { "http://localhost:3000" },
                Headers = { "*" },
                Methods = { "*" },
                SupportsCredentials = true,
            }));
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        //
        // See: https://autofac.readthedocs.io/en/latest/integration/aspnetcore.html#asp-net-core-3-0-and-generic-hosting
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new PersistenceModule(Configuration["ConnectionString"], Configuration["MappingAssembly"]));
            builder.RegisterModule<DomainModule>();
            builder.RegisterModule<WebModule>();
        }

        // This method gets called by the runtime. Use this method to configure
        // the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // For pipeline ordering, see https://docs.microsoft.com/en-us/aspnet/core/security/gdpr?view=aspnetcore-3.0
            // also see https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30?view=aspnetcore-3.0

            // Create database - for prototyping only. Switch to EF Core Migrations for prod.
            using(var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var client = scope.ServiceProvider.GetRequiredService<DbContext>();
                client.Database.CurrentTransaction.Commit();
                client.Database.EnsureCreated();
            }

            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHsts();
            if (!env.IsDevelopment())
            {
                app.UseHttpsRedirectionElbAware();
            }
            app.UseSpaRouting();
            app.UseStaticFiles();
            app.UseMinimalCaching();
            app.UseCookiePolicy();
            app.UseRouting();

            if (env.IsDevelopment())
            {
                // Allow cross-origin requests from localhost:3000 because
                // TemplateProductName.WebClient and TemplateProductName.WebApi
                // projects run on separate ports during development.
                app.UseCors("allowlocal");
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
