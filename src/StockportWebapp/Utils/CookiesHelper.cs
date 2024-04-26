namespace StockportWebapp.Utils;

public class CookiesHelper : ICookiesHelper
{
    private IHttpContextAccessor httpContextAccessor;

    public CookiesHelper(IHttpContextAccessor accessor)
    {
        httpContextAccessor = accessor;
    }

    public List<T> PopulateCookies<T>(List<T> items, string cookieType)
    {
        var cookiesAsObject = GetCookiesAsObject(cookieType);

        if (!cookiesAsObject.Keys.Any()) return items;

        var type = typeof(T).ToString().ToLower().Replace("Processed", "");

        var cookies = cookiesAsObject[type];

        foreach (var item in items)
        {
            PropertyInfo cookieProp = item.GetType().GetProperty("Favourite");
            PropertyInfo slugProp = item.GetType().GetProperty("Slug");

            if (cookieProp is not null && slugProp is not null && cookieProp.CanWrite)
            {
                var exists = cookies.Any(f => f == slugProp.GetValue(item).ToString());
                cookieProp.SetValue(item, exists, null);
            }
            else
            {
                throw new Exception("The object you are adding to favourites does not have either the property 'Favourite' or the property 'Slug'");
            }
        }

        return items;
    }

    public void AddToCookies<T>(string slug, string cookieType)
    {
        var cookiesAsObject = GetCookiesAsObject(cookieType);
        string key = typeof(T).ToString().ToLower();

        if (!cookiesAsObject.ContainsKey(key))
            cookiesAsObject[key] = new List<string>();

        if (cookiesAsObject.ContainsKey(key) && slug is not null && !cookiesAsObject[key].Contains(slug))
            cookiesAsObject[key].Add(slug);

        UpdateCookies(cookiesAsObject, cookieType);
    }

    public void RemoveFromCookies<T>(string slug, string cookieType)
    {
        var cookiesAsObject = GetCookiesAsObject(cookieType);
        string key = typeof(T).ToString().ToLower();

        if (!cookiesAsObject.ContainsKey(key))
            cookiesAsObject.Add(key, new List<string>());

        if (cookiesAsObject.ContainsKey(key) && cookiesAsObject[key].Contains(slug))
            cookiesAsObject[key].Remove(slug);

        UpdateCookies(cookiesAsObject, cookieType);
    }

    public void RemoveAllFromCookies<T>(string cookieType)
    {
        var cookiesAsObject = GetCookiesAsObject(cookieType);

        if (cookiesAsObject.ContainsKey(typeof(T).ToString().ToLower()))
            cookiesAsObject.Remove(typeof(T).ToString().ToLower());

        UpdateCookies(cookiesAsObject, cookieType); 
    }

    public void RemoveCookie(string key)
    {        
        var cookie = httpContextAccessor.HttpContext.Request.Cookies[key];
        if (string.IsNullOrEmpty(cookie))
            return;

        httpContextAccessor.HttpContext.Response.Cookies.Append(key, string.Empty, new CookieOptions { Expires = DateTime.Now.AddDays(-1) });
    }

    public void RemoveCookiesStartingWith(string startKey)
    {
        var cookies = httpContextAccessor.HttpContext.Request.Cookies.Where(cookie => cookie.Key.StartsWith(startKey));
        if (!cookies.Any())
            return;

        cookies.ToList().ForEach(cookie => RemoveCookie(cookie.Key));
    }

    public List<string> GetCookies<T>(string cookieType) =>
        GetCookiesAsObject(cookieType).Values.SelectMany(cookieAsObject => cookieAsObject.Select(cookie => cookie.ToString())).ToList();

    public static Dictionary<string, List<string>> ExtractValuesFromJson(string cookies)
    {
        Dictionary<string, List<string>> alertDictionary = new Dictionary<string, List<string>>();
        JObject cookiesObject = JObject.Parse(cookies);

        foreach (var property in cookiesObject.Properties())
        {
            string key = property.Name;
            List<string> values = property.Value.ToObject<List<string>>();
            alertDictionary.Add(key, values);
        }

        return alertDictionary;
    }

    private Dictionary<string, List<string>> GetCookiesAsObject(string cookieType)
    {

        string cookies = httpContextAccessor.HttpContext.Request.Cookies[cookieType];
        Dictionary<string, List<string>> alertDictionary = new();
        
        if (cookies is not null && !cookies.Equals(string.Empty))
            alertDictionary = ExtractValuesFromJson(cookies);

        return alertDictionary;
    }

    private void UpdateCookies(Dictionary<string, List<string>> cookies, string cookieType)
    {
        var data = JsonConvert.SerializeObject(cookies);
        httpContextAccessor.HttpContext.Response.Cookies.Append(cookieType, data, new CookieOptions { Expires = DateTime.Now.AddMonths(1) });
    }

    public bool HasCookieConsentBeenCollected()
    {
        var consentAcctepted = httpContextAccessor.HttpContext.Request.Cookies["cookie_consent_user_accepted"];
        return !string.IsNullOrEmpty(consentAcctepted);
    }

    public CookieConsentLevel GetCurrentCookieConsentLevel()
    {
        var consentAcceptedValue = httpContextAccessor.HttpContext.Request.Cookies["cookie_consent_user_accepted"];

        if (string.IsNullOrEmpty(consentAcceptedValue))
            return new CookieConsentLevel();

        if (!bool.Parse(consentAcceptedValue))
            return new CookieConsentLevel();

        var consentLevel = httpContextAccessor.HttpContext.Request.Cookies["cookie_consent_level"];

        return string.IsNullOrEmpty(consentLevel)
            ? new CookieConsentLevel()
            : JsonConvert.DeserializeObject<CookieConsentLevel>(consentLevel);
    }
}
