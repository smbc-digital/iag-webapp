using HtmlAgilityPack;

namespace StockportWebapp.TagParsers;

public class CarouselTagParser : ISimpleTagParser
{
    private readonly TagReplacer _tagReplacer;
    protected Regex TagRegex => new("{{CAROUSEL:(.*)}}", RegexOptions.Compiled);
    private readonly IViewRender _viewRenderer;

    public CarouselTagParser(IViewRender viewRenderer)
    {
        _viewRenderer = viewRenderer;
        _tagReplacer = new TagReplacer(GenerateHtml, TagRegex);
    }
    
    protected string GenerateHtml(string tagData)
    {
        tagData = tagData.Replace("{{CAROUSEL:", string.Empty);
        tagData = tagData.Replace("}}", string.Empty);

        string[] tagArray = tagData.Split(',');

        List<(string Src, string Alt)> carouselItems = new();

        foreach (string item in tagArray)
        {
            HtmlDocument doc = new();
            doc.LoadHtml(item);

            // Attempt to extract the src and alt attributes
            string src = doc.DocumentNode.SelectSingleNode("//img")?.GetAttributeValue("src", string.Empty) ?? string.Empty;
            string alt = doc.DocumentNode.SelectSingleNode("//img")?.GetAttributeValue("alt", string.Empty) ?? string.Empty;

            if (!string.IsNullOrEmpty(src))
            {
                carouselItems.Add((src, alt));
            }
            else
            {
                // Regex fallback for (src) and [alt] syntax
                Regex srcRegex = new(@"\(([^\)]*)\)");
                Regex altRegex = new(@"\[([^\]]*)\]");

                var srcText = srcRegex.Match(item).Groups[1].Value;
                var altText = altRegex.Match(item).Groups[1].Value;

                if (!string.IsNullOrEmpty(srcText))
                    carouselItems.Add((srcText, altText));
            }
        }

        return _viewRenderer.Render("CarouselTagParserContent", carouselItems);
    }

    public string Parse(string body, string title = null) =>
        _tagReplacer.ReplaceAllTags(body);
}