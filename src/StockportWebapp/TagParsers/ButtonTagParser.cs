namespace StockportWebapp.TagParsers;

public class ButtonTagParser : ISimpleTagParser
{
    private readonly TagReplacer _tagReplacer;
    protected Regex TagRegex => new("{{BUTTON:(\\s*[/a-zA-Z0-9][^}]+)}}", RegexOptions.Compiled);
    private const string buttonClassStyle = "btn button button-hs button-primary button-outline button-partialrounded btn--chevron-forward";

    public string GenerateHtml(string tagData)
    {
        var commaSplitString = tagData.Split(new[] { ',' }, 2);
        var thereIsLinkText = commaSplitString.Length.Equals(2);
        var link = tagData;
        var title = tagData;

        if (thereIsLinkText)
        {
            link = commaSplitString[0].Trim();
            title = commaSplitString[1].Trim();
        }

        return $"<a class=\"{buttonClassStyle}\" href=\"{link}\">{title}</a>";
    }

    public ButtonTagParser()
        => _tagReplacer = new TagReplacer(GenerateHtml, TagRegex);

    public string Parse(string body, string title = null) 
        => _tagReplacer.ReplaceAllTags(body);
}