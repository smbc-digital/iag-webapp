namespace StockportWebapp.Extensions
{
    public class SocialLinksExtension
    {
        public string GetSubstring(string stringUrl)
        {
            stringUrl = stringUrl.ToLower();
            var facebook = "facebook.com/";
            var twitter = "twitter.com/";
            int urlIndex = 0;
            var result = "";

            if (stringUrl.Contains(facebook))
            {
                urlIndex = facebook.Length + stringUrl.IndexOf(facebook);
                result = "/" + stringUrl.Remove(0, urlIndex);
            }

            if (stringUrl.Contains(twitter))
            {
                urlIndex = twitter.Length + stringUrl.IndexOf(twitter);
                result = "@" + stringUrl.Remove(0, urlIndex);
            } 

            return result;
        }
    }
}
