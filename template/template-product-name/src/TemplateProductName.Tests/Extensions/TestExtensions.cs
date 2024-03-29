using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TemplateProductName.Common;
using TemplateProductName.Common.Errors;
using Xunit;

namespace TemplateProductName.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class TestExtensions
    {
        /// <summary>
        /// Gets a file relative to the output directory of INFORMd.Tests
        /// </summary>
        /// <param name="parts"></param>
        /// <returns></returns>
        public static Stream GetTestFile(params string[] parts) => File.OpenRead(GetTestFilePath(parts));

        /// <summary>
        /// Gets a file path relative to the output directory of INFORMd.Tests
        /// </summary>
        /// <param name="parts"></param>
        /// <returns></returns>
        public static string GetTestFilePath(params string[] parts)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var codebase = new Uri(assembly.Location);
            var path = codebase.LocalPath;
            var thisDirectory = Path.GetDirectoryName(path);
            var file = Path.Combine(new[] { thisDirectory }.Concat(parts).ToArray());
            return file;
        }

        public static void AssertNoValidationErrors(ValidationErrors errors)
        {
            var errorSummary = errors
                .Select(x => $"\r\n{x.Property}: {x.Message}")
                .Aggregate(", ");

            Assert.False(errors.Any(), "Expected no error messages but got " + errorSummary);
        }

        public static void AssertNoValidationError(
            ValidationErrors errors,
            string errorMessageStartsWith,
            string propertyName)
        {
            var result = HasValidationError(errors, errorMessageStartsWith, propertyName);

            Assert.False(result.Item1, $"Expected no error message like '{errorMessageStartsWith}' " +
                                         $"for property '{propertyName}' but got {result.Item2}");
        }

        public static void AssertHasValidationError(
            ValidationErrors errors,
            string errorMessageStartsWith,
            string propertyName)
        {
            var result = HasValidationError(errors, errorMessageStartsWith, propertyName);

            Assert.True(result.Item1, $"Expected error message like '{errorMessageStartsWith}' " +
                                         $"for property '{propertyName}' but got {result.Item2}");
        }

        public static void AssertNoValidationErrorCode(
            ValidationErrors errors,
            string errorCode,
            string propertyName)
        {
            var result = HasValidationErrorCode(errors, errorCode, propertyName);

            Assert.False(result.Item1, $"Expected no error code like '{errorCode}' " +
                                         $"for property '{propertyName}' but got {result.Item2}");
        }

        public static void AssertHasValidationErrorCode(
            ValidationErrors errors,
            string errorCode,
            string propertyName)
        {
            var result = HasValidationErrorCode(errors, errorCode, propertyName);

            Assert.True(result.Item1, $"Expected error code like '{errorCode}' " +
                                        $"for property '{propertyName}' but got {result.Item2}");
        }

        private static Tuple<bool, string> HasValidationError(
            ValidationErrors errors,
            string errorMessageStartsWith,
            string propertyName)
        {
            var hasError = errors
                .Any(x =>
                    x.Message.StartsWith(errorMessageStartsWith) &&
                    x.Property == propertyName);

            var errorSummary = errors
                .Select(x => $"\r\n{x.Property}: {x.Message}")
                .Aggregate(", ");

            return Tuple.Create(hasError, errorSummary);
        }

        private static Tuple<bool, string> HasValidationErrorCode(
            ValidationErrors errors,
            string errorCode,
            string propertyName)
        {
            var hasError = errors
                .Any(x =>
                    x.Code.Equals(errorCode) &&
                    x.Property == propertyName);

            var errorSummary = errors
                .Select(x => $"\r\n{x.Property}: {x.Code}")
                .Aggregate(", ");

            return Tuple.Create(hasError, errorSummary);
        }

        public static void IsRequired<TEntity, TProperty>(
            Func<ValidationErrors> getErrors,
            TEntity entity,
            Expression<Func<TEntity, TProperty>> prop)
        {
            var expr = (MemberExpression)prop.Body;
            var propInfo = (PropertyInfo)expr.Member;
            var nonNullDummyValue = GetNonNullDummyValue<TProperty>();

            // Check null case
            propInfo.SetValue(entity, null, null);
            AssertHasValidationErrorCode(getErrors(), "notempty_error", propInfo.Name);

            // Check non-null case
            propInfo.SetValue(entity, nonNullDummyValue, null);
            AssertNoValidationErrorCode(getErrors(), "notempty_error", propInfo.Name);
        }

        public static object GetNonNullDummyValue<T>()
        {
            var type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            return GetNonNullDummyValue(type);
        }

        public static object GetNonNullDummyValue(Type type)
        {
            if (type == typeof(int))
            {
                return 42;
            }

            if (type == typeof(string))
            {
                return "DEADBEEF";
            }

            if (type == typeof(DateTime))
            {
                return new DateTime(2000, 01, 01);
            }

            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                // If the property implements IEnumerable return an empty list.
                var constructor = typeof(List<>)
                    .MakeGenericType(type.GetGenericArguments().Single())
                    .GetConstructor(Type.EmptyTypes);

                if (constructor == null)
                {
                    throw new InvalidOperationException("Can't find a constructor");
                }

                return (IList)constructor.Invoke(null);
            }

            return Activator.CreateInstance(type);
        }
    }
}
