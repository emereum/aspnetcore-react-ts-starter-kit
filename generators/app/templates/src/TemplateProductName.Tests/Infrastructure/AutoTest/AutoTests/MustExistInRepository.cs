using System;
using System.Linq;
using TemplateProductName.Tests.Extensions;

namespace TemplateProductName.Tests.Infrastructure.AutoTest.AutoTests
{
    /// <summary>
    /// Checks that a string property corresponds to a record in a repository.
    /// A function must be provided as an argument which accepts a string and
    /// creates an entity in the repository with that string as an identifier.
    /// </summary>
    public class MustExistInRepository : IAutoTest
    {
        public bool CanTest(object testFixture, string testName, object[] args) =>
            testName.EndsWith("MustExistInRepository") && testFixture is ICommandHandlerTest;

        public void RunTest(object testFixture, string testName, object[] args)
        {
            var cmdHandlerTestFixture = (ICommandHandlerTest) testFixture;

            // Get TestFixture.Command
            var command = cmdHandlerTestFixture.GetCommand();

            // Get TestFixture.Command.{propName}
            var propName = testName.Substring(0, testName.Length - "MustExistInRepository".Length);
            var propInfo = command.GetType().GetProperty(propName);
            if (propInfo == null)
            {
                throw new InvalidOperationException($"Couldn't find property {propName} on command {command.GetType().Name}");
            }

            // Get repository method
            var repositoryMethod = args?.SingleOrDefault() as Action<string>;
            if (repositoryMethod == null)
            {
                throw new InvalidOperationException($"Expected a function argument that accepts a string and adds a record to the repository. Got {args}.");
            }

            // Add an item to the repository
            repositoryMethod.Invoke("imhere");

            // Check not matching the repository entity
            Console.WriteLine($"Testing {command.GetType().Name}.{propName} = idontexist (should fail validation)");
            propInfo.SetValue(command, "idontexist");
            TestExtensions.AssertHasValidationErrorCode(cmdHandlerTestFixture.Handle(), "notinrepository_error", propName);

            // Check matching the repository entity 
            Console.WriteLine($"Testing {command.GetType().Name}.{propName} = imhere (should pass validation)");
            propInfo.SetValue(command, "imhere");
            TestExtensions.AssertNoValidationErrorCode(cmdHandlerTestFixture.Handle(), "notinrepository_error", propName);
        }
    }
}
