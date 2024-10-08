﻿namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class GroupHomepageFactoryTest
{
    private readonly Mock<MarkdownWrapper> _markdownWrapperMock = new Mock<MarkdownWrapper>();
    private readonly Mock<ITagParserContainer> _tagParserContainerMock = new Mock<ITagParserContainer>();
    private readonly GroupHomepageFactory _groupHomepageFactory;
    private const string Title = "title";
    private string Body = "body";
    private GroupHomepage _groupHomepage;


    public GroupHomepageFactoryTest()
    {
        _groupHomepageFactory = new GroupHomepageFactory(_tagParserContainerMock.Object, _markdownWrapperMock.Object);


        _groupHomepage = new GroupHomepage
        {
            Title = Title,
            BackgroundImage = "background image",
            FeaturedGroupsHeading = string.Empty,
            FeaturedGroups = new List<Group>(),
            FeaturedGroupsCategory = new GroupCategory(),
            FeaturedGroupsSubCategory = new GroupSubCategory(),
            Alerts = new List<Alert>(),
            Body = "body",
            SecondaryBody = "secondary body",
            EventBanner = new EventBanner("title", "teaser", "icon", "link")
        };

        _markdownWrapperMock.Setup(o => o.ConvertToHtml(Body)).Returns(Body);
        _tagParserContainerMock.Setup(o => o.ParseAll(Body, It.IsAny<string>(), It.IsAny<bool>(), null, null, null, null, null, It.IsAny<bool>())).Returns(Body);
    }

    [Fact]
    public void ItBuildsAGroupsHomepageWithProcessedBody()
    {
        var result = _groupHomepageFactory.Build(_groupHomepage);

        result.Title.Should().Be(_groupHomepage.Title);
        result.BackgroundImage.Should().Be(_groupHomepage.BackgroundImage);
        result.Body.Should().Be(_groupHomepage.Body);
    }

    [Fact]
    public void ShouldParseAllOfBody()
    {
        var result = _groupHomepageFactory.Build(_groupHomepage);

        _tagParserContainerMock.Verify(o => o.ParseAll(Body, It.IsAny<string>(), It.IsAny<bool>(), null, null, null, null, null, It.IsAny<bool>()), Times.Once);
        _markdownWrapperMock.Verify(o => o.ConvertToHtml(Body), Times.Once);
    }

}