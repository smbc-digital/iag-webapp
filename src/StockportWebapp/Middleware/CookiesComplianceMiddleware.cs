namespace StockportWebapp.Middleware;

/// <summary>
/// This is a one off piece of middleware to  ensure 
/// that no old cookies are retained from the previous consent scheme 
/// that we know require consent (Google Analytics, SiteImprove, Alerts dismisal, Old cookie consent)
/// </summary>
/// 
public class CookiesComplianceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ICookiesHelper _cookiesHelper;
    private readonly ILogger<CookiesComplianceMiddleware> _logger;

    public CookiesComplianceMiddleware(RequestDelegate next, CookiesHelper cookiesHelper, ILogger<CookiesComplianceMiddleware> logger)
    {
        _next = next;
        _cookiesHelper = cookiesHelper;
        _logger = logger;
    }

    public Task Invoke(HttpContext httpContext)
    {
        // Remove legacy cookie consent
        _cookiesHelper.RemoveCookie("wpcc");

        if (!_cookiesHelper.HasCookieConsentBeenCollected())
        {
            _logger.LogWarning("CookiesComplianceMiddleware:Invoke: Cookie compliance has not been collected removing non essential cookies");
            RemoveFunctionalCookies();
            RemoveTrackingCookies();
            return _next(httpContext);
        }

        var consentLevels = _cookiesHelper.GetCurrentCookieConsentLevel();

        if (!consentLevels.Functionality)
        {
            _logger.LogWarning("CookiesComplianceMiddleware:Invoke: Consent not given for FUNCITONAL Cookies removing functional all cookies");
            RemoveFunctionalCookies();
        }


        if (!consentLevels.Tracking)
        {
            _logger.LogWarning("CookiesComplianceMiddleware:Invoke: Consent not given for TRACKING Cookies removing tracking all cookies");
            RemoveTrackingCookies();
        }

        if (!consentLevels.Targetting)
        {
            _logger.LogWarning("CookiesComplianceMiddleware:Invoke: Consent not given for TARGETTING Cookies removing targetting all cookies");
            RemoveTargettingCookies();
        }

        return _next(httpContext);
    }

    private void RemoveFunctionalCookies()
    {
        _cookiesHelper.RemoveCookie("alerts");
        _cookiesHelper.RemoveCookie("favourites");
    }

    private void RemoveTrackingCookies()
    {
        _cookiesHelper.RemoveCookiesStartingWith("_ga");
        _cookiesHelper.RemoveCookie("_gat");
        _cookiesHelper.RemoveCookie("_gid");
        _cookiesHelper.RemoveCookie("nmstat");
        _cookiesHelper.RemoveCookie("hstc");
        _cookiesHelper.RemoveCookie("unam");
        _cookiesHelper.RemoveCookie("hsfirstvisit");
        _cookiesHelper.RemoveCookie("hubspotuk");
        _cookiesHelper.RemoveCookie("siteimproveses");
        _cookiesHelper.RemoveCookie("ga");
    }

    private void RemoveTargettingCookies()
    {
        // No targetting cookies currently set  
    }
}
