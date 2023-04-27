namespace StockportWebappTests_Unit.Unit.Model;

public class StaticAssetsTest
{
    [Fact]
    public void ShouldProvideFullUrlForAnAssetUsingConfiguredRoot()
    {
        var config = new Mock<IApplicationConfiguration>();
        var assets = new StaticAssets(config.Object);

        config.Setup(o => o.GetStaticAssetsRootUrl()).Returns("http://assets.example.com/");

        var asset = assets.UrlFor("logo.png");

        asset.Should().Be("http://assets.example.com/logo.png");
    }
}