namespace StockportWebappTests_Unit.Unit.Controllers;

public class PublicationTemplateControllerTests
{
    private readonly PublicationTemplateController _publicationTemplateController;
    private readonly Mock<IPublicationTemplateRepository> _repository = new();
    private readonly Mock<IFeatureManager> _featureManager = new();
    
    private readonly PublicationTemplate publicationTemplate = new()
    {
        Slug = "slug",
        Title = "title",
        Subtitle = "subtitle",
        Breadcrumbs = new List<Crumb>(),
        MetaDescription = "metaDescription",
        HeaderImage = new MediaAsset(),
        PublicationPages = new List<PublicationPage>()
        {
            new()
            {
                Title = "page title",
                Slug = "page-slug",
                Body = new JsonElement()
            }
        }
    };

    public PublicationTemplateControllerTests()
    {
        _repository
            .Setup(repo => repo.Get(It.IsAny<string>()))
            .ReturnsAsync(HttpResponse.Successful(200, publicationTemplate));

         _featureManager
            .Setup(manager => manager.IsEnabledAsync("PublicationTemplate"))
            .Returns(Task.FromResult(true));

        _publicationTemplateController = new PublicationTemplateController(_repository.Object, _featureManager.Object);
    }

    [Fact]
    public async Task Index_ShouldReturnNotFoundStatusCode()
    {
        // Arrange
        _repository
            .Setup(repo => repo.Get(It.IsAny<string>()))
            .ReturnsAsync(HttpResponse.Failure(404, "Error"));

        // Act
        StatusCodeResult result = await _publicationTemplateController.Index("slug", null, null) as StatusCodeResult;
        
        // Assert
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task Index_ShouldReturnNotFound_WhenResponseContentIsNotPublicationTemplate()
    {
        // Arrange
        _repository
            .Setup(repo => repo.Get(It.IsAny<string>()))
            .ReturnsAsync(HttpResponse.Successful(200, new object()));

        // Act
        StatusCodeResult result = await _publicationTemplateController.Index("slug", null, null) as StatusCodeResult;

        // Assert
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task Index_ShouldReturnNotFound_WhenPublicationPageNotFound()
    {
        // Arrange
        publicationTemplate.PublicationPages = new List<PublicationPage>();
        PublicationTemplateViewModel expectedViewModel = new(publicationTemplate, new PublicationPage(), null);

        // Act
        StatusCodeResult result = await _publicationTemplateController.Index("slug", "page-slug", null) as StatusCodeResult;

        // Assert
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task Index_ShouldReturnView_WithPublicationTemplateViewModel()
    {
        // Arrange
        PublicationTemplateViewModel expectedViewModel = new(publicationTemplate, new PublicationPage(), null);

        // Act
        ViewResult result = await _publicationTemplateController.Index("slug", "page-slug", null) as ViewResult;
        PublicationTemplateViewModel actualViewModel = result.Model as PublicationTemplateViewModel;

        // Assert
        Assert.Equal("Index", result.ViewName);
        Assert.Equal(expectedViewModel.PublicationTemplate, actualViewModel.PublicationTemplate);
    }

    [Fact]
    public async Task Index_ShouldUseFirstPublicationPage_WhenPageSlugIsNull()
    {
        // Arrange
        PublicationPage firstPage = new()
        {
            Slug = "first-page",
            Title = "First",
            Body = new JsonElement()
        };
        
        PublicationPage secondPage = new()
        {
            Slug = "second-page",
            Title = "Second",
            Body = new JsonElement()
        };

        publicationTemplate.PublicationPages = new List<PublicationPage> { firstPage, secondPage };

        _repository
            .Setup(repo => repo.Get(It.IsAny<string>()))
            .ReturnsAsync(HttpResponse.Successful(200, publicationTemplate));

        // Act
        ViewResult result = await _publicationTemplateController.Index("slug", null, null) as ViewResult;
        PublicationTemplateViewModel model = result.Model as PublicationTemplateViewModel;

        // Assert
        Assert.Equal(firstPage, model.PublicationTemplate.PublicationPages.FirstOrDefault());
    }

    [Fact]
    public async Task Index_ShouldReturnView_WithMatchingSection_WhenSectionSlugMatchesCaseInsensitive()
    {
        // Arrange
        PublicationSection section = new()
        {
            Slug = "section-slug",
            Title = "Section title"
        };

        PublicationPage page = new()
        {
            Slug = "page-slug",
            Title = "page",
            PublicationSections = new List<PublicationSection> { section },
            Body = new JsonElement()
        };

        publicationTemplate.PublicationPages = new List<PublicationPage> { page };

        _repository
            .Setup(repo => repo.Get(It.IsAny<string>()))
            .ReturnsAsync(HttpResponse.Successful(200, publicationTemplate));

        // Act
        ViewResult result = await _publicationTemplateController.Index("slug", "page-slug", "SECTION-SLUG") as ViewResult;
        PublicationTemplateViewModel model = result.Model as PublicationTemplateViewModel;

        // Assert
        Assert.Equal(section, model.PublicationTemplate.PublicationPages.FirstOrDefault().PublicationSections.FirstOrDefault());
    }

    [Fact]
    public async Task Index_ShouldReturnView_WithNullSection_WhenSectionNotFound()
    {
        // Arrange
        PublicationPage page = new()
        {
            Slug = "page-slug",
            Title = "page",
            PublicationSections = new List<PublicationSection>(),
            Body = new JsonElement()
        };

        publicationTemplate.PublicationPages = new List<PublicationPage> { page };

        _repository
            .Setup(repo => repo.Get(It.IsAny<string>()))
            .ReturnsAsync(HttpResponse.Successful(200, publicationTemplate));

        // Act
        ViewResult result = await _publicationTemplateController.Index("slug", "page-slug", "nonexistent") as ViewResult;
        PublicationTemplateViewModel model = result.Model as PublicationTemplateViewModel;

        // Assert
        Assert.Null(model.PublicationTemplate.PublicationPages.FirstOrDefault().PublicationSections.FirstOrDefault());
    }

    [Fact]
    public async Task Index_ShouldSetCanonical_ToRoot_WhenFirstPage()
    {
        // Arrange
        PublicationPage firstPage = new()
        {
            Slug = "page-1",
            Title = "Page 1",
            PublicationSections = new List<PublicationSection>(),
            Body = new JsonElement()
        };

        PublicationPage secondPage = new()
        {
            Slug = "page-2",
            Title = "Page 2",
            Body = new JsonElement()
        };

        publicationTemplate.PublicationPages = new List<PublicationPage> { firstPage, secondPage };

        // Act
        await _publicationTemplateController.Index("slug", "page-1", null);

        // Assert
        Assert.Equal("/publications/slug", _publicationTemplateController.ViewData["CanonicalUrl"]);
    }

    [Fact]
    public async Task Index_ShouldSetCanonical_ToPage_WhenFirstSection()
    {
        // Arrange
        PublicationSection section1 = new() { Slug = "section-1" };
        PublicationSection section2 = new() { Slug = "section-2" };

        PublicationPage page = new()
        {
            Slug = "page-2",
            PublicationSections = new List<PublicationSection> { section1, section2 },
            Body = new JsonElement()
        };

        publicationTemplate.PublicationPages = new List<PublicationPage>
        {
            new() { Slug = "page-1", Body = new JsonElement() },
            page
        };

        // Act
        await _publicationTemplateController.Index("slug", "page-2", "section-1");

        // Assert
        Assert.Equal("/publications/slug/page-2", _publicationTemplateController.ViewData["CanonicalUrl"]);
    }

    [Fact]
    public async Task Index_ShouldNotSetCanonical_WhenNotFirstSection()
    {
        // Arrange
        PublicationSection section1 = new() { Slug = "section-1" };
        PublicationSection section2 = new() { Slug = "section-2" };

        PublicationPage page = new()
        {
            Slug = "page-2",
            PublicationSections = new List<PublicationSection> { section1, section2 },
            Body = new JsonElement()
        };

        publicationTemplate.PublicationPages = new List<PublicationPage>
        {
            new() { Slug = "page-1", Body = new JsonElement() },
            page
        };

        // Act
        await _publicationTemplateController.Index("slug", "page-2", "section-2");

        // Assert
        Assert.Null(_publicationTemplateController.ViewData["CanonicalUrl"]);
    }

    [Fact]
    public async Task Index_ShouldNotSetCanonical_WhenNotFirstPage_AndNoSection()
    {
        // Arrange
        PublicationPage firstPage = new()
        {
            Slug = "page-1",
            Body = new JsonElement()
        };

        PublicationPage secondPage = new()
        {
            Slug = "page-2",
            Body = new JsonElement()
        };

        publicationTemplate.PublicationPages = new List<PublicationPage> { firstPage, secondPage };

        // Act
        await _publicationTemplateController.Index("slug", "page-2", null);

        // Assert
        Assert.Null(_publicationTemplateController.ViewData["CanonicalUrl"]);
    }
}