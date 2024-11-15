using HtmlAgilityPack;

namespace StockportWebapp.TagParsers;

public class InlineCarouselTagParser : ISimpleTagParser
{
    private readonly TagReplacer _tagReplacer;
    protected Regex TagRegex => new("{{Carousel-Inline:(.*)}}", RegexOptions.Compiled);
    private readonly IViewRender _viewRenderer;

    public InlineCarouselTagParser(IViewRender viewRenderer)
    {
        _viewRenderer = viewRenderer;
        _tagReplacer = new TagReplacer(GenerateHtml, TagRegex);
    }

    protected string GenerateHtml(string tagData)
    {
        tagData = tagData.Replace("{{Carousel-Inline:", string.Empty);
        tagData = tagData.Replace("}}", string.Empty);

        string[] tagArray = tagData.Split(',');

        List<(string Src, string Alt)> carouselItems = new();
        // Regex fallback for (src) and [alt] syntax
        Regex srcRegex = new(@"\(([^\)]*)\)");
        Regex altRegex = new(@"\[([^\]]*)\]");

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
                string srcText = srcRegex.Match(item).Groups[1].Value;
                string altText = altRegex.Match(item).Groups[1].Value;

                if (!string.IsNullOrEmpty(srcText))
                    carouselItems.Add((srcText, altText));
            }
        }

        return _viewRenderer.Render("CarouselTagParserContent", carouselItems);
    }

    public string Parse(string body, string title = null) =>
        _tagReplacer.ReplaceAllTags(body);
}