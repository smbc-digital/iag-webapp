namespace StockportWebapp.TagParsers;

public class IFrameTagParser : ISimpleTagParser
{
    private readonly TagReplacer _tagReplacer;
    protected Regex TagRegex => new("{{IFRAME:(.*)}}", RegexOptions.Compiled);

    public string GenerateHtml(string tagData)
    {
        tagData.Replace("{{IFRAME:", string.Empty);
        tagData.Replace("}}", string.Empty);

        Regex ValidUrl = new(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$");

        string[] splitTagData = tagData.Split(";");
        if (!ValidUrl.IsMatch(splitTagData[0]))
            return null;

        string iFrameTitle = string.Empty;

        if (splitTagData.Length > 1)
            iFrameTitle = $"title=\"{splitTagData[1]}\"";

        return $"<iframe {iFrameTitle} class='mapframe' allowfullscreen src='{splitTagData[0]}'></iframe>";
    }

    public IFrameTagParser() =>
        _tagReplacer = new TagReplacer(GenerateHtml, TagRegex);

    public string Parse(string body, string title = null) =>
        _tagReplacer.ReplaceAllTags(body);
}