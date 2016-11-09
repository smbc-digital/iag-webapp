using StockportWebapp.Config;

namespace StockportWebapp.Models
{
    public interface IStaticAssets
    {
        string UrlFor(string assetName);
    }

    public class StaticAssets : IStaticAssets
    {
        private readonly IApplicationConfiguration _configObject;

        public StaticAssets(IApplicationConfiguration configObject)
        {
            _configObject = configObject;
        }

        public string UrlFor(string assetName)
        {
            return string.Concat(_configObject.GetStaticAssetsRootUrl(), assetName);
        }
    }
}
