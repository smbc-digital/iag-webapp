namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class PaymentFactoryTest
{
    private readonly Mock<MarkdownWrapper> _markdownWrapper = new();
    private readonly Mock<ITagParserContainer> _tagParserContainer = new();
    private readonly Payment _payment;
    private readonly PaymentFactory _factory;
    private readonly string Title = "Pay your council Tax";
    private readonly string Slug = "council-tax";
    private readonly string Description = "Description";
    private readonly string PaymentDetailsText = "Payment Details Text";
    private readonly string GlCodeCostCentreNumber = "1234";
    private readonly string ParisReference = "Paris reference";
    private readonly string Fund = "Fund";
    private readonly string ReferenceLabel = "Reference label";
    private readonly string MetaDescription = "Meta description";

    public PaymentFactoryTest()
    {
        _factory = new PaymentFactory(_tagParserContainer.Object, _markdownWrapper.Object);
        _payment = new Payment()
        {
            Title = Title,
            Slug = Slug,
            Description = Description,
            PaymentDetailsText = PaymentDetailsText,
            Fund = Fund,
            ReferenceLabel = ReferenceLabel,
            GlCodeCostCentreNumber = GlCodeCostCentreNumber,
            BreadCrumbs = new List<Crumb>(),
            MetaDescription = MetaDescription
        };

        _tagParserContainer
            .Setup(parser => parser.ParseAll(Description,
                                            It.IsAny<string>(),
                                            It.IsAny<bool>(),
                                            null,
                                            null,
                                            null,
                                            null,
                                            null,
                                            null,
                                            It.IsAny<bool>()))
            .Returns(Description);

        _markdownWrapper
            .Setup(wrapper => wrapper.ConvertToHtml(Description))
            .Returns(Description);
    }

    [Fact]
    public void ShouldSetTheCorrespondingFieldsForAProcessedPayment()
    {
        // Act
        ProcessedPayment result = _factory.Build(_payment);

        // Assert
        Assert.Equal("Pay your council Tax", result.Title);
        Assert.Equal("Description", result.Description);
        Assert.Equal("council-tax", result.Slug);
        Assert.Equal("Meta description", result.MetaDescription);
    }

    [Fact]
    public void ShouldProcessDescriptionWithMarkdown()
    {
        // Act & Assert
        _factory.Build(_payment);
        _markdownWrapper.Verify(wrapper => wrapper.ConvertToHtml(Description), Times.Once);
    }

    [Fact]
    public void ShouldPassTitleToAllSimpleParsersWhenBuilding()
    {
        // Act & Assert
        _factory.Build(_payment);
        _tagParserContainer.Verify(parser => parser.ParseAll(Description,
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