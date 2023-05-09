namespace StockportWebappTests_Unit.Unit.Model;

public class RedirectsTest
{
    [Fact]
    public void ShouldCompareKeysWithCurrentCultureIgnoreCase()
    {
        var fromJsonRedirects = new RedirectDictionary
        {
            {"from", "to"},
            {"from_again", "to_again"}
        };

        var businessIdRedirects = new BusinessIdRedirectDictionary
        {
            {"unittest", fromJsonRedirects}
        };

        var redirects = new ShortUrlRedirects(businessIdRedirects);

        redirects.Redirects.Count.Should().Be(1);
        redirects.Redirects["unittest"].Comparer.Should().Be(StringComparer.CurrentCultureIgnoreCase);
    }
}
