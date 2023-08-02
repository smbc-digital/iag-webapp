namespace StockportWebappTests_Unit.Unit.Controllers;

public class CookiesControllerTests
{
    private readonly Mock<ICookiesHelper> _cookiesHelperMock;
    private readonly CookiesController _cookiesController;

    public CookiesControllerTests()
    {
        _cookiesHelperMock = new Mock<ICookiesHelper>();
        _cookiesController = new CookiesController(_cookiesHelperMock.Object);
    }

    [Fact]
    public void AddCookie_ShouldAddToCookies_Groups()
    {
        // Act
        var result = _cookiesController.AddCookie("groupSlug", "group");

        // Assert
        _cookiesHelperMock.Verify(helper => helper.AddToCookies<Group>("groupSlug", "favourites"), Times.Once);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public void AddCookie_ShouldAddToCookies_Events()
    {
        // Act
        var result = _cookiesController.AddCookie("eventSlug", "event");

        // Assert
        _cookiesHelperMock.Verify(helper => helper.AddToCookies<Event>("eventSlug", "favourites"), Times.Once);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public void AddCookie_ShouldAddToCookies_Alerts()
    {
        // Act
        var result = _cookiesController.AddCookie("alertSlug", "alert");

        // Assert
        _cookiesHelperMock.Verify(helper => helper.AddToCookies<Alert>("alertSlug", "alerts"), Times.Once);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public void RemoveCookie_ShouldRemoveFromCookies_Groups()
    {
        // Act
        var result = _cookiesController.RemoveCookie("groupSlug", "group");

        // Assert
        _cookiesHelperMock.Verify(helper => helper.RemoveFromCookies<Group>("groupSlug", "favourites"), Times.Once);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public void RemoveCookie_ShouldRemoveFromCookies_Events()
    {
        // Act
        var result = _cookiesController.RemoveCookie("eventSlug", "event");

        // Assert
        _cookiesHelperMock.Verify(helper => helper.RemoveFromCookies<Event>("eventSlug", "favourites"), Times.Once);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public void RemoveCookie_ShouldRemoveFromCookies_Alerts()
    {
        // Act
        var result = _cookiesController.RemoveCookie("alertSlug", "alert");

        // Assert
        _cookiesHelperMock.Verify(helper => helper.RemoveFromCookies<Alert>("alertSlug", "alerts"), Times.Once);
        Assert.IsType<OkResult>(result);
    }
}
