using System.Text;
using System.Text.RegularExpressions;

namespace StockportWebapp.Parsers
{
    public class CarouselTagParser : ISimpleTagParser
    {
        private readonly TagReplacer _tagReplacer;
        protected Regex TagRegex => new Regex("{{CAROUSEL:(.*)}}", RegexOptions.Compiled);

        protected string GenerateHtml(string tagData)
        {
            tagData = tagData.Replace("{{CAROUSEL:", "");
            tagData = tagData.Replace("}}", "");

            string[] tagArray = tagData.Split(',');

            var altRegex = new Regex(@"\[([^\]]*)\]");
            var srcRegex = new Regex(@"\(([^\)]*)\)");

            StringBuilder returnCarousel = new StringBuilder("<div class='carousel'>");

            if (tagArray[0] != "")
            {
                foreach (var item in tagArray)
                {
                    var doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(item);

                    if (doc.DocumentNode.SelectSingleNode("//img") != null)
                    {
                        var srcTxt = doc.DocumentNode.SelectSingleNode("//img").Attributes["src"];
                        var altTxt = doc.DocumentNode.SelectSingleNode("//img").Attributes["alt"];

                        if (!string.IsNullOrEmpty(srcTxt.Value))
                            returnCarousel.Append(
                                $"<div class=\"carousel-image stockport-carousel\" style=\"background-image:url({srcTxt.Value});\" title=\"{altTxt.Value}\" /><div class=\"stockport-carousel-text article-carousel-text\"><p class=\"carousel-text\">{altTxt.Value}</p></div></div>");
                    }
                    else
                    {
                        var srcText = srcRegex.Match(item).Groups[1];
                        var altText = altRegex.Match(item).Groups[1];
                        if (!string.IsNullOrEmpty(srcText.Value))
                            returnCarousel.Append(
                                $"<div class=\"carousel-image stockport-carousel\" style=\"background-image:url({srcText});\" title=\"{altText}\" /><div class=\"stockport-carousel-text article-carousel-text\"><p class=\"carousel-text\">{altText}</p></div></div>");
                    }
                }
            }
            return returnCarousel.Append("</div>").ToString();
        }

        public CarouselTagParser()
        {
            _tagReplacer = new TagReplacer(GenerateHtml, TagRegex);
        }

        public string Parse(string body, string title = null)
        {
            return _tagReplacer.ReplaceAllTags(body);
        }
    }
}