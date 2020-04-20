using System;
using System.Linq;
using System.Linq.Expressions;
using FluentValidation;
using Xunit;
using TemplateProductName.Common;

namespace TemplateProductName.Tests.Infrastructure
{
    public abstract class FluentValidatorTest<TModel>
    {
        protected AbstractValidator<TModel> Validator;
        protected TModel Model;

        protected void AssertHasValidationError<TProperty>(
            string errorMessageStartsWith,
            Expression<Func<TModel, TProperty>> property)
        {
            var propertyName = TypeExtensions.NameOf(property);
            AssertHasValidationError(errorMessageStartsWith, propertyName);
        }

        protected void AssertHasValidationError(
            string errorMessageStartsWith,
            string propertyName)
        {
            var errors = Validator
                .Validate(Model)
                .Errors;

            var result = errors.Any(x =>
                x.ErrorMessage.StartsWith(errorMessageStartsWith) &&
                x.PropertyName == propertyName);

            var errorMessageSummary = string.Join(", ",
                errors.Select(x => "\r\n" + x.PropertyName + ": " + x.ErrorMessage));

            Assert.True(result, $"Expected error message like '{errorMessageStartsWith}' " +
                                  $"for property '{propertyName}' but got {errorMessageSummary}");
        }

        protected void AssertNoValidationError<TProperty>(
            string errorMessageStartsWith,
            Expression<Func<TModel, TProperty>> property)
        {
            var propertyName = TypeExtensions.NameOf(property);

            AssertNoValidationError(errorMessageStartsWith, propertyName);
        }

        protected void AssertNoValidationError(
            string errorMessageStartsWith,
            string propertyName)
        {
            var errors = Validator
                .Validate(Model)
                .Errors;

            var result = errors.Any(x =>
                x.ErrorMessage.StartsWith(errorMessageStartsWith) &&
                x.PropertyName == propertyName);

            var errorMessageSummary = string.Join(", ",
                errors.Select(x => "\r\n" + x.PropertyName + ": " + x.ErrorMessage));

            Assert.False(result, $"Expected no error messages like '{errorMessageStartsWith}' " +
                                  $"for property '{propertyName}' but got {errorMessageSummary}");
        }

        protected void AssertNoValidationError<TProperty>(
            Expression<Func<TModel, TProperty>> property)
        {
            var propertyName = TypeExtensions.NameOf(property);

            var errors = Validator
                .Validate(Model)
                .Errors;

            var result = errors.Any(x => x.PropertyName == propertyName);

            var errorMessageSummary = string.Join(", ",
                errors.Select(x => "\r\n" + x.PropertyName + ": " + x.ErrorMessage));

            Assert.False(result, $"Expected no error messages for property '{propertyName}' " +
                                   $"but got {errorMessageSummary}");
        }
    }
}
