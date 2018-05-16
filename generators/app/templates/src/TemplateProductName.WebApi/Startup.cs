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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using TemplateProductName.WebApi.Extensions;
using TemplateProductName.WebApi.Infrastructure;

namespace TemplateProductName.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add
        // services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services
                .AddMvc(options =>
                {
                    // Do not cache mvc responses in IE
                    options.Filters.Add(new ResponseCacheFilter(new CacheProfile { NoStore = true, Location = ResponseCacheLocation.None }));

                    // Automatically set Http status codes depending on the type
                    // of action result returned from a controller action.
                    options.Filters.Add(new HttpStatusCodeConventionFilter());

                    // Populate controller action parameters from the JSON body
                    // by default. Equivalent to adding [FromBody] to every
                    // complex type parameter.
                    options.Conventions.Add(new BindComplexTypesFromBodyConvention());
                })
                .AddJsonOptions(jsonOptions =>
                {
                    // Trim strings when deserializing JSON payloads.
                    jsonOptions.SerializerSettings.Converters.Add(new TrimStringsJsonConverter());

                    // Serialize enums as strings, not ints.
                    jsonOptions.SerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });

                    // Assume dates are being submitted in the same timezone
                    // as the server's timezone. This is only appropriate
                    // when an app is being developed for a specific city.
                    // Apps that span multiple timezones should remove this
                    // and communicate times in UTC.
                    jsonOptions.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;

                    // Don't serialize NHibernate properties on entities.
                    jsonOptions.SerializerSettings.ContractResolver = new NHibernateContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    };
                });

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

            // Create the container builder.
            var builder = new ContainerBuilder();

            // Register dependencies, populate the services from
            // the collection, and build the container.
            builder.RegisterModule(new PersistenceModule(Configuration["ConnectionString"], Configuration["MappingAssembly"]));
            builder.RegisterModule<DomainModule>();
            builder.RegisterModule<WebModule>();
            builder.Populate(services);
            var container = builder.Build();

            // Return the IServiceProvider resolved from the container.
            return container.Resolve<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure
        // the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();

                // Allow cross-origin requests from localhost:3000 because
                // TemplateProductName.WebClient and TemplateProductName.WebApi
                // projects run on separate ports during development.
                app.UseCors("allowlocal");
            }
            else
            {
                // todo: loggerFactory.AddEventLog();
                // Temporarily just log to console in release mode.
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            }

            app.UseMvc();

            // Rewrite all non-api and non-file requests to /index.html which
            // is where the SPA artifacts are deployed in production. This
            // allows a client-side router to delegate URLs to pages within the
            // SPA.
            app.UseSpaRouting();
            app.UseStaticFiles();
        }
    }
}
