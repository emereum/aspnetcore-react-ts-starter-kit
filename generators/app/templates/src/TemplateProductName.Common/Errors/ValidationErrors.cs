using System.Collections.Generic;

namespace TemplateProductName.Common.Errors
{
    public class ValidationErrors : List<ValidationError>, IErrors
    {
        public ValidationErrors(IEnumerable<ValidationError> errors) : base(errors) { }
    }
}
