using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.ApplicationInsights.Extensibility;

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

            var altRegex = new Regex(@"\[([^\]]*)\]");
            var srcRegex = new Regex(@"\(([^\)]*)\)");
            string[] tagArray = tagData.Split(',');

            StringBuilder returnCarousel = new StringBuilder("<div class='carousel'>");

            foreach (var item in tagArray)
            {

                var srcText = srcRegex.Match(item).Groups[1];
                var altText = altRegex.Match(item).Groups[1];
                if(!string.IsNullOrEmpty(srcText.Value))
                    returnCarousel.Append($"<div class=\"carousel-image stockport-carousel\" style=\"background-image:url({srcText});\" title=\"{altText}\" /><div class=\"stockport-carousel-text article-carousel-text\"><p class=\"carousel-text\">{altText}</p></div></div>");

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