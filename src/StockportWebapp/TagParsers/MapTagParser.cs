namespace StockportWebapp.TagParsers;

public class MapTagParser : ISimpleTagParser
{
    private readonly TagReplacer _tagReplacer;
    private readonly IViewRender _viewRenderer;
    protected Regex TagRegex => new("{{MAP:(.*)}}", RegexOptions.Compiled);

    public MapTagParser(IViewRender viewRenderer)
    {
        _viewRenderer = viewRenderer;
        _tagReplacer = new TagReplacer(GenerateHtml, TagRegex);
    }

    public string GenerateHtml(string tagData)
    {
        tagData = tagData.Replace("{{MAP:", string.Empty)
                    .Replace("}}", string.Empty);

        MapViewModel mapViewModel = new(tagData);
        return _viewRenderer.Render("MapContent", mapViewModel);
    }

    public string Parse(string body, string title = null) =>
        _tagReplacer.ReplaceAllTags(body);
}