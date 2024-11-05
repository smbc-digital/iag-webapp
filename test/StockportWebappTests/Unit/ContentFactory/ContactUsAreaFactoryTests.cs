namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class ContactUsAreaFactoryTests
{
    private readonly Mock<IContactUsCategoryFactory> _mockContactUsCategoryFactory = new();

    private readonly ContactUsAreaFactory _factory;

    private readonly ContactUsArea _contactUsArea = new("title",
        "slug",
        "categories title",
        new List<Crumb>
        {
            new("title", "slug", "type")
        },
        new List<Alert>
        {
            new("title", "subHeading", "body", "severity", DateTime.Now, DateTime.Now, "slug", true, "imageUrl")
        },
        new List<SubItem>
        {
            new("slug", "title", "teaser", "teaser image", "icon", "type", "image", new List<SubItem>(), EColourScheme.Blue)
        },
        new List<ContactUsCategory>
        {
            new("title", "bodyTextLeft", "bodyTextRight", "icon"),
            new("title", "bodyTextLeft", "bodyTextRight", "icon")
        },
        "inset text title",
        "inset text body",
        "meta description");

    public ContactUsAreaFactoryTests()
    {
        _mockContactUsCategoryFactory
            .Setup(factory => factory.Build(It.IsAny<ContactUsCategory>()))
            .Returns(new ProcessedContactUsCategory("title", "bodyTextLeft", "bodyTextRight", "icon"));

        _factory = new ContactUsAreaFactory(_mockContactUsCategoryFactory.Object);
    }

    [Fact]
    public void Build_ShouldReturnCorrectProcessedContactUsArea()
    {
        // Act
        ProcessedContactUsArea result = _factory.Build(_contactUsArea);

        // Assert
        Assert.Equal(_contactUsArea.Title, result.Title);
        Assert.Equal(_contactUsArea.Slug, result.Slug);
        Assert.Equal(_contactUsArea.CategoriesTitle, result.CategoriesTitle);
        Assert.Single(result.Breadcrumbs);
        Assert.Single(result.PrimaryItems);
        Assert.Single(result.Alerts);
        Assert.Equal(2, result.ContactUsCategories.Count());
        Assert.Equal(_contactUsArea.InsetTextTitle, result.InsetTextTitle);
        Assert.Equal(_contactUsArea.InsetTextBody, result.InsetTextBody);
        Assert.Equal(_contactUsArea.MetaDescription, result.MetaDescription);
    }
}
