using System.Text.RegularExpressions;

namespace StockportWebapp.Extensions
{
    public static class StringExtensions
    {
        public static string StripHttpAndHttps(this string target)
        {
            return target.TrimStart("https://").TrimStart("http://");
        }

        public static string StripEmojis(this string input)
        {
            return Regex.Replace(input, @"[^\u0000-\u007F]+", "");
        }

        private static string TrimStart(this string target, string trimString)
        {
            var result = target;

            while (result.StartsWith(trimString))
            {
                result = result.Substring(trimString.Length);
            }

            return result;
        }
    }
}
