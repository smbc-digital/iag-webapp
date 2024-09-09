namespace StockportWebapp.TagParsers;

public class VideoTagParser : ISimpleTagParser
{
    private readonly TagReplacer _tagReplacer;
    private readonly IViewRender _viewRenderer;

    protected Regex TagRegex => new("{{VIDEO:([0-9aA-zZ]*;[0-9aA-zZ]*;?[0-9aA-zZ '#+]*)}}", RegexOptions.Compiled);

    public VideoTagParser(IViewRender viewRenderer)
    {
        _viewRenderer = viewRenderer;
        _tagReplacer = new TagReplacer(GenerateHtml, TagRegex);
    }

    protected string GenerateHtml(string tagData)
    {
        var videoData = tagData.Split(';');
        var iframe = _viewRenderer.Render("VideoIFrame", new VideoViewModel(videoData[2], videoData[1], videoData[0]));
        return $"<div class=\"video-wrapper\">{iframe}</div>";
    }

    public string Parse(string body, string title = null) => _tagReplacer.ReplaceAllTags(body);
}