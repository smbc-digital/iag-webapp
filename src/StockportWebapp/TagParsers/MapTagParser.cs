namespace StockportWebapp.TagParsers;

public class MapTagParser : ISimpleTagParser
{
    private readonly TagReplacer _tagReplacer;
    protected Regex TagRegex => new Regex("{{MAP:(.*)}}", RegexOptions.Compiled);

    public MapTagParser()
    {
        _tagReplacer = new TagReplacer(GenerateHtml, TagRegex);
    }

    public string GenerateHtml(string tagData)
    {
        tagData.Replace("{{MAP:", string.Empty);
        tagData.Replace("}}", string.Empty);

        return $"<div id='root' class='leaflet-map'></div><script type='text/javascript' src='{tagData}/main-latest.js'></script><script type='text/javascript' src='{tagData}/vendor-latest.js'></script>";
    }

    public string Parse(string body, string title = null)
    {
        return _tagReplacer.ReplaceAllTags(body);
    }


}
