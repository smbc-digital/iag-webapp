namespace StockportWebapp.Controllers;
[Obsolete("Groups is being replaced by directories/directory entries, favorites is no longer used")]
[ExcludeFromCodeCoverage(Justification="Obsolete")]
public class FavouritesController(CookiesHelper cookiesHelper,
                                IHttpContextAccessor httpContextAccessor)
{
    private CookiesHelper _cookiesHelper = cookiesHelper;
    private IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

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

        StringValues referer = _httpContextAccessor.HttpContext.Request.Headers["referer"];

        if (string.IsNullOrEmpty(referer))
            return new RedirectToActionResult("FavouriteGroups", "Groups", null);

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

        StringValues referer = _httpContextAccessor.HttpContext.Request.Headers["referer"];

        if (string.IsNullOrEmpty(referer))
            return new RedirectToActionResult("FavouriteGroups", "Groups", null);

        return new RedirectResult(referer);
    }
}