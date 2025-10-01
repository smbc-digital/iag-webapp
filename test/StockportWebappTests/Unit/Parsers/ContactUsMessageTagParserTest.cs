using Newtonsoft.Json.Linq;

namespace StockportWebappTests_Unit.Unit.Parsers;

public class ContactUsMessageTagParserTest
{
    private readonly ContactUsMessageTagParser _tagParser;
    private readonly Mock<IViewRender> _viewRenderer = new();
    private readonly string _bodyWithContactUsMessageTag = $"This is some content {ContactUsTagParser.ContactUsMessageTagRegex} <form></form>";

    public ContactUsMessageTagParserTest()
    {
        _viewRenderer
            .Setup(renderer => renderer.Render("ContactUsMessage", It.IsAny<string>()))
            .Returns("<p>This is a message</p>");

        _tagParser = new(_viewRenderer.Object);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void ShouldNotAddAnyMessageIfNoMessageGiven(string message)
    {
        // Arrange
        ProcessedSection section = ProcessedSectionWithDefaultSlugAndBody();
        ProcessedSection anotherSection = ProcessedSectionWithDefaultSlugAndBody("this-is-a-slug", _bodyWithContactUsMessageTag);
        ProcessedArticle processedArticle = new("title",
                                                "slug",
                                                "default body",
                                                "teaser",
                                                "meta description",
                                                new List<ProcessedSection>() { section, anotherSection },
                                                "icon",
                                                "backgroundImage",
                                                "image",
                                                "alt",
                                                new List<Crumb>(),
                                                new List<Alert>(),
                                                DefaultTopic(),
                                                new List<Alert>(),
                                                new DateTime(),
                                                new bool(),
                                                null,
                                                "logo",
                                                null,
                                                string.Empty,
                                                string.Empty,
                                                new DateTime(),
                                                new List<InlineQuote>(),
                                                new List<Event>(),
                                                string.Empty,
                                                new JObject());

        // Act
        _tagParser.Parse(processedArticle, message, "this-is-a-slug");

        // Assert
        Assert.Equal("default body", processedArticle.Body);
        Assert.Equal("default body", section.Body);
        Assert.Equal(_bodyWithContactUsMessageTag, anotherSection.Body);
        _viewRenderer.Verify(renderer => renderer.Render(It.IsAny<string>(), It.IsAny<object>()), Times.Never);
    }

    [Fact]
    public void ShouldAddErrorMessageToArticleBodyWithFormTagInsideIfEmptySlugGiven()
    {
        // Arrange
        ProcessedArticle processedArticle = new("title",
                                                "slug",
                                                _bodyWithContactUsMessageTag,
                                                "teaser",
                                                "meta description",
                                                new List<ProcessedSection>(),
                                                "icon",
                                                "backgroundImage",
                                                "image",
                                                "altText",
                                                new List<Crumb>(),
                                                new List<Alert>(),
                                                DefaultTopic(),
                                                new List<Alert>(),
                                                new DateTime(),
                                                new bool(),
                                                null,
                                                "logo",
                                                null,
                                                string.Empty,
                                                string.Empty,
                                                new DateTime(),
                                                new List<InlineQuote>(),
                                                new List<Event>(),
                                                string.Empty,
                                                new JObject());

        // Act
        _tagParser.Parse(processedArticle, "This is a message", string.Empty);

        // Assert
        Assert.Equal("This is some content <p>This is a message</p> <form></form>", processedArticle.Body);
    }

    [Fact]
    public void ShouldAddErrorMessageToFirstSectionBodyWithFormTagInsideIfArticleDoesntHaveFormIfEmptySlugGiven()
    {
        // Arrange
        ProcessedSection section = ProcessedSectionWithDefaultSlugAndBody(body: _bodyWithContactUsMessageTag);
        ProcessedArticle processedArticle = new("title",
                                                "slug",
                                                "default body",
                                                "teaser",
                                                "meta description",
                                                new List<ProcessedSection>() { section },
                                                "icon",
                                                "backgroundImage",
                                                "image",
                                                "alt",
                                                new List<Crumb>(),
                                                new List<Alert>(),
                                                DefaultTopic(),
                                                new List<Alert>(),
                                                new DateTime(),
                                                new bool(),
                                                null,
                                                "logo",
                                                null,
                                                string.Empty,
                                                string.Empty,
                                                new DateTime(),
                                                new List<InlineQuote>(),
                                                new List<Event>(),
                                                string.Empty,
                                                new JObject());

        // Act
        _tagParser.Parse(processedArticle, "This is a message", string.Empty);

        // Assert
        Assert.Equal("default body", processedArticle.Body);
        Assert.Equal("This is some content <p>This is a message</p> <form></form>", section.Body);
    }

    [Fact]
    public void ShouldAddErrorMessageToSectionBodyWithFormTagInsideIfCorrespondingSlugGiven()
    {
        // Arrange
        ProcessedSection section = ProcessedSectionWithDefaultSlugAndBody();
        ProcessedSection anotherSection = ProcessedSectionWithDefaultSlugAndBody("this-is-a-slug", _bodyWithContactUsMessageTag);
        ProcessedArticle processedArticle = new("title",
                                                "slug",
                                                "default body",
                                                "teaser",
                                                "meta description",
                                                new List<ProcessedSection>() { section, anotherSection },
                                                "icon",
                                                "backgroundImage",
                                                "image",
                                                "alt",
                                                new List<Crumb>(),
                                                new List<Alert>(),
                                                DefaultTopic(),
                                                new List<Alert>(),
                                                new DateTime(),
                                                new bool(),
                                                null,
                                                "logo",
                                                null,
                                                string.Empty,
                                                string.Empty,
                                                new DateTime(),
                                                new List<InlineQuote>(),
                                                new List<Event>(),
                                                string.Empty,
                                                new JObject());

        // Act
        _tagParser.Parse(processedArticle, "This is a message", "this-is-a-slug");

        // Assert
        Assert.Equal("default body", processedArticle.Body);
        Assert.Equal("default body", section.Body);
        Assert.Equal("This is some content <p>This is a message</p> <form></form>", anotherSection.Body);
    }

    [Fact]
    public void ShouldDoNothingIfSlugProvidedButNoSectionsAreProvided()
    {
        // Arrange
        ProcessedArticle processedArticle = new("title",
                                                "slug",
                                                "default body",
                                                "teaser",
                                                "meta description",
                                                new List<ProcessedSection>() { },
                                                "icon",
                                                "backgroundImage",
                                                "image",
                                                "alt",
                                                new List<Crumb>(),
                                                new List<Alert>(),
                                                DefaultTopic(),
                                                new List<Alert>(),
                                                new DateTime(),
                                                new bool(),
                                                null,
                                                "logo",
                                                null,
                                                string.Empty,
                                                string.Empty,
                                                new DateTime(),
                                                new List<InlineQuote>(),
                                                new List<Event>(),
                                                string.Empty,
                                                new JObject());

        // Act
        _tagParser.Parse(processedArticle, "This is a message",  "this-is-a-slug");

        // Assert
        Assert.Equal("default body", processedArticle.Body);
    }

    [Fact]
    public void ShouldRenderMessage()
    {
        // Arrabge
        ProcessedArticle processedArticle = new ("title",
                                                "slug",
                                                "default body",
                                                "teaser",
                                                "meta description",
                                                new List<ProcessedSection>() { },
                                                "icon",
                                                "backgroundImage",
                                                "image",
                                                "alt",
                                                new List<Crumb>(),
                                                new List<Alert>(),
                                                DefaultTopic(),
                                                new List<Alert>(),
                                                new DateTime(),
                                                new bool(),
                                                null,
                                                "logo",
                                                null,
                                                string.Empty,
                                                string.Empty,
                                                new DateTime(),
                                                new List<InlineQuote>(),
                                                new List<Event>(),
                                                string.Empty,
                                                new JObject());

        // Act
        _tagParser.Parse(processedArticle, "This is a message", "this-is-a-slug");

        // Assert
        _viewRenderer.Verify(renderer => renderer.Render("ContactUsMessage", "This is a message"), Times.Once);
    }

    private static ProcessedSection ProcessedSectionWithDefaultSlugAndBody(string slug = "slug", string body = "default body", string metaDescription = "default meta description") 
        => new("title",
                slug,
                metaDescription,
                body,
                new List<Profile>(),
                null,
                new List<Alert>(),
                new List<TrustedLogo>(),
                "logoAreaTitle",
                new DateTime(),
                null);

    private static Topic DefaultTopic() =>
        new("name",
            "slug",
            "summary",
            "teaser",
            "metaDescription",
            "icon",
            "backgroundImage",
            "image",
            new List<SubItem>(),
            new List<SubItem>(),
            new List<SubItem>(),
            new List<Crumb>(),
            new List<Alert>(),
            null,
            true,
            new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty, new DateTime()),
            string.Empty,
            null,
            string.Empty);
}