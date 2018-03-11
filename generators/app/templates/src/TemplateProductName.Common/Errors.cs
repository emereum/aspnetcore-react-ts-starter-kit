using System;
using System.Collections.Generic;
using FluentValidation.Results;

namespace TemplateProductName.Common
{
    /// <summary>
    /// Wrapper class for validation dictionaries.
    /// </summary>
    [Serializable]
    public class Errors : Dictionary<Guid, Dictionary<string, List<ValidationFailure>>>
    {
        public bool IsValid => Count == 0;

        public static Errors CreateError(IHasGuid model, string propertyName, string errorText)
        {
            var failures = new Dictionary<string, List<ValidationFailure>>
            {
                { propertyName, new List<ValidationFailure> { new ValidationFailure(propertyName, errorText) } }
            };

            return new Errors
            {
                { model.Guid, failures }
            };
        }
    }
}
