﻿namespace StockportWebappTests_Unit.Unit.ViewComponents;

public class SemanticFooterViewComponentTest
{
    private readonly Mock<IRepository> _repository;
    private readonly SemanticFooterViewComponent _semanticFooterViewComponent;
    private readonly Mock<ILogger<SemanticFooterViewComponent>> _logger;

    public SemanticFooterViewComponentTest()
    {
        _repository = new Mock<IRepository>();
        _logger = new Mock<ILogger<SemanticFooterViewComponent>>();
        _semanticFooterViewComponent = new SemanticFooterViewComponent(_repository.Object, _logger.Object);
    }

    [Fact]
    public async Task ShouldReturnFooterAsModelInView()
    {
        var footer = new Footer("Title", "Slug", new List<SubItem>(), new List<SocialMediaLink>(), string.Empty, string.Empty, string.Empty);
        _repository.Setup(o => o.Get<Footer>(It.IsAny<string>(), It.IsAny<List<Query>>())).ReturnsAsync(HttpResponse.Successful(200, footer));

        var result = await _semanticFooterViewComponent.InvokeAsync() as ViewViewComponentResult;

        result.ViewData.Model.Should().BeOfType<Footer>();
        var footerModel = result.ViewData.Model as Footer;
        footerModel.Should().Be(footer);

        LogTesting.Assert(_logger, LogLevel.Information, "Call to retrieve the footer");
    }

    [Fact]
    public async Task ShouldNotReturnAFooterInViewIfViewNotFound()
    {
        _repository.Setup(o => o.Get<Footer>(It.IsAny<string>(), It.IsAny<List<Query>>())).ReturnsAsync(HttpResponse.Failure(404, "No Footer Found"));

        var result = await _semanticFooterViewComponent.InvokeAsync() as ViewViewComponentResult;

        result.ViewData.Model.Should().BeNull();
        Assert.Equal("NoFooterFound", result.ViewName);
        LogTesting.Assert(_logger, LogLevel.Information, "Call to retrieve the footer");
    }
}
