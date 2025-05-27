namespace StockportWebapp.Utils;

public interface ICookiesHelper
{
    void AddToCookies<T>(string slug, string cookieType);
    List<string> GetCookies<T>(string cookieType);
    void RemoveAllFromCookies<T>(string cookieType);
    void RemoveFromCookies<T>(string slug, string cookieType);
    CookieConsentLevel GetCurrentCookieConsentLevel();
    bool HasCookieConsentBeenCollected();
    void RemoveCookie(string key);
    void RemoveCookiesStartingWith(string startKey);
}