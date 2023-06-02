namespace StockportWebappTests_Unit.Unit.Utils;

public class FakeCookie : IRequestCookieCollection, IResponseCookies
{
    private Dictionary<string, string> _dictionary = new Dictionary<string, string>();

    public FakeCookie(bool addDefaults = false)
    {
        if (addDefaults)
        {
            _dictionary.Add("favourites", $"{{ \"{typeof(Group)}\":[\"foo\",\"bar\",\"test1\"] }}");
        }
    }

    public string this[string key]
    {
        get
        {
            if (_dictionary.ContainsKey(key))
            {
                return _dictionary[key];
            }
            else
            {
                return string.Empty;
            }
        }
    }

    public int Count { get; }

    public ICollection<string> Keys { get; }

    public bool ContainsKey(string key)
    {
        return true;
    }

    public bool TryGetValue(string key, out string value)
    {
        value = _dictionary[key];
        return true;
    }

    public IEnumerable<KeyValuePair<string, string>> GetEnumerator()
    {
        return new List<KeyValuePair<string, string>>();
    }

    IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public void Append(string key, string value)
    {
        _dictionary[key] = value;
    }

    public void Append(string key, string value, CookieOptions options)
    {
        _dictionary[key] = value;
    }

    public void Delete(string key)
    {
        throw new NotImplementedException();
    }

    public void Delete(string key, CookieOptions options)
    {
        throw new NotImplementedException();
    }
}

public class CookiesHelperTests
{
    private readonly CookiesHelper cookiesHelper;
    private Mock<IHttpContextAccessor> httpContextAccessor;

    public CookiesHelperTests()
    {
        httpContextAccessor = new Mock<IHttpContextAccessor>();
        cookiesHelper = new CookiesHelper(httpContextAccessor.Object);
    }

    [Fact]
    public void ShouldPopulateFavouritePropertyToCollectionWhencallingPopulateFavourites()
    {
        // Arrange
        var cookies = new FakeCookie(true);

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
    public void ShouldAddToFavouritesCollection()
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
        result[typeof(Group).ToString()].Should().Equal(@"test1");
        result[typeof(Event).ToString()].Should().Equal(@"test2");
    }

    [Fact]
    public void ShouldRemoveFromFavouritesCollection()
    {
        // Arrange
        var cookies = new FakeCookie(true);

        httpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies).Returns(cookies);
        httpContextAccessor.Setup(_ => _.HttpContext.Response.Cookies).Returns(cookies);

        // Act
        cookiesHelper.RemoveFromCookies<Group>("foo", "favourites");
        cookiesHelper.RemoveFromCookies<Group>("bar", "favourites");
        var result = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(cookies["favourites"]);

        // Assert
        result[typeof(Group).ToString()].Should().Equal(@"test1");
    }

    [Fact]
    public void ShouldRemoveAllFromFavouritesCollection()
    {
        // Arrange
        var cookies = new FakeCookie(true);

        httpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies).Returns(cookies);
        httpContextAccessor.Setup(_ => _.HttpContext.Response.Cookies).Returns(cookies);

        // Act
        cookiesHelper.RemoveAllFromCookies<Group>("favourites");
        var result = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(cookies["favourites"]);

        // Assert
        result.ContainsKey(typeof(Group).ToString()).Should().Be(false);
    }

    [Fact]
    public void ShouldGetAllFavouritesFromFavouritesCollection()
    {
        // Arrange
        var cookies = new FakeCookie(true);

        httpContextAccessor.Setup(_ => _.HttpContext.Request.Cookies).Returns(cookies);
        httpContextAccessor.Setup(_ => _.HttpContext.Response.Cookies).Returns(cookies);

        // Act
        var favourites = cookiesHelper.GetCookies<Group>("favourites");

        // Assert
        favourites.Should().HaveCount(3);
    }
}
