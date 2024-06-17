using SharpKml.Dom;
using StockportWebapp.Models;
using System.Collections.Generic;

namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class StartPageFactoryTests
{
    private readonly Mock<MarkdownWrapper> _mockMarkdownWrapper = new Mock<MarkdownWrapper>();
    private readonly Mock<ITagParserContainer> _mockTagParser = new Mock<ITagParserContainer>();
    private readonly StartPageFactory _factory;
    private readonly StartPage _startPage;

    public StartPageFactoryTests()
    {
        _factory = new StartPageFactory(_mockTagParser.Object, _mockMarkdownWrapper.Object);
        _startPage = new StartPage("test-start-page", "Test start page", "This is a test start page", "Use this page to start test processes", "Test upper body content", "Test Link",
        "https://www.stockport.gov.uk", "Test lower body content", new List<Crumb>(), string.Empty,
        "fa-test", new List<Alert>(), new List<Alert>());

        _mockTagParser
            .Setup(_ => _.ParseAll(_startPage.UpperBody, It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<IEnumerable<Alert>>(), null, null, null, null))
            .Returns(_startPage.UpperBody);

        _mockTagParser
            .Setup(_ => _.ParseAll(_startPage.LowerBody, It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<IEnumerable<Alert>>(), null, null, null, null))
            .Returns(_startPage.LowerBody);

        _mockMarkdownWrapper
            .Setup(_ => _.ConvertToHtml(_startPage.UpperBody))
            .Returns(_startPage.UpperBody);

        _mockMarkdownWrapper
            .Setup(_ => _.ConvertToHtml(_startPage.LowerBody))
            .Returns(_startPage.LowerBody);
    }


    [Fact]
    public void ShouldSetTheCorrespondingFieldsForAProcessedServicePayPayment()
    {
        var result = _factory.Build(_startPage);

        result.Slug.Should().Be(_startPage.Slug);
        result.Title.Should().Be(_startPage.Title);
        result.Teaser.Should().Be(_startPage.Teaser);
        result.Summary.Should().Be(_startPage.Summary);
        result.UpperBody.Should().Be(_startPage.UpperBody);
        result.FormLinkLabel.Should().Be(_startPage.FormLinkLabel);
        result.FormLink.Should().Be(_startPage.FormLink);
        result.LowerBody.Should().Be(_startPage.LowerBody);
        result.Breadcrumbs.Should().BeEquivalentTo(_startPage.Breadcrumbs);
        result.BackgroundImage.Should().Be(_startPage.BackgroundImage);
        result.Icon.Should().Be(_startPage.Icon);
        result.Alerts.Should().BeEquivalentTo(_startPage.Alerts);
    }

    [Fact]
    public void ShouldCallMarkdownWrapper()
    {
        _factory.Build(_startPage);
        _mockMarkdownWrapper.Verify(_ => _.ConvertToHtml(It.IsAny<string>()), Times.Exactly(2));
    }

    [Fact]
    public void ShouldCallTagParser()
    {
        _factory.Build(_startPage);
        _mockTagParser.Verify(_ => _.ParseAll(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<IEnumerable<Alert>>(), null, null, null, null), Times.Exactly(2));
    }
}
