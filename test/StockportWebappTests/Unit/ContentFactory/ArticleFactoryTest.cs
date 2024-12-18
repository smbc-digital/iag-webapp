using Document = StockportWebapp.Models.Document;

namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class ArticleFactoryTest
{
    private readonly ArticleFactory _articleFactory;
    private readonly Mock<ITagParserContainer> _tagParserContainer = new();
    private readonly Mock<ISectionFactory> _sectionFactory = new();
    private readonly Mock<MarkdownWrapper> _markdownWrapper = new();
    private readonly Mock<IRepository> _repository = new();
    private const string Body = "<p>{{PrivacyNotice:test}}body</p>";
    private readonly Section _sectionOne;
    private readonly ProcessedSection _processedSectionOne;
    private readonly Section _sectionTwo;
    private readonly ProcessedSection _processedSectionTwo;
    private readonly Article _article;

    public ArticleFactoryTest()
    {
        _articleFactory = new(_tagParserContainer.Object, _sectionFactory.Object, _markdownWrapper.Object, _repository.Object);
        _sectionOne = new(It.IsAny<string>(), "id-1", It.IsAny<string>(), It.IsAny<string>(), new List<Profile>(), new List<Document>(), new List<Alert>(), new List<GroupBranding>(), "logoAreaTitle", new DateTime());
        _processedSectionOne = new(It.IsAny<string>(), It.IsAny<string>(), "id-1", It.IsAny<string>(), new List<Profile>(), new List<Document>(), new List<Alert>(), new List<GroupBranding>(), "logoAreaTitle", new DateTime());
        _sectionTwo = new(It.IsAny<string>(), "id-1", It.IsAny<string>(), It.IsAny<string>(), new List<Profile>(), new List<Document>(), new List<Alert>(), new List<GroupBranding>(), "logoAreaTitle", new DateTime());
        _processedSectionTwo = new(It.IsAny<string>(), "id-1", It.IsAny<string>(), It.IsAny<string>(), new List<Profile>(), new List<Document>(), new List<Alert>(), new List<GroupBranding>(), "logoAreaTitle", new DateTime());

        List<Section> sections = new(){ _sectionOne, _sectionTwo };

        _article = new("title",
                    "slug",
                    Body,
                    "teaser",
                    "meta description",
                    sections,
                    "icon",
                    "backgroundImage",
                    "image",
                    "altText",
                    new List<Crumb>(),
                    new List<Profile>(),
                    new List<Document>(),
                    new List<Alert>(),
                    new DateTime(),
                    false,
                    new List<GroupBranding>(),
                    "logoAreaTitle",
                    new List<SubItem>(),
                    "author",
                    "photographer",
                    new DateTime(),
                    new List<InlineQuote>(),
                    "dance",
                    new List<Event>());

        _sectionFactory
            .Setup(_ => _.Build(_sectionOne, _article.Title))
            .Returns(_processedSectionOne);

        _repository
            .Setup(_ => _.Get<List<PrivacyNotice>>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, new List<PrivacyNotice>(), string.Empty));

        _markdownWrapper
            .Setup(_ => _.ConvertToHtml(Body))
            .Returns(Body);
        
        _tagParserContainer
            .Setup(_ => _.ParseAll(Body,
                                   It.IsAny<string>(),
                                   It.IsAny<bool>(),
                                   It.IsAny<IEnumerable<Alert>>(),
                                   It.IsAny<IEnumerable<Document>>(),
                                   It.IsAny<IEnumerable<InlineQuote>>(),
                                   It.IsAny<IEnumerable<PrivacyNotice>>(),
                                   It.IsAny<IEnumerable<Profile>>(),
                                   It.IsAny<bool>()))
            .Returns(Body);
    }

    [Fact]
    public void Build_ShouldSetTheCorrespondingFieldsForAProcessedArticle()
    {
        // Arrange
        _sectionFactory.Setup(_ => _.Build(_sectionTwo, _article.Title)).Returns(_processedSectionTwo);

        // Act
        ProcessedArticle result = _articleFactory.Build(_article);

        // Assert
        Assert.Equal("title", result.Title);
        Assert.Equal("/slug", result.NavigationLink);
        Assert.Equal(Body, result.Body);
        Assert.Equal("teaser", result.Teaser);
        Assert.Equal("meta description", result.MetaDescription);
        Assert.Equal(2, result.Sections.Count());
        Assert.Equal(_processedSectionOne, result.Sections.ToList().First());
        Assert.Equal(_processedSectionTwo, result.Sections.ToList()[1]);
        Assert.Equal("icon", result.Icon);
        Assert.Equal("backgroundImage", result.BackgroundImage);
        Assert.Equal("image", result.Image);
        Assert.Equal(new List<Crumb>(), result.Breadcrumbs);
    }

    [Fact]
    public void Build_ShouldProcessAllSectionsInArticle()
    {
        // Act
        _articleFactory.Build(_article);
        
        // Assert
        _sectionFactory.Verify(_ => _.Build(_sectionOne, _article.Title), Times.Once);
        _sectionFactory.Verify(_ => _.Build(_sectionTwo, _article.Title), Times.Once);
    }

    [Fact]
    public void Build_ShouldProcessBodyWithMarkdown()
    {
        // Act
        _articleFactory.Build(_article);

        // Assert
        _markdownWrapper.Verify(_ => _.ConvertToHtml(Body), Times.Once);
    }

    [Fact]
    public void Build_ShouldProcessBodyWithStaticTagParsing()
    {
        // Act
        _articleFactory.Build(_article);

        // Assert
        _tagParserContainer.Verify(_ => _.ParseAll(Body,
                                                It.IsAny<string>(),
                                                It.IsAny<bool>(),
                                                It.IsAny<IEnumerable<Alert>>(),
                                                It.IsAny<IEnumerable<Document>>(),
                                                It.IsAny<IEnumerable<InlineQuote>>(),
                                                It.IsAny<IEnumerable<PrivacyNotice>>(),
                                                It.IsAny<IEnumerable<Profile>>(),
                                                It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    public void Build_ShouldPassTitleToParserWhenBuilding()
    {
        // Act
        _articleFactory.Build(_article);
        
        // Assert
        _tagParserContainer.Verify(_ => _.ParseAll(Body,
                                                _article.Title,
                                                It.IsAny<bool>(),
                                                It.IsAny<IEnumerable<Alert>>(),
                                                It.IsAny<IEnumerable<Document>>(),
                                                It.IsAny<IEnumerable<InlineQuote>>(),
                                                It.IsAny<IEnumerable<PrivacyNotice>>(),
                                                It.IsAny<IEnumerable<Profile>>(),
                                                It.IsAny<bool>()), Times.Once);
    }
}