namespace TemplateProductName.Tests.Infrastructure.AutoTest.AutoTests
{
    interface IAutoTest
    {
        bool CanTest(object testFixture, string testName, object[] args);
        void RunTest(object testFixture, string testName, object[] args);
    }
}
