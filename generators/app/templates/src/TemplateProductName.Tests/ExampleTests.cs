using Xunit;

namespace TemplateProductName.Tests
{
    public class ExampleTests
    {
        [Fact]
        public void OnePlusOneEqualsTwo()
        {
            const int one = 1;

            var result = one + one;

            Assert.Equal(2, result);
        }
    }
}
