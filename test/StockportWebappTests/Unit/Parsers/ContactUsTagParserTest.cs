using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;
using StockportWebapp.ViewDetails;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Parsers
{
    public class ContactUsTagParserTest
    {
        private readonly ContactUsTagParser _contactUsTagParser;
        private readonly Mock<IViewRender> _viewRenderer;
        private static string ContactUsMessageTag = "<!-- ##CONTACT_US_MESSAGE## -->";
        private readonly Mock<ILogger<ContactUsTagParser>> _mockLogger;

        public ContactUsTagParserTest()
        {
            _viewRenderer = new Mock<IViewRender>();
            _mockLogger = new Mock<ILogger<ContactUsTagParser>>();
            _contactUsTagParser = new ContactUsTagParser(_viewRenderer.Object, _mockLogger.Object);
        }

        [Fact]
        public void ItParsesTheContactForm()
        {
            var email = "fake.email@email.com";
            var content = $"{{{{CONTACT-US: {email}}}}}";
            var renderResult = "result";

            _viewRenderer.Setup(o => o.Render("ContactUs", It.Is<ContactUsDetails>(d => d.ServiceEmailId == email)))
                .Returns(renderResult);

            var parsedHtml = _contactUsTagParser.Parse(content, null);

            _viewRenderer.Verify(o => o.Render("ContactUs", It.Is<ContactUsDetails>(d => d.ServiceEmailId == email)));
            parsedHtml.Should().Contain(renderResult);
        }

        [Fact]
        public void ShouldAddContactUsMessageAboveTheForm()
        {
            var content = $"{{{{CONTACT-US: something}}}}";
            var renderResult = "<form></form>";

            _viewRenderer.Setup(o => o.Render("ContactUs", It.IsAny<ContactUsDetails>())).Returns(renderResult);

            var parsedHtml = _contactUsTagParser.Parse(content, null);

            parsedHtml.Should().Be(ContactUsMessageTag + renderResult);
        }

        [Fact]
        public void ShouldReturnInvalidContentMessageForEmptyServiceEmail()
        {
            var content = $"{{{{CONTACT-US:}}}}";
            var parsedHtml = _contactUsTagParser.Parse(content, null);

            parsedHtml.Should().Be("<p>This contact form is temporarily unavailable. Please check back later.</p>");
            LogTesting.Assert(_mockLogger, LogLevel.Error,
                $"The service email ID in this CONTACT-US tag is invalid and this contact form will not render.");
        }

        [Fact]
        public void ShouldPassTitleToRenderer()
        {
            // Arrange
            var content = $"{{{{CONTACT-US:test@mail.com}}}}";
            const string title = "Test Title";
            _viewRenderer.Setup(renderer =>
                renderer.Render(
                    It.Is<string>(s => s == "ContactUs"),
                    It.Is<ContactUsDetails>(cont => cont.Title == title)));

            // Act
            _contactUsTagParser.Parse(content, title);

            // Assert
            _viewRenderer.Verify();
        }
    }
}