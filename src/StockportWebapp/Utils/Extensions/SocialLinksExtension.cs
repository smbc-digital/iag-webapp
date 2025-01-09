namespace StockportWebapp.Utils.Extensions;

public class SocialLinksExtension
{
    public string GetSubstring(string stringUrl)
    {
        stringUrl = stringUrl.ToLower();
        string facebook = "facebook.com/";
        string twitter = "twitter.com/";
        string result = "";

        int urlIndex;
        if (stringUrl.Contains(facebook))
        {
            urlIndex = facebook.Length + stringUrl.IndexOf(facebook);
            result = $"/{stringUrl.Remove(0, urlIndex)}";
        }

        if (stringUrl.Contains(twitter))
        {
            urlIndex = twitter.Length + stringUrl.IndexOf(twitter);
            result = $"@{stringUrl.Remove(0, urlIndex)}";
        }

        return result;
    }
}