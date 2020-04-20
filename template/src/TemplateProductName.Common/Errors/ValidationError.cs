using System;

namespace TemplateProductName.Common.Errors
{
    /// <summary>
    /// A validation error is an error that is tied to a specific property of
    /// a specific model.
    /// </summary>
    public class ValidationError
    {
        public Guid Id { get; }
        public string Property { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }

        public ValidationError(Guid id, string property, string code, string message)
        {
            Id = id;
            Property = property;
            Code = code;
            Message = message;
        }
    }
}
