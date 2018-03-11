using System;
using System.Linq.Expressions;

namespace TemplateProductName.Common
{
    public static class TypeExtensions
    {
        public static bool ImplementsInterface<TInterface>(this Type type) =>
            typeof(TInterface).IsAssignableFrom(type) && !type.IsInterface;

        public static bool ImplementsInterface(this Type type, Type interfaceType) =>
            interfaceType.IsAssignableFrom(type) && !type.IsInterface;

        public static string NameOf<TClass, TProperty>(Expression<Func<TClass, TProperty>> property)
        {
            var expr = (MemberExpression) property.Body;
            return expr.Member.Name;
        }
    }
}
