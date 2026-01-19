namespace StockportWebappTests_Unit.Unit.Services;

public class TPOServiceTests
{
    private readonly TPOService _service;
    private readonly Mock<ITPOApiClient> _mockTPOApiClient = new();
    private readonly Mock<MarkdownWrapper> _markdownWrapper = new();

    public TPOServiceTests() =>
        _service = new(_mockTPOApiClient.Object, _markdownWrapper.Object);

    [Fact]
    public async Task GetSHEDDataByHeRef_ShouldReturnShedItems_WhenApiReturnsData()
    {
        // Arrange
        string jsonResponse = "{\"tpo_name\":\"Test TPO\",\"status\":\"Test Status\"}";
        _mockTPOApiClient
            .Setup(client => client.GetTPODataByID(It.IsAny<string>()))
            .ReturnsAsync(jsonResponse);

        _markdownWrapper
            .Setup(wrapper => wrapper.ConvertToHtml(It.IsAny<string>()))
            .Returns<string>(input => $"<p>{input}</p>\n");

        // Act
        TPOItem result = await _service.GetTPODataByID("70N");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test TPO", result.Tpo_name);
        Assert.Equal("Test Status", result.Status);
    }

    [Fact]
    public async Task GetSHEDDataByHeRef_ShouldReturnEmptyList_WhenApiReturnsNoData()
    {
        // Arrange
        string jsonResponse = "";
        _mockTPOApiClient
            .Setup(client => client.GetTPODataByID(It.IsAny<string>()))
            .ReturnsAsync(jsonResponse);

        // Act
        TPOItem result = await _service.GetTPODataByID("non-existent-shed");

        // Assert
        Assert.Null(result);
    }

}