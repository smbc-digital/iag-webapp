namespace StockportWebapp.Controllers;

[Route("cookies")]
public class CookiesController(ICookiesHelper cookiesHelper) : Controller
{
    private readonly ICookiesHelper _cookiesHelper = cookiesHelper;

    [Route("add")]
    public IActionResult AddCookie(string slug, string cookieType)
    {
        switch (cookieType)
        {
            case "alert":
                _cookiesHelper.AddToCookies<Alert>(slug, "alerts");
                break;
            case "map":
                _cookiesHelper.AddToCookies<string>(slug, "map");
                break;
        }

        return Ok();
    }

    [Route("remove")]
    public IActionResult RemoveCookie(string slug, string cookieType)
    {
        switch (cookieType)
        {
            case "alert":
                _cookiesHelper.RemoveFromCookies<Alert>(slug, "alerts");    
                break;
            case "map":
                _cookiesHelper.RemoveFromCookies<string>(slug, "map");
                break;
        }

        return Ok();
    }
}