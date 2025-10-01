namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class PaymentFactoryTest
{
    private readonly Mock<MarkdownWrapper> _markdownWrapper = new();
    private readonly Mock<ITagParserContainer> _tagParserContainer = new();
    private readonly Payment _payment;
    private readonly PaymentFactory _factory;

    public PaymentFactoryTest()
    {
        _factory = new PaymentFactory(_tagParserContainer.Object, _markdownWrapper.Object);
        _payment = new Payment()
        {
            Title = "Pay your council Tax",
            Slug = "council-tax",
            Description = "Description",
            PaymentDetailsText = "Payment Details Text",
            Fund = "Fund",
            ReferenceLabel = "Reference label",
            GlCodeCostCentreNumber = "1234",
            BreadCrumbs = new List<Crumb>(),
            MetaDescription = "Meta description"
        };

        _tagParserContainer
            .Setup(parser => parser.ParseAll("Description",
                                            It.IsAny<string>(),
                                            It.IsAny<bool>(),
                                            null,
                                            null,
                                            null,
                                            null,
                                            null,
                                            null,
                                            It.IsAny<bool>()))
            .Returns("Description");

        _markdownWrapper
            .Setup(wrapper => wrapper.ConvertToHtml("Description"))
            .Returns("Description");
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
        // Act
        _factory.Build(_payment);
        
        // Assert
        _markdownWrapper.Verify(wrapper => wrapper.ConvertToHtml("Description"), Times.Once);
    }

    [Fact]
    public void ShouldPassTitleToAllSimpleParsersWhenBuilding()
    {
        // Act
        _factory.Build(_payment);

        // Assert
        _tagParserContainer.Verify(parser => parser.ParseAll("Description",
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