﻿namespace StockportWebapp.Utils.Extensions;

public static class StringExtensions
{
    public static string StripHttpAndHttps(this string target)
    {
        return target.TrimStart("https://").TrimStart("http://");
    }

    public static string StripEmojis(this string input)
    {
        return Regex.Replace(input, @"(?![\u00A3])[^\u0000-\u007F]+", "");
    }

    public static string TrimStart(this string target, string trimString)
    {
        var result = target;

        while (result.StartsWith(trimString))
        {
            result = result.Substring(trimString.Length);
        }

        return result;
    }

    public static string TrimEnd(this string source, string value)
    {
        if (!source.EndsWith(value))
        {
            return source;
        }

        return source.Remove(source.LastIndexOf(value));
    }
}
