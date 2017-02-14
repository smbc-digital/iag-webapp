using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests.Unit.Utils
{
    public class ContentSecurityPolicyElementTest
    {
        [Fact]
        public void CspElementContainsSpecifiedSourceType()
        {
            // Arrange
            string specifiedSourceType = "specified-source-type";
            var cspElement = new ContentSecurityPolicyElement(specifiedSourceType);

            // Act
            var elementString = cspElement.Finish();

            // Assert
            Assert.Equal(true, elementString.Contains(specifiedSourceType));
        }

        [Fact]
        public void CspElementContainsSelf()
        {
            // Arrange
            var cspElement = new ContentSecurityPolicyElement("source");

            // Act
            var elementString = cspElement.Finish();

            // Assert
            Assert.Equal(true, elementString.Contains(" 'self'"));
        }

        [Fact]
        public void CspElementContainsSpecifiedSource()
        {
            // Arrange
            string specifiedSource = "specified-source";
            var cspElement = new ContentSecurityPolicyElement("sourceType");

            // Act
            var elementString = cspElement
                .AddSource(specifiedSource)
                .Finish();

            // Assert
            Assert.Equal(true, elementString.Contains(specifiedSource));
        }

        [Fact]
        public void CspElementEndsWithSemiColonAndSpace()
        {
            // Arrange
            var cspElement = new ContentSecurityPolicyElement("source-type");

            // Act
            var elementString = cspElement.Finish();

            // Assert
            Assert.Equal(true, elementString.EndsWith("; "));
        }

        [Fact]
        public void CspElementContainsAllPartsInCorrectOrder()
        {
            // Arrange
            string sourceType = "source-type";
            var source1 = "http://source1.com";
            var source2 = "http://source2.com";

            // Act
            var elementString = new ContentSecurityPolicyElement(sourceType)
                .AddSource(source1)
                .AddSource(source2)
                .Finish();

            // Assert
            Assert.Equal("source-type 'self' http://source1.com http://source2.com; ", elementString);
        }
    }
}