namespace StockportWebappTests_Unit.Unit.Repositories;

public class RepositoryTest
{
    private readonly Repository _repository;
    private readonly Mock<IHttpClient> _httpClientMock = new Mock<IHttpClient>();
    private readonly UrlGenerator _urlGenerator;
    private readonly Mock<IUrlGeneratorSimple> _urlGeneratorSimple = new Mock<IUrlGeneratorSimple>();

    public RepositoryTest()
    {
        var appConfig = new Mock<IApplicationConfiguration>();
        appConfig.Setup(o => o.GetContentApiUri()).Returns(new Uri("http://localhost:5000/"));

        _urlGenerator = new UrlGenerator(appConfig.Object, new BusinessId(""));
        _urlGeneratorSimple.Setup(o => o.BaseContentApiUrl<Event>()).Returns("url");

        _repository = new Repository(_urlGenerator, _httpClientMock.Object, appConfig.Object, _urlGeneratorSimple.Object);
    }

    [Fact]
    public async Task Put_ShouldCallPutAsyncWithHeaders()
    {
        // Arrange
        var url = $"{_urlGenerator.UrlFor<Group>("test_slug")}/administrators/test@test.com";
        var httpContent = new StringContent(url, Encoding.UTF8, "application/json");

        _httpClientMock.Setup(o => o.PutAsync(It.IsAny<string>(), It.IsAny<HttpContent>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(200, null, string.Empty));

        // Act
        await _repository.Put<Group>(httpContent, "");

        // Assert
        _httpClientMock.Verify(_ => _.PutAsync(It.IsAny<string>(), It.IsAny<HttpContent>(), It.IsAny<Dictionary<string, string>>()), Times.Once());
    }

    [Fact]
    public async Task DeletesGroupAdministrator()
    {
        var url = $"{_urlGenerator.UrlFor<Group>("test_slug")}/administrators/test@test.com";

        _httpClientMock.Setup(o => o.DeleteAsync(url, It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(200, null, string.Empty));

        var httpResponse = await _repository.RemoveAdministrator("test_slug", "test@test.com");

        httpResponse.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task AddsGroupAdministrator()
    {
        var url = $"{_urlGenerator.UrlFor<Group>("test_slug")}/administrators/test@test.com";

        var jsonContent = JsonConvert.SerializeObject("E");
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        _httpClientMock.Setup(o => o.PostAsync(url, httpContent, It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(200, null, string.Empty));

        var httpResponse = await _repository.AddAdministrator(httpContent, "test_slug", "test@test.com");

        httpResponse.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task UpdatesGroupAdministrator()
    {
        var url = $"{_urlGenerator.UrlFor<Group>("test_slug")}/administrators/test@test.com";

        var jsonContent = JsonConvert.SerializeObject("E");
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        _httpClientMock.Setup(o => o.PutAsync(url, httpContent, It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(200, null, string.Empty));

        var httpResponse = await _repository.UpdateAdministrator(httpContent, "test_slug", "test@test.com");

        httpResponse.StatusCode.Should().Be(200);
    }
}