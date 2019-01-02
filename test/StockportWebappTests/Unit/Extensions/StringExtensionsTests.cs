using Xunit;
using FluentAssertions;
using StockportWebapp.Extensions;

namespace StockportWebappTests_Unit.Unit.Extensions
{
    public class StringExtensionsTests
    {
        [Fact]
        public void ShouldRemoveHttpFromStartOfString()
        {
            var result = "http://testing.com".StripHttpAndHttps();

            result.Should().Be("testing.com");
        }

        [Fact]
        public void ShouldRemoveHttpsFromStartOfString()
        {
            var result = "https://testing.com".StripHttpAndHttps();

            result.Should().Be("testing.com");
        }

        [Fact]
        public void ShouldRemoveMoreThanOneHttporHttpssFromStartOfString()
        {
            var result = "https://https://http://testing.com".StripHttpAndHttps();

            result.Should().Be("testing.com");
        }

        [Fact]
        public void ShouldRemoveEmojisFromString()
        {
            var result = "😀🙏☀⛿test".StripEmojis();

            result.Should().Be("test");
        }

        [Fact]
        public void ShouldRemoveEmojisInTheMiddleOfAString()
        {
            var result = "😀🙏☀⛿te☀⛿s☀⛿t".StripEmojis();

            result.Should().Be("test");
        }
    }
}
