using System.Web;
using HtmlAgilityPack;

namespace StockportWebapp.Utils;

[ExcludeFromCodeCoverage]
public class HtmlHelper
{
    public static string AddImageAttributes(string htmlContent, string width, string height, string maxMobileWidth, string maxTabletWidth, string maxDesktopWidth)
    {
        HtmlDocument document = new();
        document.LoadHtml(htmlContent);
        HtmlNodeCollection imageNodes = document.DocumentNode.SelectNodes("//img");

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

    private static void SetSrcAttribute(string src, HtmlNode image) 
    {
        // Hack - issue with reference being lative not being understood bu URLs full - need to make this an absolute URL
        if(src.StartsWith("//"))
            src = $"https:{src}";

        Uri uri = new Uri(src);
        UriBuilder uriBuilder = new UriBuilder(uri);
        var queryValues = HttpUtility.ParseQueryString(uriBuilder.Query);

        if(!queryValues.AllKeys.Contains("q"))
        {
            queryValues.Add("q", "89");
        }

        if(!queryValues.AllKeys.Contains("fm"))
        {
            queryValues.Add("fm", "webp");
        }
        
        uriBuilder.Query = queryValues.ToString();

        string noScheme = uri.GetComponents(UriComponents.AbsoluteUri & ~UriComponents.Scheme, UriFormat.SafeUnescaped);
        
        image.SetAttributeValue("src", $"//{noScheme}");
    }
        
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