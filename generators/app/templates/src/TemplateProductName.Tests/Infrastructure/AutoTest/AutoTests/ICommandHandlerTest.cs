using TemplateProductName.Common;

namespace TemplateProductName.Tests.Infrastructure.AutoTest.AutoTests
{
    public interface ICommandHandlerTest
    {
        object GetCommand();
        Errors Handle();
    }
}
