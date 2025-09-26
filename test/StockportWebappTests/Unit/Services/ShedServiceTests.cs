namespace StockportWebappTests_Unit.Unit.Services;

public class ShedServiceTests
{
    private readonly ShedService _service;
    private readonly Mock<IShedApiClient> _mockShedApiClient = new();
    private readonly Mock<MarkdownWrapper> _markdownWrapper = new();

    public ShedServiceTests() =>
        _service = new(_mockShedApiClient.Object, _markdownWrapper.Object);

    [Fact]
    public async Task GetSHEDDataByHeRef_ShouldReturnShedItems_WhenApiReturnsData()
    {
        // Arrange
        string jsonResponse = "[{\"name\":\"Test Shed\",\"description\":\"Test Description\"}]";
        _mockShedApiClient
            .Setup(client => client.GetSHEDDataByHeRef(It.IsAny<string>()))
            .ReturnsAsync(jsonResponse);

        _markdownWrapper
            .Setup(wrapper => wrapper.ConvertToHtml(It.IsAny<string>()))
            .Returns<string>(input => $"<p>{input}</p>\n");

        // Act
        List<ShedItem> result = await _service.GetSHEDDataByHeRef("test-shed");

        // Assert
        Assert.Single(result);
        Assert.Equal("Test Shed", result[0].Name);
        Assert.Equal("<p>Test Description</p>\n", result[0].Description);
    }

    [Fact]
    public async Task GetSHEDDataByHeRef_ShouldReturnEmptyList_WhenApiReturnsNoData()
    {
        // Arrange
        string jsonResponse = "[]";
        _mockShedApiClient
            .Setup(client => client.GetSHEDDataByHeRef(It.IsAny<string>()))
            .ReturnsAsync(jsonResponse);

        // Act
        List<ShedItem> result = await _service.GetSHEDDataByHeRef("non-existent-shed");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetSHEDDataByNameWardsAndListingTypes_ShouldReturnShedItems_WhenApiReturnsData()
    {
        // Arrange
        string jsonResponse = "[{\"name\":\"Filtered Shed\",\"description\":\"Filtered Description\"}]";
        _mockShedApiClient
            .Setup(client => client.GetSHEDDataByNameWardsAndListingTypes(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<List<string>>()))
            .ReturnsAsync(jsonResponse);

        // Act
        List<ShedItem> result = await _service.GetSHEDDataByNameWardsAndListingTypes("filtered-shed", new List<string> { "Ward1" }, new List<string> { "Type1" });

        // Assert
        Assert.Single(result);
        Assert.Equal("Filtered Shed", result[0].Name);
        Assert.Equal("Filtered Description", result[0].Description);
    }

    [Fact]
    public async Task GetSHEDDataByNameWardsAndListingTypes_ShouldReturnEmptyList_WhenApiReturnsNoData()
    {
        // Arrange
        string jsonResponse = "[]";
        _mockShedApiClient
            .Setup(client => client.GetSHEDDataByNameWardsAndListingTypes(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<List<string>>()))
            .ReturnsAsync(jsonResponse);

        // Act
        List<ShedItem> result = await _service.GetSHEDDataByNameWardsAndListingTypes("non-existent-shed", new List<string> { "Ward1" }, new List<string> { "Type1" });

        // Assert
        Assert.Empty(result);
    }
}