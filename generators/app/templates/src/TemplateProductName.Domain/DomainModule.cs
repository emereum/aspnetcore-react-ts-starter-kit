using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace TemplateProductName.Domain
{
    [ExcludeFromCodeCoverage]
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();

            // Command Handlers
            builder.RegisterAssemblyTypes(executingAssembly)
                .Where(t => t.Namespace != null && t.Namespace.Contains("Features") && t.Name.EndsWith("Handler"))
                .AsSelf()
                .InstancePerLifetimeScope();

            // Validators
            builder.RegisterAssemblyTypes(executingAssembly)
                .Where(t => t.Namespace != null && t.Namespace.Contains("Features") && t.Name.EndsWith("Validator"))
                .AsSelf()
                .InstancePerLifetimeScope();

            // Mappers
            builder.RegisterAssemblyTypes(executingAssembly)
                .Where(t => t.Namespace != null && t.Namespace.Contains("Features") && t.Name.EndsWith("Mapper"))
                .AsSelf()
                .InstancePerLifetimeScope();

            // Queries
            builder.RegisterAssemblyTypes(executingAssembly)
                .Where(t => t.Namespace != null && t.Namespace.Contains("Queries") && t.Name.EndsWith("Query"))
                .AsSelf()
                .InstancePerLifetimeScope();

            // Services
            builder.RegisterAssemblyTypes(executingAssembly)
                .Where(t => t.Namespace != null && t.Namespace.Contains("Services") && t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
