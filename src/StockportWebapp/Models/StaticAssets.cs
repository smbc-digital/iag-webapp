namespace StockportWebapp.Models;

public interface IStaticAssets
{
    string UrlFor(string assetName);
}

[ExcludeFromCodeCoverage]
public class StaticAssets(IApplicationConfiguration configObject) : IStaticAssets
{
    private readonly IApplicationConfiguration _configObject = configObject;

    public string UrlFor(string assetName) =>
        string.Concat(_configObject.GetStaticAssetsRootUrl(), assetName);
}