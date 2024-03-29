using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TemplateProductName.WebApi.Infrastructure
{
    /// <summary>
    /// Alleviates the need to add [FromBody] to every controller action
    /// parameter.
    /// 
    /// See: http://benfoster.io/blog/aspnet-core-customising-model-binding-conventions
    /// See: https://gist.github.com/tugberkugurlu/4bcb7af3682771ba9c18828329f04920
    /// </summary>
    public class BindComplexTypesFromBodyConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            foreach (var parameter in action.Parameters)
            {
                var paramType = parameter.ParameterInfo.ParameterType;
                if (parameter.BindingInfo == null && (IsSimpleType(paramType) || IsSimpleUnderlyingType(paramType)) == false)
                {
                    parameter.BindingInfo = new BindingInfo
                    {
                        BindingSource = BindingSource.Body
                    };
                }
            }
        }

        private static bool IsSimpleType(Type type) =>
            type.IsPrimitive ||
            type == typeof(string) ||
            type == typeof(DateTime) ||
            type == typeof(decimal) ||
            type == typeof(Guid) ||
            type == typeof(DateTimeOffset) ||
            type == typeof(TimeSpan);

        private static bool IsSimpleUnderlyingType(Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
            {
                type = underlyingType;
            }

            return IsSimpleType(type);
        }
    }
}
