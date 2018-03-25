using System.Collections.Generic;

namespace TemplateProductName.Common.Errors
{
    public class ValidationErrors : List<ValidationError>, IErrorResponse
    {
        public ValidationErrors(IEnumerable<ValidationError> errors) : base(errors) { }
    }
}
