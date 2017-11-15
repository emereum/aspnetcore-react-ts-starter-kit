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
    }
}
