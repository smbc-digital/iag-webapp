namespace StockportWebapp.Utils.Extensions;

public static class StringExtensions
{
    public static string StripHttpAndHttps(this string target) =>
        target.TrimStart("https://").TrimStart("http://");

    public static string TrimStart(this string target, string trimString)
    {
        string result = target;

        while (result.StartsWith(trimString))
        {
            result = result.Substring(trimString.Length);
        }

        return result;
    }
}