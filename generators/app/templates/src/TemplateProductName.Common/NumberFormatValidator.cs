using System;
using System.Text.RegularExpressions;
using FluentValidation.Validators;

namespace TemplateProductName.Common
{
    public class NumberFormatValidator : PropertyValidator
    {
        private readonly Regex regex;

        /// <summary>
        /// Validates that a number has a scale and magnitude matching the specified format.
        /// The format should be specified with hashes and up to a single decimal place. For example
        /// "###.##" validates that a number has up to three digits before the decimal place and up to
        /// two digits after it.
        /// </summary>
        /// <param name="format"></param>
        public NumberFormatValidator(string format) : base("'{PropertyName}' must be in the format " + format)
        {
            if (!Regex.IsMatch(format, @"^#+(\.#+)?$"))
            {
                throw new ArgumentException("Format should be of the form ###.##");
            }

            // Convert a format like "###.##" to a regex expression
            // like "^\d{0,3}(\.\d{0,2})?$

            var parts = format.Split('.');

            var wholeNumbers = parts[0].Length;
            var decimalNumbers = parts.Length > 1 ? parts[1].Length : 0;

            regex = decimalNumbers == 0
                ? new Regex(@"^-?\d{0," + wholeNumbers + "}$")
                : new Regex(@"^-?\d{0," + wholeNumbers + @"}(\.\d{0," + decimalNumbers + "})?$");
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            return context.PropertyValue == null || regex.IsMatch(context.PropertyValue.ToString());
        }
    }
}
