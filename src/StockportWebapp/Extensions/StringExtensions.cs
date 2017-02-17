using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockportWebapp.Extensions
{
    public static class StringExtensions
    {
        public static string StripHttpAndHttps(this string target)
        {
            return target.TrimStart("https://").TrimStart("http://");
        }

        private static string TrimStart(this string target, string trimString)
        {
            string result = target;

            while (result.StartsWith(trimString))
            {
                result = result.Substring(trimString.Length);
            }

            return result;
        }
    }
}
