using System.Linq;
using AngleSharp.Parser.Html;
using AngleSharp.Extensions;

namespace StockportWebapp.Utils
{
    public interface IHtmlUtilities
    {
        string ConvertRelativeUrltoAbsolute(string htmlText, string site);
    }

    public class HtmlUtilities : IHtmlUtilities
    {
        public string ConvertRelativeUrltoAbsolute(string htmlText, string site)
        {
            var parser = new HtmlParser();
            var document = parser.Parse(htmlText);

            var relativeHyperlinks = document.All.Where(l => l.HasAttribute("href"));
            var relativeSrcs = document.All.Where(l => l.HasAttribute("src"));
            foreach (var link in relativeHyperlinks)
            {
                var href = link.Attributes["href"].Value;
                if (href.StartsWith("/") && !href.StartsWith("//"))
                    link.Attributes["href"].Value = site + href;
            }
            foreach (var src in relativeSrcs)
            {
                var srcVal = src.Attributes["src"].Value;
                if (srcVal.StartsWith("/") && !srcVal.StartsWith("//"))
                    src.Attributes["src"].Value = site + srcVal;
            }

            return document.ToHtml();
        }
    }
}
