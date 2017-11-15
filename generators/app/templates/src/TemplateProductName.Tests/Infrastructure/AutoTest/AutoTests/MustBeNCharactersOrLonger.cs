using System;
using System.Text.RegularExpressions;
using TemplateProductName.Tests.Extensions;

namespace TemplateProductName.Tests.Infrastructure.AutoTest.AutoTests
{
    class MustBeNCharactersOrLonger: IAutoTest
    {
        public bool CanTest(object testFixture, string testName, object[] args) =>
            testName.EndsWith("CharactersOrLonger") && testFixture is ICommandHandlerTest;

        public void RunTest(object testFixture, string testName, object[] args)
        {
            var minLengthMatch = Regex.Match(testName, "^([a-zA-Z]+)MustBe([0-9]+)CharactersOrLonger$");
            if(!minLengthMatch.Success) throw new InvalidOperationException("Could not extract required information from test name.");

            var propName = minLengthMatch.Groups[1].Value;
            var minLength = int.Parse(minLengthMatch.Groups[2].Value);

            var cmdHandlerTestFixture = (ICommandHandlerTest) testFixture;

            // Get TestFixture.Command
            var command = cmdHandlerTestFixture.GetCommand();

            // Get TestFixture.Command.{propName}
            var propInfo = command.GetType().GetProperty(propName);
            if (propInfo == null) throw new InvalidOperationException($"Couldn't find property {propName} on command {command.GetType().Name}");

            // Check too short case
            var tooShortVal = new string('X', minLength - 1);
            Console.WriteLine($"Testing {command.GetType().Name}.{propName} = {tooShortVal} (too short, should fail validation)");
            propInfo.SetValue(command, tooShortVal);
            TestExtensions.AssertHasValidationErrorCode(cmdHandlerTestFixture.Handle(), "predicate_error", propName);

            // Check just long enough case
            var justRightVal = tooShortVal + 'X';
            Console.WriteLine($"Testing {command.GetType().Name}.{propName} = {justRightVal} (just long enough, should pass validation)");
            propInfo.SetValue(command, justRightVal);
            TestExtensions.AssertNoValidationErrorCode(cmdHandlerTestFixture.Handle(), "predicate_error", propName);

            // Check rather long case
            var ratherLongVal = tooShortVal + "XXXXXXX";
            Console.WriteLine($"Testing {command.GetType().Name}.{propName} = {ratherLongVal} (rather long, should pass validation)");
            propInfo.SetValue(command, ratherLongVal);
            TestExtensions.AssertNoValidationErrorCode(cmdHandlerTestFixture.Handle(), "predicate_error", propName);
        }
    }
}
