using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Security.Claims;
using Autofac;
using TemplateProductName.WebApi.Services;
using Microsoft.AspNetCore.Http;
using Module = Autofac.Module;

namespace TemplateProductName.WebApi
{
    [ExcludeFromCodeCoverage]
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();

            // Model Mappers
            builder.RegisterAssemblyTypes(executingAssembly)
                .Where(t => t.Namespace != null && t.Namespace.Contains("Mappers") && t.Name.EndsWith("Mapper"))
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerLifetimeScope();

            // Models
            builder.RegisterAssemblyTypes(executingAssembly)
                .Where(t => t.Namespace != null && t.Namespace.Contains("Models") && t.Name.EndsWith("Model"))
                .AsSelf()
                .InstancePerLifetimeScope();

            // Services (Usually services that use HttpContext as implementation
            // but are needed by the domain, like authentication features)
            builder.RegisterType<AuthenticationService>().As<Domain.Services.IAuthenticationService>();

            // Current user
            builder.Register(x =>
            {
                var contextAccessor = x.Resolve<IHttpContextAccessor>();
                var principal = contextAccessor.HttpContext.User;
                return principal;
            })
            .As<ClaimsPrincipal>()
            .InstancePerLifetimeScope();
        }
    }
}
