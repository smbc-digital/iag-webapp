namespace StockportWebappTests_Unit.Unit.Utils;

public class FakeCookie : IRequestCookieCollection, IResponseCookies
{
    private readonly Dictionary<string, string> _dictionary = new();

    public FakeCookie(bool addDefaults = false, bool isAlert = false)
    {
        if (isAlert)
            _dictionary.Add("alerts", $"{{ \"{typeof(Alert).ToString().ToLower()}\":[\"foo\",\"bar\",\"test1\"] }}");
    }

    public string this[string key] =>
        _dictionary.ContainsKey(key)
            ? _dictionary[key]
            : string.Empty;

    public int Count { get; }

    public ICollection<string> Keys { get; }

    public bool ContainsKey(string key) => true;

    public bool TryGetValue(string key, out string value)
    {
        value = _dictionary[key];
        return true;
    }

    public IEnumerable<KeyValuePair<string, string>> GetEnumerator() => new List<KeyValuePair<string, string>>();

    IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator() => throw new NotImplementedException();

    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

    public void Append(string key, string value) => _dictionary[key] = value;

    public void Append(string key, string value, CookieOptions options) => _dictionary[key] = value;

    public void Delete(string key) => throw new NotImplementedException();

    public void Delete(string key, CookieOptions options) => throw new NotImplementedException();
}

public class CookiesHelperTests
{
    private readonly CookiesHelper cookiesHelper;
    private readonly Mock<IHttpContextAccessor> httpContextAccessor = new();

    public CookiesHelperTests() =>
        cookiesHelper = new(httpContextAccessor.Object);

    [Fact]
    public void PopulateCookies_ShouldThrowException_WhenCookiePropOrSlugIsNull()
    {
        // Arrange
        FakeCookie cookies = new(false, true);

        List<Alert> alerts = new()
        {
            new("alert", "alertSubHeading", "body", string.Empty, new DateTime(), new DateTime(), "alert", true, string.Empty)
        };

        httpContextAccessor
            .Setup(http => http.HttpContext.Request.Cookies)
            .Returns(cookies);
        
        httpContextAccessor
            .Setup(http => http.HttpContext.Response.Cookies)
            .Returns(cookies);

        // Act
        Exception result = Assert.Throws<Exception>(() => cookiesHelper.PopulateCookies(alerts, "alerts"));
        
        // Assert
        Assert.Equal("The object you are adding to favourites does not have either the property 'Favourite' or the property 'Slug'", result.Message);
    }

    [Fact]
    public void AddToCookies_ShouldAddToFavouritesCollection()
    {
        // Arrange
        FakeCookie cookies = new();

        httpContextAccessor
            .Setup(http => http.HttpContext.Request.Cookies)
            .Returns(cookies);
        
        httpContextAccessor
            .Setup(http => http.HttpContext.Response.Cookies)
            .Returns(cookies);

        // Act
        cookiesHelper.AddToCookies<Event>("test2", "favourites");
        Dictionary<string, List<string>> result = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(cookies["favourites"]);

        // Assert
        Assert.Contains("test2", result[typeof(Event).ToString().ToLower()]);
    }
}