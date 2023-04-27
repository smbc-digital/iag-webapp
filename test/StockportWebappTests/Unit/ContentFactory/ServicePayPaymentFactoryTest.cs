namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class ServicePayPaymentFactoryTest
{
    private readonly Mock<MarkdownWrapper> _mockMarkdownWrapper = new Mock<MarkdownWrapper>();
    private readonly Mock<ISimpleTagParserContainer> _mockTagParser = new Mock<ISimpleTagParserContainer>();
    private readonly ServicePayPayment _payment = new ServicePayPayment
    {
        AccountReference = "123445",
        Alerts = null,
        BreadCrumbs = null,
        CatalogueId = "45334534",
        Description = "description",
        MetaDescription = "metaDescription",
        PaymentAmount = "20.36",
        PaymentDescription = "paymentDescription",
        PaymentDetailsText = "detailsText",
        Slug = "slug",
        ReferenceLabel = "Ref label",
        ReferenceValidation = EPaymentReferenceValidation.None,
        ReturnUrl = "returnUrl",
        Teaser = "teaser",
        Title = "title"
    };

    private readonly ServicePayPaymentFactory _factory;

    public ServicePayPaymentFactoryTest()
    {
        _factory = new ServicePayPaymentFactory(_mockTagParser.Object, _mockMarkdownWrapper.Object);

        _mockTagParser
            .Setup(_ => _.ParseAll(_payment.Description, It.IsAny<string>(), It.IsAny<bool>()))
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
        _mockTagParser.Verify(_ => _.ParseAll(_payment.Description, _payment.Title, It.IsAny<bool>()), Times.Once);
    }
}
