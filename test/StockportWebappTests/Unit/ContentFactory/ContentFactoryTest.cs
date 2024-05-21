namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class ContentFactoryTest
{
    private readonly ContentTypeFactory _factory;

    public ContentFactoryTest()
    {
        var tagParserContainer = new Mock<ITagParserContainer>();
        var profileTagParser = new Mock<IDynamicTagParser<Profile>>();
        var documentTagParser = new Mock<IDynamicTagParser<Document>>();
        var alertsInlineTagParser = new Mock<IDynamicTagParser<Alert>>();
        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var s3BucketParser = new Mock<IDynamicTagParser<S3BucketSearch>>();
        var privacyNoticeTagParser = new Mock<IDynamicTagParser<PrivacyNotice>>();
        tagParserContainer.Setup(o => o.ParseAll(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), null, null, null, null, null, null)).Returns("");
        s3BucketParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<S3BucketSearch>>())).Returns("");

        _factory = new ContentTypeFactory(tagParserContainer.Object, new MarkdownWrapper(), httpContextAccessor.Object);
    }

    [Fact]
    public void ItUsesSectionFactoryToBuildProcessedSectionFromSection()
    {
        var section = new Section(TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Profile>(), new List<Document>(), new List<Alert>());

        var processedSection = _factory.Build<Section>(section);

        processedSection.Should().BeOfType<ProcessedSection>();
    }

    [Fact]
    public void ItUsesArticleFactoryToBuildProcessedArticleFromArticle()
    {
        var article = new Article(TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString,
            new List<Section>(), TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Crumb>(), new List<Profile>(), new List<Document>(), new List<Alert>(), new DateTime(), new bool());

        var processedArticle = _factory.Build<Article>(article);

        processedArticle.Should().BeOfType<ProcessedArticle>();
    }

    [Fact]
    public void ItUsesNewsFactoryToBuildProcessedNewsFromNews()
    {
        var news = new News(TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Crumb>(), new DateTime(), new DateTime(), new DateTime(), new List<Alert>(), new List<string>(), new List<Document>(), new List<Profile>());

        var processed = _factory.Build<News>(news);

        processed.Should().BeOfType<ProcessedNews>();
    }
}