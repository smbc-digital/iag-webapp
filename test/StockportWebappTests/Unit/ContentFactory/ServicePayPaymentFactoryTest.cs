namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class ServicePayPaymentFactoryTests
{
    private readonly Mock<MarkdownWrapper> _mockMarkdownWrapper = new();
    private readonly Mock<ITagParserContainer> _mockTagParser = new();
    private readonly ServicePayPayment _payment = new()
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

    public ServicePayPaymentFactoryTests()
    {
        _factory = new ServicePayPaymentFactory(_mockTagParser.Object, _mockMarkdownWrapper.Object);

        _mockTagParser
            .Setup(_ => _.ParseAll(_payment.Description, It.IsAny<string>(), It.IsAny<bool>(), null, null, null, null, null, null, It.IsAny<bool>()))
            .Returns(_payment.Description);

        _mockMarkdownWrapper
            .Setup(_ => _.ConvertToHtml(_payment.Description))
            .Returns(_payment.Description);
    }

    [Fact]
    public void ShouldSetTheCorrespondingFieldsForAProcessedServicePayPayment()
    {
        // Act
        ProcessedServicePayPayment result = _factory.Build(_payment);

        // Assert
        Assert.Equal(_payment.Title, result.Title);
        Assert.Equal(_payment.Teaser, result.Teaser);
        Assert.Equal(_payment.ReferenceValidation, result.ReferenceValidation);
        Assert.Equal(_payment.ReferenceLabel, result.ReferenceLabel);
        Assert.Equal(_payment.Slug, result.Slug);
        Assert.Equal(_payment.PaymentDetailsText, result.PaymentDetailsText);
        Assert.Equal(_payment.PaymentAmount, result.PaymentAmount);
        Assert.Equal(_payment.PaymentDescription, result.PaymentDescription);
        Assert.Equal(_payment.MetaDescription, result.MetaDescription);
        Assert.Equal(_payment.Description, result.Description);
        Assert.Equal(_payment.CatalogueId, result.CatalogueId);
        Assert.Equal(_payment.BreadCrumbs, result.Breadcrumbs);
        Assert.Equal(_payment.Alerts, result.Alerts);
        Assert.Equal(_payment.AccountReference, result.AccountReference);
    }

    [Fact]
    public void ShouldCallMarkdownWrapper()
    {
        // Act & Assert
        _factory.Build(_payment);
        _mockMarkdownWrapper.Verify(wrapper => wrapper.ConvertToHtml(_payment.Description), Times.Once);
    }

    [Fact]
    public void ShouldCallTagParser()
    {
        // Act & Assert
        _factory.Build(_payment);
        _mockTagParser.Verify(parser => parser.ParseAll(_payment.Description,
                                                        _payment.Title,
                                                        It.IsAny<bool>(),
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        It.IsAny<bool>()), Times.Once);
    }
}