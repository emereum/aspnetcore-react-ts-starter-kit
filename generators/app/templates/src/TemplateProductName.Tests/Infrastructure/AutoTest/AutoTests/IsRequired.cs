using System;
using TemplateProductName.Tests.Extensions;

namespace TemplateProductName.Tests.Infrastructure.AutoTest.AutoTests
{
    /// <summary>
    /// Assumes the test fixture has a command property and a handler property.
    /// Checks that a particular command property is required per the validation
    /// rules that run in the command handler.
    /// </summary>
    public class IsRequired : IAutoTest
    {
        public bool CanTest(object testFixture, string testName, object[] args) =>
            testName.EndsWith("IsRequired") && testFixture is ICommandHandlerTest;

        public void RunTest(object testFixture, string testName, object[] args)
        {
            var cmdHandlerTestFixture = (ICommandHandlerTest) testFixture;

            // Get TestFixture.Command
            var command = cmdHandlerTestFixture.GetCommand();

            // Get TestFixture.Command.{propName}
            var propName = testName.Substring(0, testName.Length - "IsRequired".Length);
            var propInfo = command.GetType().GetProperty(propName);
            if (propInfo == null)
            {
                throw new InvalidOperationException($"Couldn't find property {propName} on command {command.GetType().Name}");
            }

            // Check null case
            Console.WriteLine($"Testing {command.GetType().Name}.{propName} = null (should fail validation)");
            propInfo.SetValue(command, null);
            TestExtensions.AssertHasValidationErrorCode(cmdHandlerTestFixture.Handle(), "notempty_error", propName);

            // Check non null case
            var val = TestExtensions.GetNonNullDummyValue(propInfo.PropertyType);
            Console.WriteLine($"Testing {command.GetType().Name}.{propName} = {val} (should pass validation)");
            propInfo.SetValue(command, val);
            TestExtensions.AssertNoValidationErrorCode(cmdHandlerTestFixture.Handle(), "notempty_error", propName);
        }
    }
}
