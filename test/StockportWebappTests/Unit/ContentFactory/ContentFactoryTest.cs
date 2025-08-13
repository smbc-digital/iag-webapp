using Newtonsoft.Json.Linq;

namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class ContentFactoryTest
{
    private readonly ContentTypeFactory _factory;

    public ContentFactoryTest()
    {
        Mock<IRepository> repository = new();
        Mock<ITagParserContainer> tagParserContainer = new();
        Mock<IHttpContextAccessor> httpContextAccessor = new();
        tagParserContainer
            .Setup(parser => parser.ParseAll(It.IsAny<string>(),
                                            It.IsAny<string>(),
                                            It.IsAny<bool>(),
                                            null,
                                            null,
                                            null,
                                            null,
                                            null,
                                            null,
                                            It.IsAny<bool>()))
            .Returns(string.Empty);

        _factory = new ContentTypeFactory(tagParserContainer.Object, new MarkdownWrapper(), httpContextAccessor.Object, repository.Object);
    }

    [Fact]
    public void ItUsesSectionFactoryToBuildProcessedSectionFromSection()
    {
        // Arrange
        Section section = new(TextHelper.AnyString,
                            TextHelper.AnyString,
                            TextHelper.AnyString,
                            TextHelper.AnyString,
                            new List<Profile>(),
                            new List<Document>(),
                            new List<Alert>(),
                            new List<TrustedLogo>(),
                            "logoAreaTitle",
                            new DateTime());

        // Act
        IProcessedContentType processedSection = _factory.Build(section);

        // Assert
        Assert.IsType<ProcessedSection>(processedSection);
    }

    [Fact]
    public void ItUsesArticleFactoryToBuildProcessedArticleFromArticle()
    {
        // Arrange
        Article article = new(TextHelper.AnyString,
                            TextHelper.AnyString,
                            TextHelper.AnyString,
                            TextHelper.AnyString,
                            TextHelper.AnyString,
                            new List<Section>(),
                            TextHelper.AnyString,
                            TextHelper.AnyString,
                            TextHelper.AnyString,
                            TextHelper.AnyString,
                            new List<Crumb>(),
                            new List<Profile>(),
                            new List<Document>(),
                            new List<Alert>(),
                            new DateTime(),
                            new bool(),
                            new List<TrustedLogo>(),
                            TextHelper.AnyString,
                            null,
                            It.IsAny<string>(),
                            It.IsAny<string>(),
                            new DateTime(),
                            new List<InlineQuote>(),
                            It.IsAny<string>(),
                            new List<CallToActionBanner>(),
                            new List<Event>(),
                            It.IsAny<string>(),
                            new JObject());

        // Act
        IProcessedContentType processedArticle = _factory.Build(article);

        // Assert
        Assert.IsType<ProcessedArticle>(processedArticle);
    }

    [Fact]
    public void ItUsesNewsFactoryToBuildProcessedNewsFromNews()
    {
        // Arrange
        News news = new(It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        new List<Crumb>(),
                        new DateTime(),
                        It.IsAny<string>(),
                        new DateTime(),
                        new DateTime(),
                        new List<Alert>(),
                        new List<string>(),
                        new List<Document>(),
                        new List<Profile>(),
                        new List<InlineQuote>(),
                        null,
                        It.IsAny<string>(),
                        new List<TrustedLogo>(),
                        null,
                        It.IsAny<string>(),
                        null);

        // Act
        IProcessedContentType processedNews= _factory.Build(news);

        // Assert
        Assert.IsType<ProcessedNews>(processedNews);
    }
}