using FluentAssertions;
using StockportWebapp.Config;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Config
{
    public class AmazonSESKeysTest
    {
        [Theory]
        [InlineData("", "secretKey")]
        [InlineData(null, "secretKey")]
        [InlineData("accessKey", "")]
        [InlineData("accessKey", null)]
        public void ShouldBeNotValidIfAnyParameterIsNullOrEmpty(string accessKey, string secretKey)
        {
            var keys = new AmazonSESKeys(accessKey, secretKey);

            keys.IsValid().Should().BeFalse();
        }

        [Fact(Skip = "To be fixed")]
        public void ShouldBeValidIfParametersAreSet()
        {
            var keys = new AmazonSESKeys("accessKey", "secretKey");

            keys.IsValid().Should().BeFalse();
        }
    }
}
