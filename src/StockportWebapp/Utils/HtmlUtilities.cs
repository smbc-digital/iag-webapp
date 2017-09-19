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
        private readonly HtmlParser _htmlParser;

        public HtmlUtilities(HtmlParser htmlParser)
        {
            _htmlParser = htmlParser;
        }

        public string ConvertRelativeUrltoAbsolute(string htmlText, string site)
        {
            var document = _htmlParser.Parse(htmlText);

            var relativeHyperlinks = document.All.Where(l => l.HasAttribute("href"));
            var relativeSrcs = document.All.Where(l => l.HasAttribute("src"));
            var relativeDataMain = document.All.Where(l => l.HasAttribute("data-main"));
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
            foreach (var data in relativeDataMain)
            {
                var dataVal = data.Attributes["data-main"].Value;
                if (dataVal.StartsWith("/") && !dataVal.StartsWith("//"))
                    data.Attributes["data-main"].Value = site + dataVal;
            }

            return document.ToHtml();
        }
    }
}
