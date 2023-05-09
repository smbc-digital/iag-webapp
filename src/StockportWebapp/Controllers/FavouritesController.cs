namespace StockportWebapp.Controllers;

public class FavouritesController
{
    private CookiesHelper _cookiesHelper;
    private IHttpContextAccessor _httpContextAccessor;

    public FavouritesController(CookiesHelper cookiesHelper, IHttpContextAccessor httpContextAccessor)
    {
        _cookiesHelper = cookiesHelper;
        _httpContextAccessor = httpContextAccessor;
    }

    [Route("/favourites/add")]
    public IActionResult AddGroupsFavourite([FromQuery] string slug, [FromQuery] string type)
    {
        switch (type)
        {
            case "group":
                _cookiesHelper.AddToCookies<Group>(slug, "favourites");
                break;
            case "event":
                _cookiesHelper.AddToCookies<Event>(slug, "favourites");
                break;
        }

        return new OkResult();
    }

    [Route("/favourites/nojs/add")]
    public IActionResult AddGroupsFavouriteNoJs([FromQuery] string slug, [FromQuery] string type)
    {
        switch (type)
        {
            case "group":
                _cookiesHelper.AddToCookies<Group>(slug, "favourites");
                break;
            case "event":
                _cookiesHelper.AddToCookies<Event>(slug, "favourites");
                break;
        }

        var referer = _httpContextAccessor.HttpContext.Request.Headers["referer"];

        if (string.IsNullOrEmpty(referer))
        {
            return new RedirectToActionResult("FavouriteGroups", "Groups", null);
        }

        return new RedirectResult(referer);
    }

    [Route("/favourites/remove")]
    public IActionResult RemoveGroupsFavourite([FromQuery] string slug, [FromQuery] string type)
    {
        switch (type)
        {
            case "group":
                _cookiesHelper.RemoveFromCookies<Group>(slug, "favourites");
                break;
            case "event":
                _cookiesHelper.RemoveFromCookies<Event>(slug, "favourites");
                break;
        }

        return new OkResult();
    }

    [Route("/favourites/nojs/remove")]
    public IActionResult RemoveGroupsFavouriteNoJs([FromQuery] string slug, [FromQuery] string type)
    {
        switch (type)
        {
            case "group":
                _cookiesHelper.RemoveFromCookies<Group>(slug, "favourites");
                break;
            case "event":
                _cookiesHelper.RemoveFromCookies<Event>(slug, "favourites");
                break;
        }

        var referer = _httpContextAccessor.HttpContext.Request.Headers["referer"];

        if (string.IsNullOrEmpty(referer))
        {
            return new RedirectToActionResult("FavouriteGroups", "Groups", null);
        }

        return new RedirectResult(referer);
    }
}
