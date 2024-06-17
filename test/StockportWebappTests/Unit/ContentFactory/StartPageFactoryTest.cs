namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class StartPageFactoryTests
{
    private readonly Mock<MarkdownWrapper> _mockMarkdownWrapper = new Mock<MarkdownWrapper>();
    private readonly Mock<ITagParserContainer> _mockTagParser = new Mock<ITagParserContainer>();
    private readonly StartPageFactory _factory;

    public StartPageFactoryTests()
    {
        _factory = new StartPageFactory(_mockTagParser.Object, _mockMarkdownWrapper.Object);

        _mockTagParser
            .Setup(_ => _.ParseAll(_payment.Description, It.IsAny<string>(), It.IsAny<bool>(), null, null, null, null, null))
            .Returns(_payment.Description);

        _mockMarkdownWrapper
            .Setup(_ => _.ConvertToHtml(_payment.Description))
            .Returns(_payment.Description);
    }

    [Fact]
    public void ShouldSetTheCorrespondingFieldsForAProcessedServicePayPayment()
    {
        var result = _factory.Build(_payment);

        result.Title.Should().Be(_payment.Title);
        result.Teaser.Should().Be(_payment.Teaser);
        result.ReturnUrl.Should().Be(_payment.ReturnUrl);
        result.ReferenceValidation.Should().Be(_payment.ReferenceValidation);
        result.ReferenceLabel.Should().Be(_payment.ReferenceLabel);
        result.Slug.Should().Be(_payment.Slug);
        result.PaymentDetailsText.Should().Be(_payment.PaymentDetailsText);
        result.PaymentAmount.Should().Be(_payment.PaymentAmount);
        result.PaymentDescription.Should().Be(_payment.PaymentDescription);
        result.MetaDescription.Should().Be(_payment.MetaDescription);
        result.Description.Should().Be(_payment.Description);
        result.CatalogueId.Should().Be(_payment.CatalogueId);
        result.Breadcrumbs.Should().BeEquivalentTo(_payment.BreadCrumbs);
        result.Alerts.Should().BeEquivalentTo(_payment.Alerts);
        result.AccountReference.Should().Be(_payment.AccountReference);
    }

    [Fact]
    public void ShouldCallMarkdownWrapper()
    {
        _factory.Build(_payment);

        _mockMarkdownWrapper.Verify(_ => _.ConvertToHtml(_payment.Description), Times.Once);
    }

    [Fact]
    public void ShouldCallTagParser()
    {
        _factory.Build(_payment);
        _mockTagParser.Verify(_ => _.ParseAll(_payment.Description, _payment.Title, It.IsAny<bool>(), null, null, null, null, null), Times.Once);
    }
}
