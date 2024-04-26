namespace StockportWebappTests_Unit.Unit.Utils;

public class FakeCookie : IRequestCookieCollection, IResponseCookies
{
    private Dictionary<string, string> _dictionary = new Dictionary<string, string>();

    public FakeCookie(bool addDefaults = false, bool isAlert = false)
    {
        if (addDefaults)
            _dictionary.Add("favourites", $"{{ \"{typeof(Group).ToString().ToLower()}\":[\"foo\",\"bar\",\"test1\"] }}");

        if (isAlert)
            _dictionary.Add("alerts", $"{{ \"{typeof(Alert).ToString().ToLower()}\":[\"foo\",\"bar\",\"test1\"] }}");
    }

    public string this[string key]
    {
        get
        {
            if (_dictionary.ContainsKey(key))
                return _dictionary[key];
            else
                return string.Empty;
        }
    }

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
    private Mock<IHttpContextAccessor> httpContextAccessor;
    private Mock<ILogger<CookiesHelper>> logger;

    public CookiesHelperTests()
    {
        httpContextAccessor = new Mock<IHttpContextAccessor>();
        cookiesHelper = new CookiesHelper(httpContextAccessor.Object, logger.Object);
    }

    [Fact]
    public void PopulateCookies_ShouldPopulateFavouritePropertyToCollection_WhenCallingPopulateFavourites()
    {
        // Arrange
        var cookies = new FakeCookie(true, false);

        var groups = new List<Group>()
        {
            new GroupBuilder().Slug("test1").Build(),
            new GroupBuilder().Build()
        };

        httpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies).Returns(cookies);

        // Act
        groups = cookiesHelper.PopulateCookies(groups, "favourites");

        // Assert
        groups[0].Favourite.Should().Be(true);
        groups[1].Favourite.Should().Be(false);
    }

    [Fact]
    public void PopulateCookies_ShouldThrowException_WhenCookiePropOrSlugIsNull()
    {
        // Arrange
        var cookies = new FakeCookie(false, true);

        var alerts = new List<Alert>()
        {
            new Alert("alert", "alertSubHeading", "body", string.Empty, new DateTime(), new DateTime(), "alert", true, string.Empty)
        };

        httpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies).Returns(cookies);
        httpContextAccessor.Setup(_ => _.HttpContext.Response.Cookies).Returns(cookies);

        // Assert
        var result = Assert.Throws<Exception>(() => cookiesHelper.PopulateCookies(alerts, "alerts"));
        Assert.Equal("The object you are adding to favourites does not have either the property 'Favourite' or the property 'Slug'", result.Message);
    }

    [Fact]
    public void AddToCookies_ShouldAddToFavouritesCollection()
    {
        // Arrange
        var cookies = new FakeCookie();

        httpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies).Returns(cookies);
        httpContextAccessor.Setup(_ => _.HttpContext.Response.Cookies).Returns(cookies);

        // Act
        cookiesHelper.AddToCookies<Group>("test1", "favourites");
        cookiesHelper.AddToCookies<Event>("test2", "favourites");
        var result = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(cookies["favourites"]);

        // Assert
        result[typeof(Group).ToString().ToLower()].Should().Equal(@"test1");
        result[typeof(Event).ToString().ToLower()].Should().Equal(@"test2");
    }

    [Fact]
    public void RemoveFromCookies_ShouldRemoveFromFavouritesCollection()
    {
        // Arrange
        var cookies = new FakeCookie(true, false);

        httpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies).Returns(cookies);
        httpContextAccessor.Setup(_ => _.HttpContext.Response.Cookies).Returns(cookies);

        // Act
        cookiesHelper.RemoveFromCookies<Group>("foo", "favourites");
        cookiesHelper.RemoveFromCookies<Group>("bar", "favourites");
        var result = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(cookies["favourites"]);

        // Assert
        result[typeof(Group).ToString().ToLower()].Should().Equal(@"test1");
    }

    [Fact]
    public void RemoveFromCookies_ShouldAddNewKeyToDict()
    {
        // Arrange
        var cookies = new FakeCookie(false, true);

        httpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies).Returns(cookies);
        httpContextAccessor.Setup(_ => _.HttpContext.Response.Cookies).Returns(cookies);

        // Act
        cookiesHelper.RemoveFromCookies<Group>("newTest", "favourites");
        var result = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(cookies["favourites"]);

        // Assert
        Assert.Equal(typeof(Group).ToString().ToLower(), result.Keys.First());
    }

    [Fact]
    public void RemoveAllFromCookies_ShouldRemoveAllFromFavouritesCollection()
    {
        // Arrange
        var cookies = new FakeCookie(true, false);

        httpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies).Returns(cookies);
        httpContextAccessor.Setup(_ => _.HttpContext.Response.Cookies).Returns(cookies);

        // Act
        cookiesHelper.RemoveAllFromCookies<Group>("favourites");
        var result = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(cookies["favourites"]);

        // Assert
        result.ContainsKey(typeof(Group).ToString().ToLower()).Should().Be(false);
    }

    [Fact]
    public void GetCookies_ShouldGetAllFavouritesFromFavouritesCollection()
    {
        // Arrange
        var cookies = new FakeCookie(true, false);

        httpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies).Returns(cookies);
        httpContextAccessor.Setup(_ => _.HttpContext.Response.Cookies).Returns(cookies);

        // Act
        var favourites = cookiesHelper.GetCookies<Group>("favourites");

        // Assert
        favourites.Should().HaveCount(3);
    }
}
