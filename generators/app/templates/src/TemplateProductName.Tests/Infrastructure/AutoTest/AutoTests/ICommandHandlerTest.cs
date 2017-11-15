using TemplateProductName.Common;

namespace TemplateProductName.Tests.Infrastructure.AutoTest.AutoTests
{
    interface ICommandHandlerTest
    {
        object GetCommand();
        Errors Handle();
    }
}
