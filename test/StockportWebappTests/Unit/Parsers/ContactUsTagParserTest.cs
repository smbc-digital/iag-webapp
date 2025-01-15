namespace StockportWebappTests_Unit.Unit.Parsers;

public class ContactUsTagParserTest
{
    private readonly ContactUsTagParser _contactUsTagParser;
    private readonly Mock<IViewRender> _viewRenderer = new();
    private readonly Mock<ILogger<ContactUsTagParser>> _mockLogger = new();
    private readonly static string _contactUsMessageTag = "<!-- ##CONTACT_US_MESSAGE## -->";

    public ContactUsTagParserTest() =>
        _contactUsTagParser = new(_viewRenderer.Object, _mockLogger.Object);

    [Fact]
    public void ItParsesTheContactForm()
    {
        // Arrange
        _viewRenderer
            .Setup(renderer => renderer.Render("ContactUs", It.Is<ContactUsDetails>(detail => detail.ServiceEmailId.Equals("fake.email@email.com"))))
            .Returns("result");

        // Act
        string parsedHtml = _contactUsTagParser.Parse("{{CONTACT-US: fake.email@email.com}}", null);

        // Assert
        _viewRenderer.Verify(renderer => renderer.Render("ContactUs", It.Is<ContactUsDetails>(d => d.ServiceEmailId.Equals("fake.email@email.com"))));
        Assert.Contains("result", parsedHtml);
    }

    [Fact]
    public void ShouldAddContactUsMessageAboveTheForm()
    {
        // Arrange
        _viewRenderer
            .Setup(renderer => renderer.Render("ContactUs", It.IsAny<ContactUsDetails>()))
            .Returns("<form></form>");

        // Act
        string parsedHtml = _contactUsTagParser.Parse("{{CONTACT-US: something}}", null);

        // Assert
        Assert.Equal($"{_contactUsMessageTag}<form></form>", parsedHtml);
    }

    [Fact]
    public void ShouldReturnInvalidContentMessageForEmptyServiceEmail()
    {
        // Act
        string parsedHtml = _contactUsTagParser.Parse("{{CONTACT-US:}}", null);

        // Assert
        Assert.Equal("<p>This contact form is temporarily unavailable. Please check back later.</p>", parsedHtml);
        LogTesting.Assert(_mockLogger, LogLevel.Error, $"The service email ID in this CONTACT-US tag is invalid and this contact form will not render.");
    }

    [Fact]
    public void ShouldPassTitleToRenderer()
    {
        // Arrange
        _viewRenderer
            .Setup(renderer =>renderer.Render(It.Is<string>(s => s.Equals("ContactUs")),
                                            It.Is<ContactUsDetails>(cont => cont.Title.Equals("Test Title"))));

        // Act
        _contactUsTagParser.Parse("{{CONTACT-US:test@mail.com}}", "Test Title");

        // Assert
        _viewRenderer.Verify();
    }
}