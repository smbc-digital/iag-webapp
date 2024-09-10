using System.Runtime.CompilerServices;
using HtmlAgilityPack;

namespace StockportWebapp.Utils;

public class HtmlHelper
{
    public static string AddImageAttributes(string htmlContent, string width, string height, string maxMobileWidth, string maxTabletWidth, string maxDesktopWidth)
    {
        HtmlDocument document = new();
        document.LoadHtml(htmlContent);
        var imageNodes = document.DocumentNode.SelectNodes("//img");

        if(imageNodes is null)
            return htmlContent;
        
        foreach (HtmlNode image in imageNodes)
        {
            string src = image.GetAttributeValue("src", string.Empty);
            if (string.IsNullOrEmpty(src)) 
                continue;

            SetSrcAttribute(src, image);
            SetSrcsetAndSizesAttribute(src, image, maxMobileWidth, maxTabletWidth, maxDesktopWidth);

            image.SetAttributeValue("width", image.GetAttributeValue("width", width));
            image.SetAttributeValue("height", image.GetAttributeValue("height", height));
            image.SetAttributeValue("loading", image.GetAttributeValue("loading", "lazy"));
        }

        return document.DocumentNode.OuterHtml;
    }

    private static void SetSrcAttribute(string src, HtmlNode image) =>
        image.SetAttributeValue("src", src += "?q=89&fm=webp");

    private static void SetSrcsetAndSizesAttribute(string src, HtmlNode image, string maxMobileWidth, string maxTabletWidth, string maxDesktopWidth)
    {
        string baseUrl = src.Split('?')[0];
        string srcset = $"{baseUrl}?w={maxMobileWidth}&q=89&fm=webp {maxMobileWidth}w, " +
                    $"{baseUrl}?w={maxTabletWidth}&q=89&fm=webp {maxTabletWidth}w, " +
                    $"{baseUrl}?w={maxDesktopWidth}&q=89&fm=webp {maxDesktopWidth}w";
        image.SetAttributeValue("srcset", srcset);

        string sizes = $"(max-width: 767px) {maxMobileWidth}px, " +
                    $"(min-width: 768px) and (max-width: 1023px) {maxTabletWidth}px, " +
                    $"(min-width: 1024px) {maxDesktopWidth}px";
        image.SetAttributeValue("sizes", sizes);
    }
}