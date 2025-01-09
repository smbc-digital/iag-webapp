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
        string[] videoData = tagData.Split(';');
        VideoViewModel videoViewModel = new(videoData[1], videoData[0])
        {
            Title = videoData.Length > 2
                        ? videoData[2]
                        : string.Empty
        };

        return $"<div class=\"video-wrapper\">{_viewRenderer.Render("VideoIFrame", videoViewModel)}</div>";
    }

    public string Parse(string body, string title = null) =>
        _tagReplacer.ReplaceAllTags(body);
}