namespace StockportWebappTests_Unit.Unit.Controllers;

public class CookiesControllerTests
{
    private readonly Mock<ICookiesHelper> _cookiesHelperMock = new();
    private readonly CookiesController _cookiesController;

    public CookiesControllerTests() =>
        _cookiesController = new CookiesController(_cookiesHelperMock.Object);

    [Fact]
    public void AddCookie_ShouldAddToCookies_Alerts()
    {
        // Act
        IActionResult result = _cookiesController.AddCookie("alertSlug", "alert");

        // Assert
        _cookiesHelperMock.Verify(helper => helper.AddToCookies<Alert>("alertSlug", "alerts"), Times.Once);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public void RemoveCookie_ShouldRemoveFromCookies_Alerts()
    {
        // Act
        IActionResult result = _cookiesController.RemoveCookie("alertSlug", "alert");

        // Assert
        _cookiesHelperMock.Verify(helper => helper.RemoveFromCookies<Alert>("alertSlug", "alerts"), Times.Once);
        Assert.IsType<OkResult>(result);
    }
}