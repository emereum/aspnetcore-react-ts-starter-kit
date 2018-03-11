using System;
using System.Globalization;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;

namespace TemplateProductName.Common
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> NumberFormat<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> builder, string format)
        {
            return builder.SetValidator(new NumberFormatValidator(format));
        }

        public static IRuleBuilderOptions<T, TProperty> NumberFormat<T, TProperty>(
            this IRuleBuilderInitial<T, TProperty> builder, string format)
        {
            return builder.SetValidator(new NumberFormatValidator(format));
        }

        public static IRuleBuilderOptions<TModel, TReturn> WithGuid<TModel, TReturn>(this IRuleBuilderOptions<TModel, TReturn> builder) where TModel : IHasGuid
        {
            return builder.WithState(x => x.Guid);
        }

        /// <summary>
        /// Groups errors by their "CustomState" field then by the
        /// "PropertyName" field. Validators should set each error state to
        /// entity-under-validation's Guid and use this method to provide
        /// grouped error sets back to a client. The client can then use the
        /// Guids to map errors back to their corresponding entities when a
        /// hierarchy of objects is being validated.
        /// </summary>
        /// <param name="validationResult"></param>
        /// <returns></returns>
        public static Errors ToErrorDictionary(this ValidationResult validationResult)
        {
            var errorsByGuids = validationResult
                .Errors
                .GroupBy(x =>
                {
                    if (x.CustomState == null)
                    {
                        throw new InvalidOperationException(
                            $"Validation rule for '{x.ErrorMessage}' must have" +
                            ".WithGuid() to support ErrorDictionaries.");
                    }
                    return (Guid)x.CustomState;
                });

            var errors = new Errors();
            foreach (var errorsByGuid in errorsByGuids)
            {
                var errorsByProperties = errorsByGuid
                    .GroupBy(x => CanonicalizePropertyName(x.PropertyName))
                    .ToDictionary(k => k.Key, v => v.ToList());
                errors.Add(errorsByGuid.Key, errorsByProperties);
            }
            return errors;
        }

        /// <summary>
        /// Strips off anything before a dot in "Parent[42].SomeChildProperty"
        /// and converts to lowercase. This is needed so the client can easily
        /// look up errors for properties without having to consider
        /// capitalisation issues (these are PascalCase in the API and CamelCase
        /// in the client) as well as the various ways FluentValidation can
        /// name a property - it differs whether we're using child collection
        /// validators and a number of other factors.
        /// </summary>
        private static string CanonicalizePropertyName(string propertyName)
        {
            var dotIndex = propertyName.LastIndexOf(".", StringComparison.Ordinal);
            if (dotIndex > -1)
            {
                propertyName = propertyName.Substring(dotIndex + 1, propertyName.Length - dotIndex - 1);
            }

            return propertyName.ToLower(CultureInfo.InvariantCulture);
        }
    }
}
