using TemplateProductName.Common;
using TemplateProductName.Common.Errors;

namespace TemplateProductName.Tests.Infrastructure.AutoTest.AutoTests
{
    public interface ICommandHandlerTest
    {
        object GetCommand();
        ValidationErrors Handle();
    }
}
