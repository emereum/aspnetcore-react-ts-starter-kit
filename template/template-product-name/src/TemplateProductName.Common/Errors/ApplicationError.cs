namespace TemplateProductName.Common.Errors
{
    /// <summary>
    /// A higher-level error than ValidationErrors. This can contain only one
    /// error message that describes why the entire request failed.
    /// </summary>
    public class ApplicationError : IErrors
    {
        public string Code { get; }
        public string Message { get; }

        public ApplicationError(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
