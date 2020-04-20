using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using TemplateProductName.Tests.Infrastructure.AutoTest.AutoTests;

namespace TemplateProductName.Tests.Infrastructure.AutoTest
{
    public static class AutoTestExtensions
    {
        private static bool initialised;
        private static List<IAutoTest> autoTests;

        public static object[] Args(this object testFixture, params object[] args) => args;

        public static void AutoTest(this object testFixture, object[] args = null, [CallerMemberName] string testName = "")
        {
            if (!initialised)
            {
                Initialise();
                initialised = true;
            }

            var autoTest = autoTests.FirstOrDefault(x => x.CanTest(testFixture, testName, args));
            if (autoTest == null)
            {
                throw new InvalidOperationException("Could not find an auto test implementation to handle this test case.");
            }

            autoTest.RunTest(testFixture, testName, args);
        }

        private static void Initialise() =>
            autoTests =
                Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where(x => x.IsClass && typeof(IAutoTest).IsAssignableFrom(x))
                    .Select(Activator.CreateInstance)
                    .Cast<IAutoTest>()
                    .ToList();
    }
}
