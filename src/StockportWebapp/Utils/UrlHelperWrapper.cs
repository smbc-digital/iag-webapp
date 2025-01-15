namespace StockportWebapp.Utils;

public interface IUrlHelperWrapper
{
    string RouteUrl(RouteValueDictionary routeValueDictionary);
}

[ExcludeFromCodeCoverage]
public class UrlHelperWrapper(IUrlHelper actualUrlHelper) : IUrlHelperWrapper
{
    private readonly IUrlHelper _actualUrlHelper = actualUrlHelper;

    public string RouteUrl(RouteValueDictionary routeValueDictionary) =>
        _actualUrlHelper.RouteUrl(routeValueDictionary);
}