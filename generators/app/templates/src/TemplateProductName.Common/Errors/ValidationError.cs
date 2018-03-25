using System;

namespace TemplateProductName.Common.Errors
{
    /// <summary>
    /// A validation error is an error that is tied to a specific property of
    /// a specific model.
    /// </summary>
    public class ValidationError
    {
        public Guid Guid { get; }
        public string Property { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }

        public ValidationError(Guid guid, string property, string code, string message)
        {
            Guid = guid;
            Property = property;
            Code = code;
            Message = message;
        }
    }
}
