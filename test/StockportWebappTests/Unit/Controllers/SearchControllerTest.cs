namespace StockportWebappTests_Unit.Unit.Controllers;

public class SearchControllerTest
{
    private readonly SearchController _searchController;
    private readonly Mock<IApplicationConfiguration> _config = new();
    private readonly string _businessId = "businessId";
    private const string PostcodeUrl = "postcode_url";

    public SearchControllerTest() =>
        _searchController = new(_config.Object, new BusinessId(_businessId), null);

    [Fact]
    public async Task ItRedirectsToPostcodeSearchUrl()
    {
        // Arrange
        _config
            .Setup(conf => conf.GetPostcodeSearchUrl(_businessId))
            .Returns(AppSetting.GetAppSetting(PostcodeUrl));

        // Act
        IActionResult result = await _searchController.Postcode("m45 3fz");

        // Assert
        _config.Verify(conf => conf.GetPostcodeSearchUrl(_businessId), Times.Once);
        RedirectResult redirect = result as RedirectResult;
        Assert.IsType<RedirectResult>(result);
        Assert.Equal($"{PostcodeUrl}m45 3fz", redirect.Url);
    }

    [Fact]
    public async Task ShouldRedierctToApplicationErrorIfPostCodeUrlConfigurationIsMissing()
    {
        // Arrange
        _config
            .Setup(conf => conf.GetPostcodeSearchUrl(_businessId))
            .Returns(AppSetting.GetAppSetting(null));

        // Act
        StatusCodeResult result = await _searchController.Postcode("a-postcode") as StatusCodeResult;

        // Assert
        Assert.Equal(404, result.StatusCode);
    }
}