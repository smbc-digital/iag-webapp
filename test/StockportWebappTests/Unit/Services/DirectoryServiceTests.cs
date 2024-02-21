using StockportGovUK.NetStandard.Gateways.Models.Exceptions;

namespace StockportWebappTests_Unit.Unit.Services;

public class DirectoryServiceTests
{
    private readonly DirectoryService _service;
    private readonly Mock<IApplicationConfiguration> _mockApplicationConfiguration = new();
    private readonly Mock<MarkdownWrapper> _mockMarkdownWrapper = new();
    private readonly Mock<IRepository> _mockRepository = new();

    public DirectoryServiceTests()
    {
        _mockMarkdownWrapper
            .Setup(_ => _.ConvertToHtml(It.IsAny<string>()))
            .Returns("string");

        _mockApplicationConfiguration.Setup(_ => _.GetEmailEmailFrom("stockportgov")).Returns(() => AppSetting.GetAppSetting("test"));

        _service = new DirectoryService(
            _mockApplicationConfiguration.Object,
            _mockMarkdownWrapper.Object,
            _mockRepository.Object
        );
    }

    [Fact]
    public async Task Get_ShouldReturnDirectory()
    {
        // Arrange
        _mockRepository
            .Setup(_ => _.Get<Directory>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, new Directory { Title = "Directory" }));

        // Act
        var result = await _service.Get<Directory>("string");

        // Assert
        Assert.IsType<Directory>(result);
    }

    [Fact]
    public void Get_ShouldThrowException_IfNotSuccessful()
    {
        // Arrange
        _mockRepository
            .Setup(_ => _.Get<Directory>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Failure(500,"Internal Server Error"));

        // Act & Assert
        Assert.ThrowsAsync<HttpRequestException>(async () => await _service.Get<Directory>("string"));
    }

    [Fact]
    public async Task GetEntry_ShouldReturnDirectoryEntry()
    {
        // Arrange
        _mockRepository
            .Setup(_ => _.Get<DirectoryEntry>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, new DirectoryEntry { Name = "DirectoryEntry" }));

        // Act
        var result = await _service.GetEntry<DirectoryEntry>("string");

        // Assert
        Assert.IsType<DirectoryEntry>(result);
    }

    [Fact]
    public async Task GetEntry_ShouldCall_MarkdownWrapper()
    {
        // Arrange
        _mockRepository
            .Setup(_ => _.Get<DirectoryEntry>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, new DirectoryEntry { Name = "DirectoryEntry" }));

        // Act
        var result = await _service.GetEntry<DirectoryEntry>("string");

        // Assert
        _mockMarkdownWrapper.Verify(_ => _.ConvertToHtml(It.IsAny<string>()), Times.Exactly(2));
    }

    [Fact]
    public void GetEntry_ShouldThrowException_IfNotSuccessful()
    {
        // Arrange
        _mockRepository
            .Setup(_ => _.Get<DirectoryEntry>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Failure(500, "Internal Server Error"));

        // Act & Assert
        Assert.ThrowsAsync<HttpRequestException>(async () => await _service.GetEntry<DirectoryEntry>("string"));
    }
}
