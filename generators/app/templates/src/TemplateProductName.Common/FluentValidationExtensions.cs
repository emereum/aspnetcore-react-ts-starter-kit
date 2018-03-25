using System;
using System.Globalization;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using TemplateProductName.Common.Errors;

namespace TemplateProductName.Common
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> NumberFormat<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> builder, string format) =>
            builder.SetValidator(new NumberFormatValidator(format));

        public static IRuleBuilderOptions<T, TProperty> NumberFormat<T, TProperty>(
            this IRuleBuilderInitial<T, TProperty> builder, string format) =>
            builder.SetValidator(new NumberFormatValidator(format));

        public static IRuleBuilderOptions<TModel, TReturn> WithGuid<TModel, TReturn>(this IRuleBuilderOptions<TModel, TReturn> builder) where TModel : IHasGuid =>
            builder.WithState(x => x.Guid);

        /// <summary>
        /// Returns a list of errors including the Guid of the model that caused
        /// the error (which is retrieved from their CustomState field).
        /// Validators should set each error state to entity-under-validation's 
        /// Guid and use this method to provide error responses back to a
        /// client. The client can then use the Guids to map errors back to
        /// their corresponding entities when a hierarchy of objects is being
        /// validated.
        /// </summary>
        public static IErrorResponse ToErrorResponse(this ValidationResult validationResult)
        {
            if (validationResult.IsValid)
            {
                return null;
            }

            // Ensure all validation results are tied to their model guids
            var badValidationResult = validationResult.Errors.FirstOrDefault(x => x.CustomState == null);

            if (badValidationResult != null)
            {
                throw new InvalidOperationException(
                    $"Validation rule for '{badValidationResult.ErrorMessage}' " +
                    $"must have .WithGuid() to support IErrorResponse.");
            }

            var errors =
                validationResult
                .Errors
                .Select(x => new ValidationError(
                    (Guid)x.CustomState,
                    CanonicalizePropertyName(x.PropertyName),
                    x.ErrorCode,
                    x.ErrorMessage
                ));

            return new ValidationErrors(errors);
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
