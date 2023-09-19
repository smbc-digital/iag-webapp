﻿namespace StockportWebappTests_Unit.Unit.Controllers;

public class AtoZControllerTest
{
    private readonly Mock<IRepository> _repository;
    private readonly Mock<IFeatureManager> _featureManager;
    private readonly AtoZController _controller;
    
    public AtoZControllerTest()
    {
        _repository = new Mock<IRepository>();
        _featureManager = new Mock<IFeatureManager>();
        _controller = new AtoZController(_repository.Object, _featureManager.Object);
    }

    [Fact]
    public async Task ItReturnsAnAtoZListing()
    {
        var atoz = new List<AtoZ> { new AtoZ("title", "slug", "teaser", "type") };
        var response = new HttpResponse((int)HttpStatusCode.OK, atoz, string.Empty);

        _repository.Setup(o => o.Get<List<AtoZ>>(It.IsAny<string>(), null))
            .ReturnsAsync(response);

        var view = await _controller.Index("v") as ViewResult;
        var model = view.ViewData.Model as AtoZViewModel;

        model.CurrentLetter.Should().Be("V");
        model.Items.Should().HaveCount(1);
        model.Items[0].Title.Should().Be("title");
        model.Items[0].NavigationLink.Should().Contain("slug");
        model.Items[0].Teaser.Should().Be("teaser");
    }

    [Fact]
    public async Task RedirectsTo500ErrorIfUnauthorised()
    {
        _repository
            .Setup(o => o.Get<List<AtoZ>>(It.IsAny<string>(), null))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.Unauthorized, string.Empty, string.Empty));

        var result = await _controller.Index("v") as HttpResponse;

        result.StatusCode.Should().Be(500);
    }

    [Fact]
    public async Task GetsABlankAtoZWhenNotFoundAtoZListing()
    {
        _repository
            .Setup(o => o.Get<List<AtoZ>>("a", null))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, "error", string.Empty));

        var result = await _controller.Index("a") as ViewResult;

        result.ViewData["Error"].Should().Be("error");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("abc")]
    [InlineData("$")]
    [InlineData("not a letter")]
    [InlineData("$Not a letter")]
    public async Task ShouldReturnANotFoundPageIfTheSearchTermIsNotInTheAlphabet(string searchTerm)
    {
        var response = await _controller.Index(searchTerm);

        response.Should().BeOfType<NotFoundResult>();
        _repository.Verify(o => o.Get<List<AtoZ>>(It.IsAny<string>(), null), Times.Never);
    }
}