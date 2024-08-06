﻿namespace StockportWebapp.TagParsers;

public class CarouselTagParser : ISimpleTagParser
{
    private readonly TagReplacer _tagReplacer;
    protected Regex TagRegex => new("{{CAROUSEL:(.*)}}", RegexOptions.Compiled);

    protected string GenerateHtml(string tagData)
    {
        tagData = tagData.Replace("{{CAROUSEL:", "");
        tagData = tagData.Replace("}}", "");

        string[] tagArray = tagData.Split(',');

        Regex altRegex = new(@"\[([^\]]*)\]");
        Regex srcRegex = new(@"\(([^\)]*)\)");

        StringBuilder returnCarousel = new("<div class='carousel'>");

        if (tagArray[0] != "")
        {
            foreach (string item in tagArray)
            {
                HtmlAgilityPack.HtmlDocument doc = new();
                doc.LoadHtml(item);

                if (doc.DocumentNode.SelectSingleNode("//img") != null)
                {
                    HtmlAgilityPack.HtmlAttribute srcTxt = doc.DocumentNode.SelectSingleNode("//img").Attributes["src"];
                    HtmlAgilityPack.HtmlAttribute altTxt = doc.DocumentNode.SelectSingleNode("//img").Attributes["alt"];

                    if (!string.IsNullOrEmpty(srcTxt.Value))
                        returnCarousel.Append(
                            $"<div class=\"carousel-image stockport-carousel\" style=\"background-image:url({srcTxt.Value});\" title=\"{altTxt.Value}\" /><div class=\"stockport-carousel-text article-carousel-text\"><p class=\"carousel-text\">{altTxt.Value}</p></div></div>");
                }
                else
                {
                    System.Text.RegularExpressions.Group srcText = srcRegex.Match(item).Groups[1];
                    System.Text.RegularExpressions.Group altText = altRegex.Match(item).Groups[1];
                    if (!string.IsNullOrEmpty(srcText.Value))
                        returnCarousel.Append(
                            $"<div class=\"carousel-image stockport-carousel\" style=\"background-image:url({srcText});\" title=\"{altText}\" /><div class=\"stockport-carousel-text article-carousel-text\"><p class=\"carousel-text\">{altText}</p></div></div>");
                }
            }
        }
        string scriptTag = "<script>\r\nrequire(['/assets/javascript/config-830b1ebe.min.js'],function(){\r\nrequire(['slick', 'carousel'],\r\nfunction(_, carousel){\r\ncarousel.Init();\r\n}\r\n);\r\n});\r\n</script>";
        return returnCarousel.Append("</div>" + scriptTag).ToString();
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