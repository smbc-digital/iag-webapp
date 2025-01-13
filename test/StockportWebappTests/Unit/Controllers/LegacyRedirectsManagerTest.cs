namespace StockportWebappTests_Unit.Unit.Controllers;

public class LegacyRedirectsManagerTest
{
    private readonly LegacyRedirectsMapper _mapper;
    private LegacyUrlRedirects _legacyUrlRedirects;
    private ShortUrlRedirects _shortUrlRedirects = new(new BusinessIdRedirectDictionary());
    private Mock<IRepository> _mockRepository = new();
    private const string BusinessId = "businessId";

    public LegacyRedirectsManagerTest()
    {
        _legacyUrlRedirects = new LegacyUrlRedirects(new BusinessIdRedirectDictionary { { BusinessId, new RedirectDictionary() } })
        {
            LastUpdated = DateTime.Now
        };

        _mapper = new LegacyRedirectsMapper(new BusinessId(BusinessId), _legacyUrlRedirects, _shortUrlRedirects, _mockRepository.Object);
    }

    [Fact]
    public async Task ShouldNotRedirectUrlIfBusinessIdIsNotInLegacyRedirects()
    {
        // Arrange
        LegacyRedirectsMapper legacyRedirectsManager = new(new BusinessId("businessId-does-not-exist"), _legacyUrlRedirects, _shortUrlRedirects, _mockRepository.Object);

        // Act
        string result = await legacyRedirectsManager.RedirectUrl(string.Empty);
        
        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task ShouldNotCallRepository_IfCacheNotExpired()
    {
        // Act
        await _mapper.RedirectUrl(string.Empty);
        
        // Assert
        _mockRepository.Verify(repo => repo.GetRedirects(), Times.Never);
    }

    [Fact]
    public async Task ShouldCallRepository_IfCacheExpired()
    {
        // Arrange
        _mockRepository
            .Setup(_ => _.GetRedirects())
            .ReturnsAsync(new HttpResponse(200, new Redirects(new BusinessIdRedirectDictionary(), new BusinessIdRedirectDictionary()), string.Empty));
        
        _legacyUrlRedirects.LastUpdated = DateTime.Now.Subtract(new TimeSpan(0, 45, 0));
        LegacyRedirectsMapper legacyRedirectsManager = new(new BusinessId("businessId-does-not-exist"), _legacyUrlRedirects, _shortUrlRedirects, _mockRepository.Object);

        // Act
        await legacyRedirectsManager.RedirectUrl(string.Empty);
        
        // Assert
        _mockRepository.Verify(repo => repo.GetRedirects(), Times.Once);
    }

    [Fact]
    public async Task ShouldIgnoreWildCardIfMoreSpecificUrlExists()
    {
        // Arrange
        AddRedirectRule("/another-url-from/a-url-with-wildcard/a-url", "/a-redirected-to-url");
        AddRedirectRule("/another-url-from/a-url-with-wildcard/*", "/another-redirected-to-url");

        // Act
        string result = await _mapper.RedirectUrl("/another-url-from/a-url-with-wildcard/a-url");
        
        // Assert
        Assert.Equal("/a-redirected-to-url", result);
    }

    [Fact]
    public async Task ShouldUseMostSpecificWildCardWhenTwoWildCardsExist()
    {
        // Arrange
        AddRedirectRule("/another-url-from/a-url-with-wildcard/*", "/another-redirected-to-url");
        AddRedirectRule("/another-url-from/*", "/yet-another-redirected-url");

        // Act
        string result = await _mapper.RedirectUrl("/another-url-from/a-url-with-wildcard/a-url");
        
        // Assert
        Assert.Equal("/another-redirected-to-url", result);
    }

    [Fact]
    public async Task ShouldRedirectToMatchedUrlIfWildcardValueMatchesPartOfRoute()
    {
        // Arrange
        AddRedirectRule("/another-url-from/a-url-with-wildcard/*", "/another-redirected-to-url");
        AddRedirectRule("/another-url-with-a-wildcard-from/*", "/another-wildcard-goes-here");

        // Act
        string firstResult = await _mapper.RedirectUrl("/another-url-from/a-url-with-wildcard/a-url-in-a-wildcard");
        string secondResult = await _mapper.RedirectUrl("/another-url-with-a-wildcard-from/another-url-from/a-url-with-wildcard");

        // Assert
        Assert.Equal("/another-redirected-to-url", firstResult);
        Assert.Equal("/another-wildcard-goes-here", secondResult);
    }

    [Fact]
    public async Task ShouldRedirectToMatchedUrlIfRouteIsTheSameAsAWildcardRoute()
    {
        // Arrange
        AddRedirectRule("/another-url-from/a-url-with-wildcard/*", "/another-redirected-to-url");

        // Act
        string result = await _mapper.RedirectUrl("/another-url-from/a-url-with-wildcard");
        
        // Assert
        Assert.Equal("/another-redirected-to-url", result);
    }

    [Fact]
    public async Task ShouldNotRedirectToUrlIfRouteDoesNotMatchTopLevelRoute()
    {
        // Arrange
        AddRedirectRule("/a-url-from/from-this", "/a-url-to");

        // Act
        string result = await _mapper.RedirectUrl("/a-url-from/from-this/a-url");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task ShouldRedirectToMatchedPageIfValueFound()
    {
        // Arrange
        AddRedirectRule("/a-url-from/from-this", "/a-url-to");
        AddRedirectRule("/another-url-from/another-from-this", "/another-url-to");

        // Act
        string firstResult = await _mapper.RedirectUrl("/a-url-from/from-this");
        string secondResult = await _mapper.RedirectUrl("/another-url-from/another-from-this");

        // Assert
        Assert.Equal("/a-url-to", firstResult);
        Assert.Equal("/another-url-to", secondResult);
    }

    [Fact]
    public async Task ShouldNotRedirectUrlIfLegacyUrlDoesNotMatch()
    {
        // Act
        string result = await _mapper.RedirectUrl("/no-url-Matching");
        
        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task ShouldRedirectToSpecificallyMatchedUrlEvenIfRequestHasASlashOnTheEnd()
    {
        // Arrange
        AddRedirectRule("/a-url-from/from-this", "/a-url-to");
        
        // Act
        string result = await _mapper.RedirectUrl("/a-url-from/from-this/");
        
        // Assert
        Assert.Equal("/a-url-to", result);
    }

    [Fact]
    public async Task ShouldRedirectToUrlMatchedByWildcardEvenIfRequestHasASlashOnTheEnd()
    {
        // Arrange
        AddRedirectRule("/path/*", "/redirected-url");
        
        // Act
        string result = await _mapper.RedirectUrl("/path/some-label/");
        
        // Assert
        Assert.Equal("/redirected-url", result);
    }

    private void AddRedirectRule(string rule, string toUrl) =>
        _legacyUrlRedirects.Redirects[BusinessId].Add(rule, toUrl);
}