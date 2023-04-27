namespace StockportWebappTests_Unit.Unit.Repositories;

public class DocumentsRepositoryTests
{
    [Fact]
    public async void GetSecureDocument_ShouldReturnDocument()
    {
        // Arrange
        var httpClient = new Mock<IHttpClient>();
        var applicationConfiguraiton = new Mock<IApplicationConfiguration>();
        var simpleUrlGenerator = new Mock<IUrlGeneratorSimple>();
        var loggedInHelper = new Mock<ILoggedInHelper>();
        var logger = new Mock<ILogger<BaseRepository>>();
        var documentsRepository = new DocumentsRepository(httpClient.Object, applicationConfiguraiton.Object, simpleUrlGenerator.Object, loggedInHelper.Object, logger.Object);
        var document = new DocumentBuilder().Build();
        var seralisedDocument = JsonConvert.SerializeObject(document);

        // Mock
        simpleUrlGenerator.Setup(o => o.BaseContentApiUrl<Document>()).Returns("url");
        loggedInHelper.Setup(o => o.GetLoggedInPerson()).Returns(new LoggedInPerson() { Email = "email" });
        httpClient.Setup(o => o.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).ReturnsAsync(new HttpResponse(200, seralisedDocument, string.Empty));

        // Act
        var documentResponse = await documentsRepository.GetSecureDocument("asset id", "group-slug");

        // Assert
        documentResponse.Should().NotBeNull();
        documentResponse.Should().BeEquivalentTo(document);
    }

    [Fact]
    public async void GetSecureDocument_ShouldLogIfExceptionIsThrown()
    {
        var httpClient = new Mock<IHttpClient>();
        var applicationConfiguraiton = new Mock<IApplicationConfiguration>();
        var simpleUrlGenerator = new Mock<IUrlGeneratorSimple>();
        var loggedInHelper = new Mock<ILoggedInHelper>();
        var logger = new Mock<ILogger<BaseRepository>>();
        var documentsRepository = new DocumentsRepository(httpClient.Object, applicationConfiguraiton.Object, simpleUrlGenerator.Object, loggedInHelper.Object, logger.Object);
        var document = new DocumentBuilder().Build();
        var seralisedDocument = JsonConvert.SerializeObject(document);

        // Mock
        simpleUrlGenerator.Setup(o => o.BaseContentApiUrl<Document>()).Returns("url");
        loggedInHelper.Setup(o => o.GetLoggedInPerson()).Returns(new LoggedInPerson() { Email = "email" });
        httpClient.Setup(o => o.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).ThrowsAsync(new System.Exception());

        // Act
        var documentResponse = await documentsRepository.GetSecureDocument("asset id", "group-slug");

        LogTesting.Assert(logger, LogLevel.Error, $"Error getting response for url url/group-slug/asset id");
        documentResponse.Should().BeNull();
    }

    [Fact]
    public async void GetSecureDocument_ShouldReturnNullIfUserIsntLoggedIn()
    {
        var httpClient = new Mock<IHttpClient>();
        var applicationConfiguraiton = new Mock<IApplicationConfiguration>();
        var simpleUrlGenerator = new Mock<IUrlGeneratorSimple>();
        var loggedInHelper = new Mock<ILoggedInHelper>();
        var logger = new Mock<ILogger<BaseRepository>>();
        var documentsRepository = new DocumentsRepository(httpClient.Object, applicationConfiguraiton.Object, simpleUrlGenerator.Object, loggedInHelper.Object, logger.Object);
        var document = new DocumentBuilder().Build();
        var seralisedDocument = JsonConvert.SerializeObject(document);

        // Mock
        simpleUrlGenerator.Setup(o => o.BaseContentApiUrl<Document>()).Returns("url");
        loggedInHelper.Setup(o => o.GetLoggedInPerson()).Returns(new LoggedInPerson());

        // Act
        var documentResponse = await documentsRepository.GetSecureDocument("asset id", "group-slug");

        LogTesting.Assert(logger, LogLevel.Warning, "Document asset id was requested, but the user wasn't logged in");
        documentResponse.Should().BeNull();
    }
}
