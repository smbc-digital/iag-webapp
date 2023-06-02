namespace StockportWebapp.Controllers;

[Route("cookies")]
public class CookiesController : Controller
{
    private readonly ICookiesHelper _cookiesHelper;

    public CookiesController(ICookiesHelper cookiesHelper)
    {
        _cookiesHelper = cookiesHelper;
    }

    [Route("add")]
    public IActionResult AddCookie(string slug, string cookieType)
    {
        switch (cookieType)
        {
            case "group":
                _cookiesHelper.AddToCookies<Group>(slug, "favourites");
                break;
            case "event":
                _cookiesHelper.AddToCookies<Event>(slug, "favourites");
                break;
            case "alert":
                _cookiesHelper.AddToCookies<Alert>(slug, "alerts");
                break;
        }

        return Ok();
    }

    [Route("remove")]
    public IActionResult RemoveCookie(string slug, string cookieType)
    {
        switch (cookieType)
        {
            case "group":
                _cookiesHelper.RemoveFromCookies<Group>(slug, "favourites");
                break;
            case "event":
                _cookiesHelper.RemoveFromCookies<Event>(slug, "favourites");
                break;
            case "alert":
                _cookiesHelper.RemoveFromCookies<Alert>(slug, "alerts");
                break;
        }

        return Ok();
    }
}
