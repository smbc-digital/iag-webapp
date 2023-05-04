using StockportWebapp.TagParsers;

namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class NewsFactoryTest
{
    private readonly NewsFactory _factory;
    private readonly Mock<MarkdownWrapper> _markdownWrapper;
    private readonly Mock<ISimpleTagParserContainer> _tagParserContainer;
    private readonly Mock<IDynamicTagParser<Document>> _documentTagParser;
    private readonly News _news;
    private const string Title = "News 26th Aug";
    private const string Slug = "news-26th-aug";
    private const string Teaser = "teaser";
    private const string Purpose = "purpose";
    private const string Image = "image";
    private const string ThumbnailImage = "image";
    private const string Body = "body";
    private readonly List<Crumb> _breadcrumbs = new List<Crumb>();
    private readonly DateTime _sunrise = new DateTime(2015, 9, 19);
    private readonly DateTime _sunset = new DateTime(2015, 9, 25);
    private readonly List<Alert> _alerts = new List<Alert>() { new Alert("Alert", "Sub heading", "The Body", "Error", new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                                             new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc), string.Empty, false) };
    private readonly List<string> _tags = new List<string> { "Events", "Bramall Hall" };
    private readonly List<Document> _documents = new List<Document>();

    public NewsFactoryTest()
    {
        _markdownWrapper = new Mock<MarkdownWrapper>();
        _tagParserContainer = new Mock<ISimpleTagParserContainer>();
        _documentTagParser = new Mock<IDynamicTagParser<Document>>();
        _factory = new NewsFactory(_tagParserContainer.Object, _markdownWrapper.Object, _documentTagParser.Object);
        _news = new News(Title, Slug, Teaser, Purpose, Image, ThumbnailImage, Body, _breadcrumbs, _sunrise, _sunset, _alerts, _tags, _documents);

        _tagParserContainer.Setup(o => o.ParseAll(Body, It.IsAny<string>(), It.IsAny<bool>())).Returns(Body);
        _markdownWrapper.Setup(o => o.ConvertToHtml(Body)).Returns(Body);
    }

    [Fact]
    public void ShouldSetTheCorrespondingFieldsForAProcessedNews()
    {
        var result = _factory.Build(_news);

        result.Title.Should().Be(Title);
        result.Slug.Should().Be(Slug);
        result.Teaser.Should().Be(Teaser);
        result.Image.Should().Be(Image);
        result.ThumbnailImage.Should().Be(ThumbnailImage);
        result.Breadcrumbs.Should().BeEquivalentTo(_breadcrumbs);
        result.SunriseDate.Should().Be(_sunrise);
        result.SunsetDate.Should().Be(_sunset);
        result.Alerts.Should().BeEquivalentTo(_alerts);
        result.Tags.Should().BeEquivalentTo(_tags);
    }

    [Fact]
    public void ShouldProcessBodyWithMarkdown()
    {
        _factory.Build(_news);

        _markdownWrapper.Verify(o => o.ConvertToHtml(Body), Times.Once);
    }

    [Fact]
    public void ShouldProcessBodyWithTagParsing()
    {
        _factory.Build(_news);

        _tagParserContainer.Verify(o => o.ParseAll(Body, It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    public void ShouldPassTitleToParserWhenBuilding()
    {
        _factory.Build(_news);

        _tagParserContainer.Verify(o => o.ParseAll(Body, _news.Title, It.IsAny<bool>()), Times.Once);
    }
}
