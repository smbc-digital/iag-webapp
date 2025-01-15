namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class GroupFactoryTest
{
    private readonly GroupFactory _factory;
    private readonly Mock<MarkdownWrapper> _markdownWrapper = new();
    private readonly Mock<ITagParserContainer> _tagParserContainer = new();
    private readonly Group _group;

    public GroupFactoryTest()
    {
        _factory = new GroupFactory(_tagParserContainer.Object, _markdownWrapper.Object);
        _group = new GroupBuilder().Build();

        _tagParserContainer
            .Setup(parser => parser.ParseAll(_group.Description,
                                            It.IsAny<string>(),
                                            It.IsAny<bool>(),
                                            null,
                                            null,
                                            null,
                                            null,
                                            null,
                                            It.IsAny<bool>()))
            .Returns(_group.Description);
        
        _tagParserContainer
            .Setup(parser => parser.ParseAll(_group.AdditionalInformation,
                                            It.IsAny<string>(),
                                            It.IsAny<bool>(),
                                            null,
                                            null,
                                            null,
                                            null,
                                            null,
                                            It.IsAny<bool>()))
            .Returns(_group.AdditionalInformation);

        _markdownWrapper
            .Setup(wrapper => wrapper.ConvertToHtml(_group.Description))
            .Returns(_group.Description);
    }

    [Fact]
    public void ShouldSetTheCorrespondingFieldsForAProcessedGroup()
    {
        // Act
        ProcessedGroup result = _factory.Build(_group);

        // Assert
        Assert.Equal(_group.Name, result.Name);
        Assert.Equal(_group.MetaDescription, result.MetaDescription);
        Assert.Equal(_group.Description, result.Description);
        Assert.Equal(_group.Slug, result.Slug);
        Assert.Equal(_group.Address, result.Address);
        Assert.Equal(_group.Website, result.Website);
        Assert.Equal(_group.Email, result.Email);
        Assert.Equal(_group.PhoneNumber, result.PhoneNumber);
        Assert.Equal(_group.ImageUrl, result.ImageUrl);
        Assert.Equal(_group.MapPosition.Lat, result.MapDetails.MapPosition.Lat);
        Assert.Equal(_group.MapPosition.Lon, result.MapDetails.MapPosition.Lon);
        Assert.Equal(_group.Twitter, result.Twitter);
        Assert.Equal(_group.Facebook, result.Facebook);
    }

    [Fact]
    public void ShouldProcessDescriptionWithMarkdown()
    {
        // Act
        _factory.Build(_group);

        // Assert
        _markdownWrapper.Verify(wrapper => wrapper.ConvertToHtml(_group.Description), Times.Once);
    }

    [Fact]
    public void ShouldPassTitleToAllSimpleParsersWhenBuilding()
    {
        // Act
        _factory.Build(_group);

        // Assert
        _tagParserContainer.Verify(parser => parser.ParseAll(_group.Description,
                                                            _group.Name,
                                                            It.IsAny<bool>(),
                                                            null,
                                                            null,
                                                            null,
                                                            null,
                                                            null,
                                                            It.IsAny<bool>()), Times.Once);
    }
}