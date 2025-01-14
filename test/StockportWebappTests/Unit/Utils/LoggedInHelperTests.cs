namespace StockportWebappTests_Unit.Unit.Utils;

public class LoggedInHelperTests
{
    private readonly Mock<IJwtDecoder> _jwtDecoder = new();
    private readonly Mock<IHttpContextAccessor> _context = new();
    private readonly Mock<ILogger<LoggedInHelper>> _logger = new();
    private readonly LoggedInHelper _loggedInHelper;

    public LoggedInHelperTests()
    {
        _jwtDecoder
            .Setup(decoder => decoder.Decode(It.IsAny<string>()))
            .Returns(new LoggedInPerson { Email = "email", Name = "name" });

        _loggedInHelper = new(_context.Object, new CurrentEnvironment("local"), _jwtDecoder.Object, _logger.Object);
    }

    [Fact]
    public void ShouldReturnLoggedInPersonIfCookieExists()
    {
        // Arrange
        DefaultHttpContext httpContext = new();
        httpContext.Request.Cookies = MockRequestCookieCollection("jwtCookie", "test");

        _context
            .Setup(ctext => ctext.HttpContext)
            .Returns(httpContext);

        // Act
        LoggedInPerson loggedInPerson = _loggedInHelper.GetLoggedInPerson();

        // Assert
        Assert.Equal("name", loggedInPerson.Name);
        Assert.Equal("email", loggedInPerson.Email);
    }

    [Fact]
    public void ShouldReturnEmptyLoggedInPersonIfNoCookieExists()
    {
        // Arrange
        DefaultHttpContext httpContext = new();

        // Mocks
        _context
            .Setup(ctext => ctext.HttpContext)
            .Returns(httpContext);

        // Act
        LoggedInPerson loggedInPerson = _loggedInHelper.GetLoggedInPerson();

        // Assert
        Assert.Null(loggedInPerson.Name);
        Assert.Null(loggedInPerson.Email);
    }

    private static IRequestCookieCollection MockRequestCookieCollection(string key, string value)
    {
        HttpRequestFeature requestFeature = new();
        FeatureCollection featureCollection = new();

        requestFeature.Headers = new HeaderDictionary
        {
            { HeaderNames.Cookie, new StringValues($"{key}={value}") }
        };

        featureCollection.Set<IHttpRequestFeature>(requestFeature);

        RequestCookiesFeature cookiesFeature = new(featureCollection);

        return cookiesFeature.Cookies;
    }
}