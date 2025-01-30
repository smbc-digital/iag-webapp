namespace StockportWebappTests_Unit.Unit.Controllers;

public class AtoZControllerTest
{
    private readonly Mock<IRepository> _repository = new();
    private readonly AtoZController _controller;

    public AtoZControllerTest() =>
        _controller = new AtoZController(_repository.Object);

    [Fact]
    public async Task Index_ItReturnsAnAtoZListing()
    {
        // Arrange
        List<AtoZ> atoz = new() { new AtoZ("title", "slug", "teaser", "type") };
        HttpResponse response = new((int)HttpStatusCode.OK, atoz, string.Empty);

        _repository
            .Setup(repo => repo.Get<List<AtoZ>>(It.IsAny<string>(), null))
            .ReturnsAsync(response);

        // Act
        ViewResult view = await _controller.Index("v") as ViewResult;
        AtoZViewModel model = view.ViewData.Model as AtoZViewModel;

        // Arrange
        Assert.Equal("V", model.CurrentLetter);
        Assert.Single(model.Items);
        Assert.Equal("title", model.Items[0].Title);
        Assert.Equal("/slug", model.Items[0].NavigationLink);
        Assert.Equal("teaser", model.Items[0].Teaser);
    }

    [Fact]
    public async Task Index_RedirectsTo500ErrorIfUnauthorised()
    {
        // Arrange
        _repository
            .Setup(repo => repo.Get<List<AtoZ>>(It.IsAny<string>(), null))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.Unauthorized, string.Empty, string.Empty));
        
        // Act
        HttpResponse result = await _controller.Index("v") as HttpResponse;

        // Assert
        Assert.Equal(500, result.StatusCode);
    }

    [Fact]
    public async Task Index_GetsABlankAtoZWhenNotFoundAtoZListing()
    {
        // Arrange
        _repository
            .Setup(repo => repo.Get<List<AtoZ>>("a", null))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, "error", string.Empty));

        // Act
        ViewResult result = await _controller.Index("a") as ViewResult;
        
        // Arrange
        Assert.Equal("error", result.ViewData["Error"]);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("abc")]
    [InlineData("$")]
    [InlineData("not a letter")]
    [InlineData("$Not a letter")]
    public async Task Index_ShouldReturnANotFoundPageIfTheSearchTermIsNotInTheAlphabet(string searchTerm)
    {
        // Act
        IActionResult response = await _controller.Index(searchTerm);

        // Assert
        Assert.IsType<NotFoundResult>(response);
        _repository.Verify(repo => repo.Get<List<AtoZ>>(It.IsAny<string>(), null), Times.Never);
    }
}