namespace StockportWebapp.Utils.Extensions;

public static class StringExtensions
{
    public static string StripHttpAndHttps(this string target) =>
        target.TrimStart("https://").TrimStart("http://");

    public static string StripEmojis(this string input) =>
        Regex.Replace(input, @"(?![\u00A3])[^\u0000-\u007F]+", string.Empty);

    public static string TrimStart(this string target, string trimString)
    {
        string result = target;

        while (result.StartsWith(trimString))
        {
            result = result.Substring(trimString.Length);
        }

        return result;
    }

    public static string TrimEnd(this string source, string value)
    {
        if (!source.EndsWith(value))
            return source;

        return source.Remove(source.LastIndexOf(value));
    }
}