namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class PrivacyNoticeFactoryTest
{
    private readonly PrivacyNoticeFactory _factory;
    private readonly Mock<MarkdownWrapper> _markdownWrapper;

    public PrivacyNoticeFactoryTest()
    {
        _markdownWrapper = new Mock<MarkdownWrapper>();
        _factory = new PrivacyNoticeFactory(_markdownWrapper.Object);
    }

    [Fact]
    public void Build_ShouldReturnAPrivacyNotice()
    {
        // Arrange
        PrivacyNotice privacyNotice = new();

        // Act
        ProcessedPrivacyNotice processedPrivacyNotice = _factory.Build(privacyNotice);
        
        // Assert
        Assert.IsType<ProcessedPrivacyNotice>(processedPrivacyNotice);
    }

    [Fact]
    public void Build_ShouldConvertPrivacyNoticeToProcessedPrivacyNotice()
    {
        // Arrange
        _markdownWrapper.Setup(_ => _.ConvertToHtml("test-type-of-data")).Returns("test-type-of-data-html");
        _markdownWrapper.Setup(_ => _.ConvertToHtml("test-purpose")).Returns("test-purpose-html");
        _markdownWrapper.Setup(_ => _.ConvertToHtml("test-externally-shared")).Returns("test-externally-shared-html");
        _markdownWrapper.Setup(_ => _.ConvertToHtml("test-obtained")).Returns("test-obtained-html");
        _markdownWrapper.Setup(_ => _.ConvertToHtml("test-retention")).Returns("test-retention-html");
        _markdownWrapper.Setup(_ => _.ConvertToHtml("test-legislation")).Returns("test-legislation-html");

        PrivacyNotice privacyNotice = new()
        {
            Slug = "test-slug",
            Title = "test-title",
            Category = "test-categories",
            Purpose = "test-purpose",
            TypeOfData = "test-type-of-data",
            Legislation = "test-legislation",
            Obtained = "test-obtained",
            ExternallyShared = "test-externally-shared",
            RetentionPeriod = "test-retention",
            OutsideEu = false,
            AutomatedDecision = false,
            UrlOne = "test-url-1",
            UrlTwo = "test-url-2",
            UrlThree = "test-url-3",
            Breadcrumbs = new List<Crumb>()
        };

        //Act
        ProcessedPrivacyNotice processedPrivacyNotice = _factory.Build(privacyNotice);

        //Assert
        Assert.Equal("test-slug", processedPrivacyNotice.Slug);
        Assert.Equal("test-title", processedPrivacyNotice.Title);
        Assert.Equal("test-categories", processedPrivacyNotice.Category);
        Assert.Equal("test-purpose-html", processedPrivacyNotice.Purpose);
        Assert.Equal("test-type-of-data-html", processedPrivacyNotice.TypeOfData);
        Assert.Equal("test-legislation-html", processedPrivacyNotice.Legislation);
        Assert.Equal("test-obtained-html", processedPrivacyNotice.Obtained);
        Assert.Equal("test-externally-shared-html", processedPrivacyNotice.ExternallyShared);
        Assert.Equal("test-retention-html", processedPrivacyNotice.RetentionPeriod);
        Assert.False(processedPrivacyNotice.OutsideEu);
        Assert.False(processedPrivacyNotice.AutomatedDecision);
        Assert.Equal("test-url-1", processedPrivacyNotice.UrlOne);
        Assert.Equal("test-url-2", processedPrivacyNotice.UrlTwo);
        Assert.Equal("test-url-3", processedPrivacyNotice.UrlThree);
    }
}