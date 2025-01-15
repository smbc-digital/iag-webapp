namespace StockportWebappTests_Unit.Unit.Models;

public class StaticAssetsTest
{
    [Fact]
    public void ShouldProvideFullUrlForAnAssetUsingConfiguredRoot()
    {
        // Arrange
        Mock<IApplicationConfiguration> config = new();
        StaticAssets assets = new(config.Object);

        config
            .Setup(conf => conf.GetStaticAssetsRootUrl())
            .Returns("http://assets.example.com/");

        // Act
        string asset = assets.UrlFor("logo.png");

        // Assert
        Assert.Equal("http://assets.example.com/logo.png", asset);
    }
}